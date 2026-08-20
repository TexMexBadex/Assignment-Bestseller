# bt-net-assignment-master

# Solution
Two ASP.NET Core services:

- **ProductsApi** - REST API backed by an EF Core InMemory database, seeded on startup with data from fakestoreapi.com (https://fakestoreapi.com/products).
  - GET /products - list all products
  - GET /products/{id} - get a single product
  - POST /products - create a product, returns 201 with the path to the new resource
  - PUT /products/{id} - update
  - DELETE /products/{id} - delete

- **Gateway** - forwards requests over HTTP to ProductsApi and returns the response.

## Middleware

The Gateway includes middleware that logs how long each request takes in ms, logged as: "[Method] [Path] responded [StatusCode] in [Elapsedmillieseconds]"
