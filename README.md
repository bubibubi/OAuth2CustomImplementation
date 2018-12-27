# OAuth2CustomImplementation
C# WebAPI Project with custom OAuth2 implementation

The main purpose of the project is to implement (as a proof of concept) a custom OAuth2 to:
1. Authenticate user between different tenants. On different tennants there can be the same user but it must be considered as different user (i.e. Tenant1\User is different from Tenant2\User)
2. Keep track of the tenant on the OAuth2 server
3. (Connect to a different database basing on tenant)

## Abstract
The standard WebAPI C# applications generates an Entity Framework (EF) Context where the users and the related infos are stored. The authentication is based on this context.
This application simulate a connection to a specific authentication repository directly related to the tenant. It assumes that the user name is in the form <TenantId>\<UserName>

## Changes
There is a first commit with the WebAPI application as generated from standard template. Comparing it with last commit you can see the differences. The main differences is also listed below

### IdentityModels.cs
The authentication EF Context must be deleted. The Authentication is totally custom
ApplicationUser can't be handled by EF so it must implement IUser directly

### AccountController.cs
In this app, logins cannot be created from API so related logic must be deleted.
No external login permitted so related logic must be deleted.

### AccountViewModels.cs
UserInfoViewModel should contain only interesting informations (TenantId and UserName in some forms)
ManageInfoViewModel must not contain infos about external logins (delete ExternalLoginProviders)

### AccountBindingModels.cs
The updates are similar to AccountViewModels

### IdentityConfig.cs
The changes are on ApplicationUserManager. It does not manage the users via EF so we need to implement a new user store different from UserStore` (it requires a TUser that derives from a specific EF entity).
The issue in implementing the new user store (TenantUserStore) is that the store is related to the user name.
In this application the TenantUserStore is in models.
The other issue could be that the standard UserManager implements a PasswordHasher not compatible with the users and passwords already stored in database. In this case we need to implement a different password hasher or override the methods that handle passwords in ApplicationUserManager (the example does this).

## Test
The test simply makes a login then calls the Values API. The ValuesController Get method is modified to return 2 values, TenantId and UserName.

## What's missing
This is a proof of concept so it lacks of some implementations, for example the database handling.
The main miss is the groups that we could use in controllers authorization.