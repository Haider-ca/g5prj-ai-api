import { AppConfig } from "../config.js";
import { HttpClient } from "./http-client.js";

export class AuthApiService {
  constructor(httpClient = new HttpClient()) {
    this.httpClient = httpClient;
  }

  async register({ email, password, role }) {
    return this.httpClient.request(this.#buildUrl(AppConfig.endpoints.register), {
      method: "POST",
      headers: this.#jsonHeaders(),
      body: JSON.stringify({ email, password, role })
    });
  }

  async login({ email, password }) {
    return this.httpClient.request(this.#buildUrl(AppConfig.endpoints.login), {
      method: "POST",
      headers: this.#jsonHeaders(),
      body: JSON.stringify({ email, password })
    });
  }

  async logout() {
    return this.httpClient.request(this.#buildUrl("/logout"), {
      method: "POST"
    });
  }

  async getCurrentUser() {
    return this.httpClient.request(this.#buildUrl(AppConfig.endpoints.me), {
      method: "GET"
    });
  }

  async getUsage() {
    return this.httpClient.request(this.#buildUrl(AppConfig.endpoints.usage), {
      method: "GET"
    });
  }

  async getAdminUsers() {
    return this.httpClient.request(this.#buildUrl(AppConfig.endpoints.adminUsers), {
      method: "GET"
    });
  }

  normalizeAdminUsers(payload) {
    return Array.isArray(payload) ? payload : (payload.users ?? []);
  }

  normalizeUsage(payload) {
    return {
      remainingCalls: payload.remainingCalls ?? payload.callsRemaining ?? "--"
    };
  }

  #buildUrl(path) {
    return `${AppConfig.authServiceBaseUrl}${path}`;
  }

  #jsonHeaders() {
    return {
      "Content-Type": "application/json"
    };
  }
}
