dotnet new globaljson;

dotnet new webapi -o wtt_main_server_api;
dotnet new xunit -o wtt_main_server_tests;

dotnet new sln --name wtt_main_server;
dotnet sln add wtt_main_server_api/wtt_main_server_api.csproj;
dotnet sln add wtt_main_server_tests/wtt_main_server_tests.csproj;

dotnet new gitignore;

New-Item dockerfile;
New-Item docker-compose.yml;
New-Item docker-compose.override.yml;
New-Item .dockerignore;
Set-Content .dockerignore '**/.classpath
**/.dockerignore
**/.env
**/.git
**/.gitignore
**/.project
**/.settings
**/.toolstarget
**/.vs
**/.vscode
**/*.*proj.user
**/*.dbmdl
**/*.jfm
**/azds.yaml
**/bin
**/charts
**/docker-compose*
**/Dockerfile*
**/node_modules
**/npm-debug.log
**/obj
**/secrets.dev.yaml
**/values.dev.yaml
**/*secret*.json
LICENSE
README.md'

git init;
pause "Press any key to exit"