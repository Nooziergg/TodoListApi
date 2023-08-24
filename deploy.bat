@echo off
SETLOCAL

:: Variables
SET PROJECT_ID=studious-plate-396617
SET SERVICE_NAME=todolist-service
SET REGION=southamerica-east1
SET IMAGE_NAME=todolist-image
SET TIMESTAMP=%date:~-4,4%%date:~-7,2%%date:~-10,2%%time:~0,2%%time:~3,2%
SET TAG=%TIMESTAMP%

:: Change the current directory to the script's location
CD %~dp0

:: Paths relative to root
SET APP_PATH=.\ToDoList
SET TEST_PATH=.\ToDoList.Tests

:: Print current directory for troubleshooting
echo Current directory: %CD%



...
:: Run the tests
echo Running Tests...
dotnet test
IF %ERRORLEVEL% NEQ 0 (
    echo Tests Failed! Aborting deployment...
    pause
    exit /b 1
)

:: Build the Docker image from the root directory pointing to the Dockerfile in the APP_PATH
echo Building Docker Image...
docker build -t gcr.io/%PROJECT_ID%/%IMAGE_NAME%:%TAG% -f %APP_PATH%\Dockerfile %APP_PATH%
IF %ERRORLEVEL% NEQ 0 (
    echo Docker build failed! Aborting deployment...
    pause
    exit /b 1
)

:: Push the Docker image to GCR
echo Pushing Image to Google Container Registry...
docker push gcr.io/%PROJECT_ID%/%IMAGE_NAME%:%TAG%
IF %ERRORLEVEL% NEQ 0 (
    echo Image push failed! Aborting deployment...
    pause
    exit /b 1
)

:: Deploy to Google Cloud Run
echo Deploying to Google Cloud Run...
gcloud run deploy %SERVICE_NAME% --image gcr.io/%PROJECT_ID%/%IMAGE_NAME%:%TAG% --platform managed --region %REGION%
IF %ERRORLEVEL% NEQ 0 (
    echo Deployment failed! Please check and try again.
    pause
    exit /b 1
)

echo Deployment Complete!
ENDLOCAL
pause
