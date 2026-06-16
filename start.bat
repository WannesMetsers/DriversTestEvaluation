@echo off

echo Starting backend...
start "Backend" cmd /k "cd /d "%~dp0Backend\DriversTestEvaluation.API" && dotnet restore && dotnet run"

echo Starting frontend...
start "Frontend" cmd /k "cd /d "%~dp0Frontend" && npm install && npm run dev"

echo Both services started.
pause