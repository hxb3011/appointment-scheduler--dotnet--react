@echo off

where docker >nul 2>nul || (
    echo Docker is not installed!
    exit /b 1
)

docker version >nul 2>nul || (
    echo Docker Daemon is not started!
    exit /b 1
)

docker ps -qf name=^dotnet$ | findstr . >nul 2>nul && (
    docker exec dotnet dotnet clean -c Debug
    docker exec dotnet dotnet clean -c Release
) || (
    echo "dotnet" container is not Started.
)

docker ps -qf name=^node$ | findstr . >nul 2>nul && (
    docker exec -w /tmp/workspace/frontend/public/react-appointment-scheduler node rm -r build
) || (
    echo "node" container is not Started.
)
exit /b 0