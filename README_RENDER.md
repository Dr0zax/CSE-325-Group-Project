Render deployment instructions

Summary
- This repository contains a single Dockerfile at `CSE-325-Group-Project.Server/Dockerfile` which performs a multi-stage build: it publishes the Blazor WebAssembly client and the ASP.NET Web API server, then copies the client static files into the server's `wwwroot`. The final container hosts the client and API from the same origin.

Deploying to Render (one service)
1. In the Render dashboard, create a new "Web Service".
2. Connect your Git repo and choose the branch to deploy.
3. Select "Docker" as the environment (Render will use the Dockerfile in `CSE-325-Group-Project.Server`).
4. No build command is required; the Dockerfile handles the build. Leave start command empty — the Dockerfile entrypoint uses the `$PORT` env Render provides.
5. (Optional) Add any required environment variables (database connection string, JWT keys, etc.) in the Render service settings.
6. Deploy. Render will build the image using the Dockerfile and run the container. The app listens on the `$PORT` Render assigns.

Local testing
- Build and run with docker-compose:

```bash
docker compose up --build
```
