# {{task-name}} 

## Scope

-- A clear description of the task to be completed

eg.

Implement API endpoints for managing user pincodes

- PUT /credential/pincode 
- Request body: { pincode: string }
- Response: 200 Ok : CredentialApiDto
- Response: 401 Unauthorized : Unauthorized ( require OAuth session )

- POST /credential/start-session
- Request body: { pincode: string }
- Response: 201 Created : UserSessionApiDto
- Response: 401 Unauthorized : Unauthorized ( require Client session and valid pincode )

## Domain model changes (optional)

eg.

```mermaid
classDiagram
    CredentialApiDto {
        +string userId
        +string? pincode
    }

    UserSessionApiDto {
        +string id
        +string userId
        +string clientId
    }
```

## Test cases

- CredentialController_UpdatePincode_Should.cs
    - Update_CredentialPincode
    - Fail_ToUpdate_CredentialPincode_WithoutOAuthSession

- CredentialController_StartSession_Should.cs
    - Start_Session_WithPincode
    - Fail_ToStart_Session_WithInvalidPincode
    - Fail_ToStart_Session_WithoutClientSession

## Affected files

- list of files that will be created, modified or deleted as part of this task

- create: src/Api/Credentials/Contract/CredentialApiDto.cs
- modify: src/Api/Credentials/Contract/UserSessionApiDto.cs
- create: src/Api/Credentials/CredentialController.cs

- create: src/Api.Tests/Credentials/CredentialController_UpdatePincode_Should.cs
- create: src/Api.Tests/Credentials/CredentialController_StartSession_Should.cs

