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
const emailError = byId("emailError");
const passwordError = byId("passwordError");

function validateEmail() {
  const email = emailInput.value.trim();
  const isValid = email && email.includes("@");
  if (emailError) {
    emailError.textContent = isValid ? "" : UiStrings.registerEmailError;
  }
  return isValid;
}

function validatePassword() {
  const password = passwordInput.value;
  const isValid = password && password.length >= 8;
  if (passwordError) {
    passwordError.textContent = isValid ? "" : UiStrings.registerPasswordError;
  }
  return isValid;
}

emailInput.addEventListener("blur", validateEmail);
passwordInput.addEventListener("blur", validatePassword);

loginForm.addEventListener("submit", async (event) => {
  event.preventDefault();
  setDisabled(loginForm.querySelector("button[type='submit']"), true);
  setMessage(formMessage, "");

  try {
    const email = emailInput.value.trim();
    const password = passwordInput.value;

    // Basic frontend validation to match backend expectations
    const isEmailValid = email && email.includes("@");
    const isPasswordValid = password && password.length >= 8;

    if (!isEmailValid || !isPasswordValid) {
      validateEmail();
      validatePassword();
      return;
    }

    const payload = await authApi.login({
      email,
      password
    });

    let currentUser = payload?.user;

    try {
      const mePayload = await authApi.getCurrentUser();
      currentUser = authApi.normalizeCurrentUser(mePayload);
    } catch {
      if (currentUser) {
        currentUser = authApi.normalizeCurrentUser(currentUser);
      }
    }

    if (!currentUser) {
      throw new Error(UiStrings.genericError);
    }

    sessionController.saveSession({
      user: currentUser
    });
    setMessage(formMessage, UiStrings.loginSuccess, "success");
    window.setTimeout(() => {
      sessionController.redirectToRoleHome(currentUser.role);
    }, 500);
  } catch (error) {
    if (error.message === "Invalid email or password.") {
      setMessage(formMessage, "Invalid email or password.", "error");
    } else {
      setMessage(formMessage, error.message || UiStrings.genericError, "error");
    }
  } finally {
    setDisabled(loginForm.querySelector("button[type='submit']"), false);
  }
});
