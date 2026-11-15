# .NET Assignment - Products API with Gateway

## Solution Overview

This solution contains two ASP.NET Core services that work together: a Products API and a Gateway. The Gateway acts as a single entry point for clients, forwarding requests to the Products API and returning responses.

## Architecture

### Products API
The core REST API that manages products using an in-memory database.

**Endpoints:**
- `GET /products` - Returns all products
- `GET /products/{id}` - Returns a single product 
- `POST /products` - Creates a new product
- `PUT /products/{id}` - Updates a product 
- `DELETE /products/{id}` - Deletes a product

**Security:** 
The API requires an `X-Gateway-Key` header on all requests. Direct calls without this key are rejected with 401 Unauthorized. This ensures the API can only be accessed through the Gateway.
For the purpose of this application, the key is in appsettings.json, but for real life scenarios it should be stored in a secure location e.g. a Key Vault.

**Database:** 
Uses EF Core's in-memory database. On startup, the `DbSeeder` automatically populates the database with 10 products from fakestoreapi.com.

### Gateway
Acts as a proxy between clients and the Products API.

**Endpoints:**
- `GET /products` - Forwards to Products API and returns the response
- `GET /products/{id}` - Forwards to Products API
- `POST /products` - Forwards the request body to Products API
- `PUT /products/{id}` - Forwards with request body
- `DELETE /products/{id}` - Forwards the delete request

**How it works:** 
When a client makes a request to the Gateway, it:
1. Receives the request
2. Automatically adds the required `X-Gateway-Key` header
3. Forwards it to the Products API
4. Streams the response back to the client

**Logging Middleware:** 
Every request through the Gateway is logged with timing information. The log includes the HTTP method, path, response status code, and how long the entire request took in milliseconds. 
Example: `[GET] [/products] responded [200] in [45ms]`

### Testing
- **Unit Tests** - Tests the ProductService with mocked dependencies (3 tests)
- **Integration Tests** - Tests the full flow from Service through Repository to Database (5 tests)
- Each test runs with an isolated in-memory database to prevent test contamination

### Validation
- Data annotations on CreateProductDto enforce rules at the API boundary:
  - Title: Required, 1-200 characters
  - Price: Required, must be greater than 0.01
  - Description: Required, max 1000 characters
  - Category: Required, max 100 characters
  - Image: Must be a valid URL, max 500 characters