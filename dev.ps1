param (
    [string]$mode = ""
)

switch ($mode) {
    "run" {
        Write-Host "🟢 Starting frontend and app..."

        Push-Location ./frontend
        npm install --prefix frontend
        $frontend = Start-Process powershell -ArgumentList "-Command", { npm run dev } -PassThru
        Pop-Location

        dotnet watch --project TryPhotinoMVVM/TryPhotinoMVVM.csproj run

        if ($frontend -ne $null) {
            Stop-Process -Id $frontend.Id -Force
        }
    }

    "publish" {
        Write-Host "🌱 Building frontend..."

        Push-Location ./frontend
        npm install --prefix frontend
        npm run build --prefix frontend
        Pop-Location

        Write-Host "🧹 Cleaning wwwroot..."
        Remove-Item -Recurse -Force TryPhotinoMVVM/wwwroot
        New-Item -ItemType Directory -Force -Path TryPhotinoMVVM/wwwroot | Out-Null

        Write-Host "📦 Copying frontend files to wwwroot..."
        Copy-Item -Recurse -Force frontend/dist/* TryPhotinoMVVM/wwwroot/

        Write-Host "🛠 Publishing .NET backend..."
        dotnet publish TryPhotinoMVVM/TryPhotinoMVVM.csproj -c Release
    }

    Default {
        Write-Host "Usage: ./dev.ps1 [run|publish]"
        Write-Host "  run     - run frontend (Vite) and app in dev mode"
        Write-Host "  publish - build frontend and publish backend"
        exit 1
    }
}