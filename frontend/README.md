# Haven Frontend

Static frontend for the `g5prj` Milestone 1 client application.

## Current service configuration

- `AI service`: `http://localhost:5139`
- `Auth service`: `http://localhost:5080`

Update service URLs in `assets/js/config.js` when backend endpoints change.

## Run locally

The deployed AI service is configured to allow the frontend origin `http://localhost:3000`.
Run the frontend from that origin to avoid CORS issues.

### Option 1: Python

If Python 3 is available:

```bash
cd frontend
python3 -m http.server 3000
```

Open:

`http://localhost:3000`

### Option 2: VS Code Live Server

Serve the `frontend/` folder on port `3000`.

## Current testable flow

- Register and login use the local auth service.
- Login succeeds through an `HttpOnly` auth cookie set by the auth service.
- Frontend requests include credentials so the browser sends the auth cookie automatically.
- The current signed-in user is restored through `GET /users/me`.
- User dashboard can call `POST /api/ai/evaluate` against the local AI service.
- Admin dashboard loads users from the local auth service.

## Authentication Notes

- The frontend does not need to read JWT directly from cookies.
- The auth cookie is `HttpOnly`, so browser JavaScript should not access it.
- Session state in local storage is limited to user profile data used for routing and UI.

## Local Services

Start the auth service:

```bash
cd auth-service/AuthService
ASPNETCORE_URLS=http://localhost:5080 dotnet run
```

Start the AI service:

```bash
cd ai-service/AiService
ASPNETCORE_URLS=http://localhost:5139 dotnet run
```

## AI request example

```json
{
  "question": "What is polymorphism?",
  "studentAnswer": "Polymorphism allows one interface to take many forms."
}
```

## Expected AI response shape

```json
{
  "score": 5,
  "feedback": "The answer is too brief. Add more detail and a clear example.",
  "model": "mock-ai",
  "remainingCalls": 20
}
```
