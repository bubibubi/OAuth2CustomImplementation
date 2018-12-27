# OAuth2CustomImplementation
C# WebAPI Project with custom OAuth2 implementation

The main purpose of the project is to implement a custom OAuth2 to:
1. Authenticate user between different tennants. On different tennants there can be the same user but it must be considered as different user (i.e. Tenant1\User is different from Tenant2\User)
2. Keep track of the tenant on the OAuth2 server
3. (Connect to a different database basing on tenant)
