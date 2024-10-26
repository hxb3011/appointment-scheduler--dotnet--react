@echo off

where docker >nul 2>nul || (
    echo Docker is not installed!
    exit /b 1
)

docker version >nul 2>nul || (
    echo Docker Daemon is not started!
    exit /b 1
)

docker ps -aqf name=^dotnet$ | findstr . >nul 2>nul && docker ps -aqf name=^mysql$ | findstr . >nul 2>nul && docker ps -aqf name=^node$ | findstr . >nul 2>nul && (
    docker ps -qf name=^dotnet$ | findstr . >nul 2>nul && (
        echo "dotnet" already Started.
    ) || (
        docker start dotnet >/dev/null && echo "dotnet" Started.
    )
    docker ps -qf name=^mysql$ | findstr . >nul 2>nul && (
        echo "mysql" already Started.
    ) || (
        docker start mysql >/dev/null && echo "mysql" Started.
    )
    docker ps -qf name=^node$ | findstr . >nul 2>nul && (
        echo "node" already Started.
    ) || (
        docker start node >/dev/null && echo "node" Started.
    )
) || (
    echo Development environment not fully initialized.
)