@echo off
set SCRIPT_DIR=%~dp0

dotnet run --project "%SCRIPT_DIR%FastArena.ApiClientGenerator.csproj" "http://localhost:8100/swagger/FastArenaAPI/swagger.json" "%SCRIPT_DIR%client-build\api-clients.ts" ts