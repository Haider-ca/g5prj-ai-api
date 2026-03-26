import { AuthApiService } from "../services/auth-api.js";
import { AiApiService } from "../services/ai-api.js";
import { SessionController } from "../services/session-controller.js";
import { UiStrings } from "../constants/strings.js";
import { AppConfig } from "../config.js";
import { byId, setMessage, setText } from "../utils/dom.js";

const sessionController = new SessionController();
const authApi = new AuthApiService();
const aiApi = new AiApiService();
const session = sessionController.requireRole(AppConfig.roles.user);

if (session) {
  const currentUserLabel = byId("currentUserLabel");
  const roleValue = byId("roleValue");
  const remainingCallsValue = byId("remainingCallsValue");
  const responseRemainingCallsValue = byId("responseRemainingCallsValue");
  const scoreValue = byId("scoreValue");
  const feedbackValue = byId("feedbackValue");
  const modelValue = byId("modelValue");
  const evaluateForm = byId("evaluateForm");
  const evaluateMessage = byId("evaluateMessage");
  const questionInput = byId("question");
  const studentAnswerInput = byId("studentAnswer");
  const loadUsageButton = byId("loadUsageButton");
  const logoutButton = byId("logoutButton");

  setText(currentUserLabel, session.user.email || UiStrings.signedInUserLabel);
  setText(roleValue, session.user.role);

  logoutButton.addEventListener("click", () => {
    sessionController.clearSession();
    window.location.href = AppConfig.pages.login;
  });

  loadUsageButton.addEventListener("click", async () => {
    await loadUsage();
  });

  evaluateForm.addEventListener("submit", async (event) => {
    event.preventDefault();
    setMessage(evaluateMessage, "");

    try {
      const payload = await aiApi.evaluateAnswer(session.token, {
        question: questionInput.value.trim(),
        studentAnswer: studentAnswerInput.value.trim()
      });

      const result = aiApi.normalizeEvaluation(payload);
      setText(scoreValue, String(result.score));
      setText(feedbackValue, result.feedback);
      setText(modelValue, result.model);
      setText(responseRemainingCallsValue, String(result.remainingCalls));
      setText(remainingCallsValue, String(result.remainingCalls));
      setMessage(evaluateMessage, UiStrings.evaluationSuccess, "success");
    } catch (error) {
      setMessage(evaluateMessage, error.message || UiStrings.genericError, "error");
    }
  });

  async function loadUsage() {
    try {
      const payload = await authApi.getUsage(session.token);
      setText(remainingCallsValue, String(payload.remainingCalls));
      setMessage(evaluateMessage, UiStrings.usageLoaded, "success");
    } catch (error) {
      setMessage(evaluateMessage, error.message || UiStrings.genericError, "error");
    }
  }

  loadUsage();
}
