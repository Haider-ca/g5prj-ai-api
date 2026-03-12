# Haven Frontend

Static frontend for the `g5prj` Milestone 1 client application.

## Current service configuration

- `AI service`: `https://g5prj-ai-api-production.up.railway.app`
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

- Landing page loads session state from local storage.
- Landing page includes temporary development buttons for creating a local user or admin session.
- User dashboard can call `POST /api/ai/evaluate` against the deployed AI service.
- Admin and auth flows still depend on the auth service being implemented and reachable.

## Temporary development access

Until the auth service is available, use the landing page buttons:

- `Open user dashboard`
- `Open admin dashboard`

These buttons create a local session in browser storage so dashboard pages can be tested.
Remove this shortcut after the real auth flow is connected.

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
