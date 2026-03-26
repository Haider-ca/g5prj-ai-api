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

  async getCurrentUser(token) {
    return this.httpClient.request(this.#buildUrl(AppConfig.endpoints.me), {
      method: "GET",
      headers: this.#authHeaders(token)
    });
  }

  async getUsage(token) {
    return this.httpClient.request(this.#buildUrl(AppConfig.endpoints.usage), {
      method: "GET",
      headers: this.#authHeaders(token)
    });
  }

  async getAdminUsers(token) {
    return this.httpClient.request(this.#buildUrl(AppConfig.endpoints.adminUsers), {
      method: "GET",
      headers: this.#authHeaders(token)
    });
  }

  #buildUrl(path) {
    return `${AppConfig.authServiceBaseUrl}${path}`;
  }

  #jsonHeaders() {
    return {
      "Content-Type": "application/json"
    };
  }

  #authHeaders(token) {
    return {
      ...this.#jsonHeaders(),
      Authorization: `Bearer ${token}`
    };
  }
}
