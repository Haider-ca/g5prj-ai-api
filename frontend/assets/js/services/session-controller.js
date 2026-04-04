import { AppConfig } from "../config.js";
import { TokenService } from "./token-service.js";

export class SessionController {
  constructor(tokenService = new TokenService()) {
    this.tokenService = tokenService;
  }

  getSession() {
    return {
      token: this.tokenService.getToken(),
      user: this.tokenService.getUser()
    };
  }

  saveSession({ token = "", user }) {
    if (token) {
      this.tokenService.setToken(token);
    } else {
      this.tokenService.clearToken();
    }
    this.tokenService.setUser(user);
  }

  clearSession() {
    this.tokenService.clearSession();
  }

  hasSession() {
    const session = this.getSession();
    return Boolean(session.user);
  }

  redirectToRoleHome(role) {
    window.location.href = role === AppConfig.roles.admin
      ? AppConfig.pages.adminDashboard
      : AppConfig.pages.userDashboard;
  }

  requireRole(requiredRole) {
    const session = this.getSession();
    if (!session.user) {
      window.location.href = AppConfig.pages.login;
      return null;
    }

    if (requiredRole && session.user.role !== requiredRole) {
      this.redirectToRoleHome(session.user.role);
      return null;
    }

    return session;
  }
}
