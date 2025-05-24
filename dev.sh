#!/bin/bash
set -e

case "$1" in
  run)
    echo "🟢 Starting frontend and app..."
    cd frontend
    pnpm install
    pnpm run dev &
    FRONTEND_PID=$!
    cd ..

    dotnet watch --project TryPhotinoMVVM/TryPhotinoMVVM.csproj run
    kill $FRONTEND_PID
    ;;
  publish)
    echo "🌱 Building frontend..."
    cd frontend
    pnpm install
    pnpm run build
    cd ..

    echo "🧹 Cleaning wwwroot..."
    rm -rf TryPhotinoMVVM/wwwroot
    mkdir -p TryPhotinoMVVM/wwwroot

    echo "📦 Copying frontend files to wwwroot..."
    cp -r frontend/dist/* TryPhotinoMVVM/wwwroot/

    echo "🛠 Publishing .NET backend..."
    dotnet publish TryPhotinoMVVM/TryPhotinoMVVM.csproj -c Release

    if [ "$(uname)" == 'Darwin' ]; then
      ./publish_mac_app.sh
    fi
    ;;
  *)
    echo "Usage: $0 {run|publish}"
    echo "  run     - run frontend (Vite) and app in dev mode"
    echo "  publish - build frontend and publish backend"
    exit 1
    ;;
esac