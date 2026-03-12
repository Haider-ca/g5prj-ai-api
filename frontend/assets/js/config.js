export const AppConfig = Object.freeze({
  authServiceBaseUrl: "http://localhost:5080",
  aiServiceBaseUrl: "https://g5prj-ai-api-production.up.railway.app",
  endpoints: Object.freeze({
    register: "/register",
    login: "/login",
    me: "/users/me",
    usage: "/users/usage",
    adminUsers: "/admin/users",
    evaluate: "/api/ai/evaluate"
  }),
  storageKeys: Object.freeze({
    token: "haven.jwt",
    user: "haven.user"
  }),
  roles: Object.freeze({
    user: "user",
    admin: "admin"
  }),
  devSession: Object.freeze({
    enabled: true,
    testUserEmail: "student@example.com",
    testAdminEmail: "admin@example.com",
    placeholderToken: "frontend-dev-session"
  }),
  pages: Object.freeze({
    landing: "./index.html",
    login: "./login.html",
    register: "./register.html",
    userDashboard: "./user-dashboard.html",
    adminDashboard: "./admin-dashboard.html"
  })
});
