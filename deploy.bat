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
SET APP_PATH=.
SET TEST_PATH=.\ToDoList.Tests

:: Navigate to your test project directory
cd %TEST_PATH%

:: Run the tests
echo Running Tests...
dotnet test
IF %ERRORLEVEL% NEQ 0 (
    echo Tests Failed! Aborting deployment...
    exit /b 1
)

:: Navigate to your application directory
cd %APP_PATH%

:: Build the Docker image
echo Building Docker Image...
docker build -t gcr.io/%PROJECT_ID%/%IMAGE_NAME%:%TAG% .

:: Push the Docker image to GCR
echo Pushing Image to Google Container Registry...
docker push gcr.io/%PROJECT_ID%/%IMAGE_NAME%:%TAG%

:: Deploy to Google Cloud Run
echo Deploying to Google Cloud Run...
gcloud run deploy %SERVICE_NAME% --image gcr.io/%PROJECT_ID%/%IMAGE_NAME%:%TAG% --platform managed --region %REGION%

echo Deployment Complete!
ENDLOCAL
pause
