# {{ change-name }}

## Problem Statement

-- A general description of the problem that this change is trying to solve and why we are trying to solve it. 

eg. 
A user that has authenticated with OAuth in the last 24hours, using a client with an active client session, should be able to startup a new user session using a pincode without having to go through the full OAuth flow again.

## Proposed Solution

-- A high-level description of the solution that you are proposing. This should include the main components of the solution and how they will work together to solve the problem.

eg. 

Allow OAuth authenticated users to configure a pincode for their credential.
Store pincodes on user credentials
Allow starting user sessions using a pincode if the user has a stored user credential and is using a client with an active client session.

## Business Requirements

-- A list of business requirements/rules that will be added during implementation. 

eg.:

**Given** A user with an active OAuth Session
**When** The user configures a pincode for their credential
**Then** The credential will be updated to include the pincode

**Given** A user with a configured pincode for their credential, using a client with an active client session
**When** The user attempts to start a new session using the pincode
**Then** A new session will be started

## Acceptance Criteria

-- a list of acceptance criteria that must be met in order for this change to be considered complete. This should include both functional and non-functional requirements.

eg.

- [ ] The user can configure a pincode for their credential 
- [ ] The user can start a new session using a pincode 


## Testing Plan

-- a plan for how you will test the change to ensure that it meets the acceptance criteria. This should include both manual and automated testing strategies.

eg.

- Manual testing: 
    - Test that a user with an active OAuth session can configure a pincode for their credential
    - Test that a user with a configured pincode can start a new session using the pincode
    - Test that a user without an active OAuth session cannot configure a pincode for their credential
    - Test that a user without a configured pincode cannot start a new session using a pincode
    - Test that a client without an active client session cannot be used to start a new session using a pincode

- Automated testing:
    - Unit tests for the pincode configuration and session starting logic
    - Integration tests for the overall flow of configuring a pincode and starting a session using a pincode

## Refactors (OPTIONAL)

- a list of identified refactors that should be done in order to implement the change.

## Potential Pitfalls (OPTIONAL)

- a list of risks, edge cases, or constraints that could impact implementation quality or delivery.

eg.

- User credential model should be restructured into a type hierarchy
    - Credential (base record)
    - locked-credential (extends credential, tracks failed pincode attempts )
    - unlocked-credential (extends credential, allows pincode configuration and session starting using pincode)


