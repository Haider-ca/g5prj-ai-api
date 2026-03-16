import { AuthApiService } from "../services/auth-api.js";
import { UiStrings } from "../constants/strings.js";
import { byId, setDisabled, setMessage } from "../utils/dom.js";

const authApi = new AuthApiService();
const registerForm = byId("registerForm");
const emailInput = byId("email");
const passwordInput = byId("password");
const roleInput = byId("role");
const formMessage = byId("formMessage");

registerForm.addEventListener("submit", async (event) => {
  event.preventDefault();
  setDisabled(registerForm.querySelector("button[type='submit']"), true);
  setMessage(formMessage, "");

  try {
    const email = emailInput.value.trim();
    const password = passwordInput.value;
    const role = roleInput.value;

    // Mirror backend validation rules for better UX
    const isEmailValid = email && email.includes("@");
    const isPasswordValid = password && password.length >= 3;
    const isRoleValid = role === "user" || role === "admin";

    if (!isEmailValid || !isPasswordValid || !isRoleValid) {
      setMessage(formMessage, UiStrings.registerValidationError, "error");
      return;
    }

    await authApi.register({
      email,
      password,
      role
    });

    registerForm.reset();
    roleInput.value = "user";
    setMessage(formMessage, UiStrings.registerSuccess, "success");
  } catch (error) {
    setMessage(formMessage, error.message || UiStrings.genericError, "error");
  } finally {
    setDisabled(registerForm.querySelector("button[type='submit']"), false);
  }
});
