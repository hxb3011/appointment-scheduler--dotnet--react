@echo off

where docker >nul 2>nul || (
    echo Docker is not installed!
    exit /b 1
)

docker version >nul 2>nul || (
    echo Docker Daemon is not started!
    exit /b 1
)

docker ps -aqf name=^dotnet$ | findstr . >nul 2>nul
set _dotnet=%errorlevel%
docker ps -aqf name=^mysql$ | findstr . >nul 2>nul
set _mysql=%errorlevel%
docker ps -aqf name=^node$ | findstr . >nul 2>nul
set _node=%errorlevel%

if %_dotnet% == 0 goto needForce
if %_mysql% == 0 goto needForce
if not %_node% == 0 goto initialize
:needForce
set a=_%*_
set a=%a: =_%
set a=%a:'force'=force%
set a=%a:"force"=force%
set a=%a:'=_%
set a=%a:"=_%

if %a% == _force_ (
    if %_dotnet% == 0 (
        docker ps -qf name=^dotnet$ | findstr . >nul 2>nul && docker stop dotnet >nul && echo "dotnet" Stopped.
        docker rm dotnet >nul && echo "dotnet" Removed.
    )
    if %_mysql% == 0 (
        docker ps -qf name=^mysql$ | findstr . >nul 2>nul && docker stop mysql >nul && echo "mysql" Stopped.
        docker rm mysql >nul && docker volume rm mysqld >nul && echo "mysql" Removed.
    )
    if %_node% == 0 (
        docker ps -qf name=^node$ | findstr . >nul 2>nul && docker stop node >nul && echo "node" Stopped.
        docker rm node >nul && echo "node" Removed.
    )
    docker network ls -qf name=^devEnvNET$ | findstr . >nul 2>nul && (
        docker network rm devEnvNET >nul && echo "devEnvNET" Removed.
    )
) else (
    if %a% == __ (
        echo Development environment already exists. You may have to use "%0 force" to force a re-initialization of the development environment.
    ) else (
        echo Please use "%0" to initialize the development environment. Or "%0 force" to force a re-initialization of the development environment.
    )
    echo Note: With the "force" option, the data of containers named "dotnet", "mysql" and "node" will be deleted except for VSCode extensions installed on them.
    exit /b 1
)
:initialize
docker network create --gateway 10.0.0.1 --subnet 10.0.0.0/8 devEnvNET && echo "devEnvNET" Created.
docker run -v "%cd%":/tmp/workspace -v vscrm:/root/.vscode-server -e "DB_SERVER=10.0.0.2" -e "DB_PORT=3306" -e "DB_USER=user0" -e "DB_PASSWORD=HeLlo|12" -e "DB_DATABASE=apomtschedsys" -e "API_SERVER=10.0.0.3" -e "API_PORT=8087" -w /tmp/workspace --network devEnvNET --ip 10.0.0.3 --name dotnet --pull always -td mcr.microsoft.com/dotnet/sdk:8.0 && echo "dotnet" Created.
docker run -v "%cd%":/tmp/workspace -v mysqld:/var/lib/mysql -e "MYSQL_ROOT_PASSWORD=HeLlo|12" -e "MYSQL_DATABASE=apomtschedsys" -e "MYSQL_USER=user0" -e "MYSQL_PASSWORD=HeLlo|12" -p 64001:3306 -w /tmp/workspace/database --network devEnvNET --ip 10.0.0.2 --name mysql --pull always -td mysql:8.3.0-oraclelinux8 && echo "mysql" Created.
docker run -v "%cd%":/tmp/workspace -v vscrm:/root/.vscode-server -w /tmp/workspace/frontend/public/react-appointment-scheduler --network devEnvNET --ip 10.0.0.4 --name node --pull always -td node:22.9.0-bullseye && echo "node" Created.
exit /b 0