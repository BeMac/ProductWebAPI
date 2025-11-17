
A lot of assumptions for standing this up were made from this tutorial: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio

We got a web api with endpoints to handle post and Get operations, but it doesn't persist outside of runtime. 
We baked in Postgres into a Docker image. Postgres based on its speed, the conversation with Sam, and the fact that it's open source
A couple references I used to stand this portion up:
    https://mohamadlawand.medium.com/net-7-how-to-containerise-web-api-with-docker-use-postgresql-e3791293c4f0
    https://www.dbvis.com/thetable/how-to-set-up-postgres-using-docker/