@echo off
echo Building RoboAnalyzer Chat - One Click Solution...

set PROJECT_DIR=%~dp0..
set OUTPUT_DIR=%PROJECT_DIR%\dist

if exist "%OUTPUT_DIR%" rmdir /s /q "%OUTPUT_DIR%"
mkdir "%OUTPUT_DIR%"

echo.
echo 🔄 Downloading ngrok...
powershell -Command "Invoke-WebRequest -Uri 'https://bin.equinox.io/c/bNyj1mQVY4c/ngrok-v3-stable-windows-amd64.zip' -OutFile '%PROJECT_DIR%\Resources\ngrok.zip'"
powershell -Command "Expand-Archive -Path '%PROJECT_DIR%\Resources\ngrok.zip' -DestinationPath '%PROJECT_DIR%\Resources\' -Force"
del "%PROJECT_DIR%\Resources\ngrok.zip"

echo.
echo 🔄 Building single executable...
cd "%PROJECT_DIR%\ChatLauncher"

dotnet publish ^
    --configuration Release ^
    --runtime win-x64 ^
    --self-contained true ^
    --output "%OUTPUT_DIR%" ^
    /p:PublishSingleFile=true ^
    /p:IncludeNativeLibrariesForSelfExtract=true ^
    /p:PublishTrimmed=false ^
    /p:EnableCompressionInSingleFile=true

echo.
echo ✅ Build completed!
echo 📁 Output: %OUTPUT_DIR%\ChatLauncher.exe
echo.
echo 🚀 You can now share ChatLauncher.exe with your friends!
echo    Each person just needs to run it and click "Start Chat Server"
echo.
pause