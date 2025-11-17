Quick start. Running .NET core 10
Entity Framework 10 
PostrgeSql

Architecture, overall, the architecture of this solution is a web api project. 
It would ideally use a repository pattern where the controller would be responsible for validation and error handling.
Service layer would be responsible for any business logic.
Repository layer would be responsible for database interaction.
We want to implement Interfaces for the service and repository layers.
    this helps with both dependency injection, unit testing capibilities (mocking), and general best practice. 

Database: this solution has a relience on a PostgreSQL relational database for persistance. 
In the short term, for testing, the main startup() implements inmemory storage as follows:
builder.Services.AddDbContext<ProductContext>(opt => opt.UseInMemoryDatabase("Product"));

However for persistance we have baked in a database 

We baked in Postgres into a Docker image. Postgres based on its speed, the conversation with Sam, and the fact that it's open source
A couple references I used to stand this portion up:
https://mohamadlawand.medium.com/net-7-how-to-containerise-web-api-with-docker-use-postgresql-e3791293c4f0
https://www.dbvis.com/thetable/how-to-set-up-postgres-using-docker/

There is a Dockerfile which does simple build operations for a .NET solution, 
And we use docker-compose.yaml file that adds in the PostgreSql server. 

Database setup:
I set up a foreign key relationship between Product and CategoryId
I set up a index for this foreign key contraint
Set up a second index for searching on Product Name

A lot of assumptions for standing this up were made from this tutorial: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio

We got a web api with endpoints to handle post and Get operations, but it doesn't persist outside of runtime. 




Added a first attempt at the implementation of Product Search, but ran short on time at this point