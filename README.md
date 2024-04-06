# api-service

steps to Deploying web api to Docker

Step 1
build the web api 
docker build -t personalapiproject

Run the application from docker container - This will run in production mode, so all configurations will be taken from appsettings.Production

docker run -p 8080:8080 personalapiproject

