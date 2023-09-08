# Californian Health Calendar Api

##  Description

This is the REST API for Californian Health to display the Calendars of each Consultant and their availabilities.

## Installation

Download the project and build it with an IDE.

## Requirements

- .NET 7.0
- NuGet packages :
    > Microsoft.EntityFrameworkCore
    > Microsoft.EntityFrameworkCore.InMemory
    > Microsoft.EntityFrameworkCore.SqlServer
    > Microsoft.EntityFrameworkCore.Tools
    > Microsoft.AspNetCore.Web.CodeGeneration.Design
    > Swashbuckle.AspNetCore
- Libraries :
    > CalifornianHealthLib
  
## Usage

You can use swagger to try the API trough this url : https://localhost:44366/swagger/index.html or https://localhost/5000/swagger/index.html


### Endpoints

ConsultantsController API Documentation
Overview

The ConsultantsController provides endpoints to interact with consultant data and availabilities.
Base Route

    Base Route: /api/Consultants

Endpoints
Get a List of All Consultants

    Description: Get a list of all consultants.
    HTTP Method: GET
    Route: /api/Consultants
    Returns: A list of consultants.
    Status Codes:
        200 OK: Successful request with a list of consultants.
        404 Not Found: No consultants found.

Get a Consultant by ID

    Description: Get a consultant by their ID.
    HTTP Method: GET
    Route: /api/Consultants/{id}
    Parameters:
        id (int): The ID of the consultant to retrieve.
    Returns: The consultant with the specified ID.
    Status Codes:
        200 OK: Successful request with the consultant.
        404 Not Found: No consultant found with the specified ID.

Get Consultant Availabilities by Consultant ID

    Description: Get a list of consultant availabilities by consultant ID.
    HTTP Method: GET
    Route: /api/Consultants/{consultantId}/ConsultantAvailabilities
    Parameters:
        consultantId (int): The ID of the consultant.
    Returns: A list of consultant availabilities.
    Status Codes:
        200 OK: Successful request with a list of availabilities.
        404 Not Found: No availabilities found for the specified consultant.

Request Examples
Example 1: Get a List of All Consultants

http

GET /api/Consultants

Example 2: Get a Consultant by ID

http

GET /api/Consultants/123

Example 3: Get Consultant Availabilities by Consultant ID

http

GET /api/Consultants/456/ConsultantAvailabilities

Response Examples
Example 1: Get a List of All Consultants (200 OK)

json

[
{
"id": 1,
"name": "John Doe",
"specialty": "Cardiologist"
// ... other consultant properties ...
},
// ... other consultants ...
]

Example 2: Get a Consultant by ID (200 OK)

json

{
"id": 123,
"name": "Jane Smith",
"specialty": "Neurologist"
// ... other consultant properties ...
}

Example 3: Get Consultant Availabilities by Consultant ID (200 OK)

json

[
{
"id": 101,
"consultantId": 456,
"availabilityDate": "2023-09-15"
// ... other availability properties ...
},
// ... other availabilities ...
]

Error Responses

    404 Not Found: No data found for the specified request.

This documentation provides an overview of the ConsultantsController API, including its endpoints, request examples, response examples, and error responses, to help you understand and use the API effectively.