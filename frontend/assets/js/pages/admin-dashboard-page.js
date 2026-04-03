import { AuthApiService } from "../services/auth-api.js";
import { SessionController } from "../services/session-controller.js";
import { UiStrings } from "../constants/strings.js";
import { AppConfig } from "../config.js";
import { byId, setMessage, setText } from "../utils/dom.js";

const sessionController = new SessionController();
const authApi = new AuthApiService();
const session = sessionController.requireRole(AppConfig.roles.admin);

if (session) {
  const currentAdminLabel = byId("currentAdminLabel");
  const userCountValue = byId("userCountValue");
  const usersTableBody = byId("usersTableBody");
  const adminMessage = byId("adminMessage");
  const refreshUsersButton = byId("refreshUsersButton");
  const logoutButton = byId("logoutButton");

  setText(currentAdminLabel, session.user.email || UiStrings.signedInAdminLabel);

  logoutButton.addEventListener("click", async () => {
    try {
      await authApi.logout();
    } catch {
      // Always clear local state even if logout API is unavailable.
    }
    sessionController.clearSession();
    window.location.href = AppConfig.pages.login;
  });

  refreshUsersButton.addEventListener("click", async () => {
    await loadUsers();
  });

  async function loadUsers() {
    setMessage(adminMessage, "");

    try {
      const payload = await authApi.getAdminUsers();
      const users = authApi.normalizeAdminUsers(payload);
      renderUsers(users);
      setText(userCountValue, String(users.length));
      setMessage(adminMessage, UiStrings.usersLoaded, "success");
    } catch (error) {
      setMessage(adminMessage, error.message || UiStrings.genericError, "error");
    }
  }

  function renderUsers(users) {
    usersTableBody.textContent = "";

    if (!users.length) {
      const emptyRow = document.createElement("tr");
      const emptyCell = document.createElement("td");
      emptyCell.colSpan = 4;
      emptyCell.textContent = UiStrings.adminNoUsers;
      emptyRow.appendChild(emptyCell);
      usersTableBody.appendChild(emptyRow);
      return;
    }

    for (const user of users) {
      const row = document.createElement("tr");
      row.appendChild(createCell(user.email));
      row.appendChild(createCell(user.role));
      row.appendChild(createCell(String(user.remainingCalls)));
      row.appendChild(createCell(String(user.usageCount)));
      usersTableBody.appendChild(row);
    }
  }

  function createCell(value) {
    const cell = document.createElement("td");
    cell.textContent = value;
    return cell;
  }

  loadUsers();
}
