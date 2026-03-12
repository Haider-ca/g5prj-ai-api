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
    await authApi.register({
      email: emailInput.value.trim(),
      password: passwordInput.value,
      role: roleInput.value
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
