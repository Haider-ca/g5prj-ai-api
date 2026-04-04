# Auth Service

ASP.NET Core Web API for Milestone 1 authentication and API usage tracking.

## Endpoints

- `POST /register`
- `POST /login`
- `POST /logout`
- `GET /users/me`
- `GET /users/usage`
- `POST /users/decrement-usage`
- `GET /admin/users`

The service also exposes `/api/users/me`, `/api/users/usage`, `/api/users/decrement-usage`, and `/api/admin/users` for AI-service compatibility.

## Local Run

1. Install the .NET 8 SDK and runtime.
2. Go to `auth-service/AuthService`.
3. Run `dotnet restore`
4. Run `ASPNETCORE_URLS=http://localhost:5080 dotnet run`

The service should listen on `http://localhost:5080`.

## Authentication Flow

- `POST /login` returns the authenticated user and sets an `auth_token` `HttpOnly` cookie.
- Browser clients should send authenticated requests with credentials included so the cookie is sent automatically.
- Protected endpoints validate JWT from the `auth_token` cookie when no bearer token is provided.
- `POST /logout` clears the authentication cookie.

## Notes

- JWT key is stored in `Resources/Security/jwt-key.txt`.
- Response messages are loaded from `Resources/Messages/*.txt`.
- Users are stored in `Data/users.json`.
