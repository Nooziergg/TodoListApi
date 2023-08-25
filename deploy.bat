@echo off
SETLOCAL

:: Access Environment Variables
SET PROJECT_ID=%PROJECT_ID%
SET SERVICE_NAME=%SERVICE_NAME%
SET REGION=%REGION%
SET IMAGE_NAME=%IMAGE_NAME%
SET TIMESTAMP=%date:~-4,4%%date:~-7,2%%date:~-10,2%%time:~0,2%%time:~3,2%
SET TAG=%TIMESTAMP%

:: Paths relative to the current script's directory
SET APP_PATH=%~dp0\ToDoList
SET TEST_PATH=%~dp0\ToDoList.Tests

:: Navigate to your test project directory
cd %TEST_PATH%

:: Run the tests
echo Running Tests...
dotnet test
IF %ERRORLEVEL% NEQ 0 (
    echo Tests Failed! Aborting deployment...
    pause
    exit /b 1
)

:: Navigate to your application directory
cd %APP_PATH%

:: Build the Docker image
echo Building Docker Image...
echo %PROJECT_ID%
echo %IMAGE_NAME%
echo %TAG%
docker build -t gcr.io/%PROJECT_ID%/%IMAGE_NAME%:%TAG% .

IF %ERRORLEVEL% NEQ 0 (
    echo Docker build failed! Aborting deployment...
    pause
    exit /b 1
)

:: Push the Docker image to GCR
echo Pushing Image to Google Container Registry...
docker push gcr.io/%PROJECT_ID%/%IMAGE_NAME%:%TAG%
IF %ERRORLEVEL% NEQ 0 (
    echo Docker push failed! Aborting deployment...
    pause
    exit /b 1
)

:: Deploy to Google Cloud Run
echo Deploying to Google Cloud Run...
gcloud run deploy %SERVICE_NAME% --image gcr.io/%PROJECT_ID%/%IMAGE_NAME%:%TAG% --platform managed --region %REGION%
IF %ERRORLEVEL% NEQ 0 (
    echo Deployment failed!
    pause
    exit /b 1
)

echo Deployment Complete!
pause
ENDLOCAL

