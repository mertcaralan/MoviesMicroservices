# MoviesMicroservices

.NET 8, SQLite, MediatR, JWT and Ocelot.

## Setup

Install the .NET 8 SDK and EF Core CLI if needed:

```sh
dotnet tool install --global dotnet-ef --version 8.0.29
```

From the solution directory:

```sh
dotnet restore MoviesMS.sln
cd Movies.API
dotnet ef database update --project ../Movies.APP --startup-project .
cd ..
```

Users.API creates its database and demo accounts on its first Development startup.
Existing user data is preserved on restart.

## Run

Run each command in a separate terminal from the solution directory:

```sh
dotnet run --project Users.API --launch-profile http
dotnet run --project Movies.API --launch-profile http
dotnet run --project Gateway.API --launch-profile http
```

| Service | Address |
| --- | --- |
| Gateway | http://localhost:5225 |
| Movies Swagger | http://localhost:5151/swagger |
| Users Swagger | http://localhost:5223/swagger |

## Gateway routes

| Path | Service |
| --- | --- |
| `/api/Movies`, `/api/Genres`, `/api/Directors` | Movies.API |
| `/api/Users`, `/api/Roles`, `/api/Statuses` | Users.API |
| `/api/Token`, `/api/RefreshToken` | Users.API |
| `/api/SeedMoviesDb` | Movies.API `/api/SeedDb` |
| `/api/SeedUsersDb` | Users.API `/api/SeedDb` |

Use GET for lists, GET or DELETE with `/{id}`, and POST or PUT on the base paths.
Token and RefreshToken accept POST. Seed routes accept GET and require Admin.

## Authentication

POST `/api/Token` through the Gateway:

```json
{"userName":"admin","password":"admin"}
```

The other demo account is `user` / `user`. Set the `Authorization` header to the
returned `jwt` value, which already includes `Bearer `. In Swagger, paste the
same value into Authorize.

As Admin, GET `/api/SeedMoviesDb` to create the sample movies, directors and genres.
GET `/api/Movies` is anonymous. Other data reads require authentication, and
writes require Admin.

POST the returned `jwt` and `refreshToken` to `/api/RefreshToken` to renew them.
Use the new refresh token for the next request; the old value is invalidated.
The signing key, issuer and audience must match in both API configurations.
