# Auth!

This is an authentication server that allows that has the following features:
 - Register - The newly created user gets saved into the database. The password it's stored using a hash algorithm.
 - Login - The user credentials are validated and the user will receive a token that can be used for 5 minutes. The token is generated using a custom method.
 - Verify token - A route that for a provided token verify if it's still valid.
 - Logout - A route that will mark a token as expired. For speed, the revoked token it's saved in a list of complexity O(1) inside the Redis Cache.

The AuthAPI and all the required resources are deployed in Azure. The documentation of the API can be accessed on the following URL https://auth-api-web.azurewebsites.net/swagger/index.html.
Github actions were used for the CI/CD pipeline. The API it's built and deployed for every push into the **Main** branch.
