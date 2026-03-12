import { AuthApiService } from "../services/auth-api.js";
import { UiStrings } from "../constants/strings.js";
import { byId, setDisabled, setMessage } from "../utils/dom.js";
import { SessionController } from "../services/session-controller.js";

const authApi = new AuthApiService();
const sessionController = new SessionController();

const loginForm = byId("loginForm");
const emailInput = byId("email");
const passwordInput = byId("password");
const formMessage = byId("formMessage");

loginForm.addEventListener("submit", async (event) => {
  event.preventDefault();
  setDisabled(loginForm.querySelector("button[type='submit']"), true);
  setMessage(formMessage, "");

  try {
    const payload = await authApi.login({
      email: emailInput.value.trim(),
      password: passwordInput.value
    });

    const session = authApi.normalizeLoginResult(payload, emailInput.value.trim());
    if (!session.token) {
      throw new Error("Login succeeded but no JWT token was returned.");
    }

    sessionController.saveSession(session);
    setMessage(formMessage, UiStrings.loginSuccess, "success");
    window.setTimeout(() => {
      sessionController.redirectToRoleHome(session.user.role);
    }, 500);
  } catch (error) {
    setMessage(formMessage, error.message || UiStrings.genericError, "error");
  } finally {
    setDisabled(loginForm.querySelector("button[type='submit']"), false);
  }
});
