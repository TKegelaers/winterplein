# Plan: Deploy Winterplein to Azure

**TL;DR**: Deploy the standalone Blazor WASM client to **Azure Static Web Apps** (free tier) and the ASP.NET Core API to **Azure App Service** (Basic B1, ~$13/month). GitHub Actions handles CI/CD. No database needed since you're keeping in-memory storage.

---

## Phase 1 — Create Azure Resources (az CLI or Portal)

1. Create a Resource Group
2. Create an App Service Plan (Basic B1, Linux)
3. Create an App Service for the API — note the URL: `https://<app-name>.azurewebsites.net`
4. Create an Azure Static Web App linked to your GitHub repo — note the URL: `https://<swa-name>.azurestaticapps.net`

## Phase 2 — Code Changes

5. Modify `src/Winterplein.Api/Program.cs` — extract hardcoded CORS origins (`http://localhost:5149`, `http://localhost:5173`) into configuration (`AllowedOrigins` array from `appsettings.json`)
6. Add `src/Winterplein.Api/appsettings.Production.json` — sets `AllowedOrigins` to the Azure SWA URL from step 4
7. Add `src/Winterplein.Client/wwwroot/appsettings.Production.json` — sets `ApiBaseUrl` to the Azure App Service URL from step 3 (overrides the `localhost` URL at publish time)

## Phase 3 — GitHub Actions Workflows

8. Create `.github/workflows/deploy-api.yml` — triggers on push to `main`, runs `dotnet publish` on `Winterplein.Api`, deploys zip to App Service using publish profile secret
9. Create `.github/workflows/deploy-client.yml` — triggers on push to `main`, runs `dotnet publish` on `Winterplein.Client`, deploys `wwwroot` output to Static Web Apps with `skip_app_build: true`

## Phase 4 — GitHub Secrets

10. Add `AZURE_WEBAPP_PUBLISH_PROFILE` — download from App Service → "Get publish profile" in Azure Portal
11. Add `AZURE_STATIC_WEB_APPS_API_TOKEN` — from Static Web Apps → "Manage deployment token" in Azure Portal

## Phase 5 — Verify

12. Test API Swagger UI at `https://<app-name>.azurewebsites.net/swagger`
13. Test client at the SWA URL and confirm it loads players/matches from the API

---

## Relevant Files

- `src/Winterplein.Api/Program.cs` — CORS refactor (steps 5–6)
- `src/Winterplein.Api/appsettings.Production.json` — new file (step 6)
- `src/Winterplein.Client/wwwroot/appsettings.Production.json` — new file (step 7)
- `.github/workflows/deploy-api.yml` — new file (step 8)
- `.github/workflows/deploy-client.yml` — new file (step 9)

---

## Decisions

- In-memory storage means data resets on every API restart/redeploy — acceptable for now
- Publish profile auth is simpler than OIDC and fine for a single-developer project
- Swagger is currently `Development`-only — enable in Production or leave it off
- Azure Static Web Apps free tier allows 1 custom domain and 100 GB bandwidth/month — sufficient for this project

## Open Questions

1. **Swagger in production**: Currently guarded by `if (app.Environment.IsDevelopment())`. Enable on the Azure API URL?
2. **Custom domain**: Azure Static Web Apps free tier supports one custom domain. Use default `*.azurestaticapps.net` for now?
