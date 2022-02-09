
# Auth API

This is an Authentication API that has the following features:

- Register - Validates and saves the user details. The newly created user gets saved into the database. The password it's stored using a hash algorithm.

- Login - The user credentials are validated and the user will receive a token that can be used for 5 minutes. The token is generated using custom encryption.

- Verify token - A route that for a provided token verify if it's still valid.

- Logout - A route that will mark a token as expired. For speed, the revoked token it's saved in Redis Cache. Searching for the revoked token inside Redis has a time complexity of O(1). (https://redis.io/topics/data-types#sets)

### Development environment
For the development environment Docker it's used to create and run all the required resources:
 - Auth API - .Net 5 API
 - Auth Database - MSSQL SERVER
 - Auth Cache - Redis Cache

For running the application locally it's enough to have Docker Desktop installed. You can start the application by:

 - Start from Visual Studio - Hit the start button and the application should start
 - Start using CMD or Powershell - run the following command at the root of the application `docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d` 

You should be able to access the app using the following url https://localhost/swagger/index.html

### Azure environment
The application and all the required resources are deployed in Azure. For hosting the application the following resources were created:

 - Azure Web App using Docker Container on Linux - used to deploy the Auth API
 - Azure SQL Server and DB - used to store the Auth API data
 - Azure Redis Cache - used to store the revoked token for fast querying
 - Azure Container Registry - used to store the Auth API images

The application can be accessed using the following url https://auth-api-web.azurewebsites.net/swagger/index.html (for resources economy - the web app it's not always on, so the first time hitting the URL it may take a few seconds for the application to start)
 
 ### Github actions
 A Github action was defined for CI/CD. The action it's triggered by a push into the **Main** branch and orchestrate the following:
 
 - Builds the Auth API using the existing Dockerfile
 - Push the Docker Image to the Azure Container Registry
 - Publish the new image to the Azure Web App