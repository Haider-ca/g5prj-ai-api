import { AppConfig } from "../config.js";
import { HttpClient } from "./http-client.js";

export class AiApiService {
  constructor(httpClient = new HttpClient()) {
    this.httpClient = httpClient;
  }

  async evaluateAnswer(token, requestBody) {
    return this.httpClient.request(`${AppConfig.aiServiceBaseUrl}${AppConfig.endpoints.evaluate}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
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
