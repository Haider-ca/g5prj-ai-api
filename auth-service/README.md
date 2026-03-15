# Auth Service

ASP.NET Core Web API for Milestone 1 authentication and API usage tracking.

## Endpoints

- `POST /register`
- `POST /login`
- `GET /users/me`
- `GET /users/usage`
- `POST /users/decrement-usage`
- `GET /admin/users`

The service also exposes `/api/users/usage`, `/api/users/decrement-usage`, and `/api/admin/users` for AI-service compatibility.

## Local Run

1. Install the .NET 8 SDK and runtime.
2. Go to `auth-service/AuthService`.
3. Run `dotnet restore`
4. Run `dotnet run`

The service listens on `http://localhost:5080`.

## Notes

- JWT key is stored in `Resources/Security/jwt-key.txt`.
- Response messages are loaded from `Resources/Messages/*.txt`.
- Users are stored in `Data/users.json`.
