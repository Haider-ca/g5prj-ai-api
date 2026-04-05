import { AppConfig } from "../config.js";
import { HttpClient } from "./http-client.js";
import { TokenService } from "./token-service.js";

export class AiApiService {
  constructor(httpClient = new HttpClient(), tokenService = new TokenService()) {
    this.httpClient = httpClient;
    this.tokenService = tokenService;
  }

  async evaluateAnswer(requestBody) {
    const token = this.tokenService.getToken();
    const headers = {
      "Content-Type": "application/json"
    };

    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }

    return this.httpClient.request(`${AppConfig.aiServiceBaseUrl}${AppConfig.endpoints.evaluate}`, {
      method: "POST",
      headers,
      body: JSON.stringify(requestBody)
    });
  }

  normalizeEvaluation(payload) {
    return {
      score: payload.score ?? "--",
      feedback: payload.feedback ?? "No feedback returned.",
      model: payload.model ?? "Unknown",
      remainingCalls: payload.remainingCalls ?? payload.callsRemaining ?? "--"
    };
  }
}
