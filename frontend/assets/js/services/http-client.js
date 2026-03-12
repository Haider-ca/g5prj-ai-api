export class HttpClient {
  async request(url, options = {}) {
    const response = await fetch(url, options);
    const payload = await this.#readPayload(response);

    if (!response.ok) {
      const message = this.#extractMessage(payload) || `Request failed with status ${response.status}.`;
      const error = new Error(message);
      error.status = response.status;
      error.payload = payload;
      throw error;
    }

    return payload;
  }

  async #readPayload(response) {
    const contentType = response.headers.get("content-type") || "";
    if (contentType.includes("application/json")) {
      return response.json();
    }

    return response.text();
  }

  #extractMessage(payload) {
    if (!payload) {
      return "";
    }

    if (typeof payload === "string") {
      return payload;
    }

    return payload.message || payload.error || "";
  }
}
