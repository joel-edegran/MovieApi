# Exercise 3: "Movie API" (EF Core & ASP.NET Core Web API)

## Project Purpose
The purpose of this project is to build a robust fullstack-oriented Movie API using ASP.NET Core Web API and Entity Framework Core. Managing a movie database stored via SQL Server LocalDB, the application implements structured domain models, 1:1, 1:M, and N:M relationships, DTO separation, input validation, robust query filtering, global relevance search, and statistical reporting endpoints.

## Core Technologies
* **.NET & ASP.NET Core Web API:** RESTful architecture serving endpoints for full HTTP CRUD operations (`GET`, `POST`, `PUT`, `PATCH`, `DELETE`), status code handling, and attribute-based request validation (`[Required]`, `[Range]`).
* **Entity Framework Core & LINQ:** Code-first database modeling, migrations, fluent API relational configurations, composite keys, and advanced LINQ queries (`Select`, `SelectMany`, `GroupBy`) for relational mapping and statistical aggregations.
* **DTO Architecture:** Strict separation of concerns using specialized Data Transfer Objects for create, update, summary, and detailed views (`MovieCreateDto`, `MovieUpdateDto`, `MovieDto`, `MovieDetailDto`, `ActorDto`, `ActorCreateDto`, `ReviewDto`, `ReviewCreateDto`).
* **Database Seeding:** Automated JSON-backed initialization seeding comprehensive datasets for movies, details, actors, reviews, and relational mappings.

## Project Structure
The repository contains the ASP.NET Core Web API backend structured with clean separation of concerns:

```text
lexicon-be-api-ex03-ef-core/
├── docs/                      # Assignment instructions and reference material
│   ├── exercise/
│   │   └── Övning 3 Movie API.pdf
│   └── theory/
│       └── Föreläsning - Entity Framework (EF) CORE-Relationer-260615.pdf
├── MovieApi/
│   ├── Controllers/           # API controllers (Actors, Movies, Reports, Reviews)
│   │   ├── ActorsController.cs
│   │   ├── MoviesController.cs
│   │   ├── ReportsController.cs
│   │   └── ReviewsController.cs
│   ├── Data/                  # DbContext, database files, and initial JSON seed data
│   │   ├── actors.json
│   │   ├── MovieContext.cs
│   │   ├── movies.json
│   │   └── reviews.json
│   ├── DTOs/                  # Data Transfer Objects for requests, responses, and details
│   │   ├── ActorCreateDto.cs
│   │   ├── ActorDto.cs
│   │   ├── ActorJsonDto.cs
│   │   ├── MovieActorCreateDto.cs
│   │   ├── MovieActorJsonDto.cs
│   │   ├── MovieActorRoleDto.cs
│   │   ├── MovieCreateDto.cs
│   │   ├── MovieDetailDto.cs
│   │   ├── MovieDto.cs
│   │   ├── MoviePatchDto.cs
│   │   ├── MovieSeedDto.cs
│   │   ├── MovieUpdateDto.cs
│   │   ├── ReviewCreateDto.cs
│   │   ├── ReviewDto.cs
│   │   └── ReviewSeedDto.cs
│   ├── Extensions/            # Extension methods including database seeding logic
│   │   └── SeedDataExtensions.cs
│   ├── Migrations/            # EF Core database migration history
│   ├── Models/                # Domain models and relational entities
│   │   ├── Actor.cs
│   │   ├── Country.cs
│   │   ├── Director.cs
│   │   ├── Genre.cs
│   │   ├── Movie.cs
│   │   ├── MovieActor.cs
│   │   ├── MovieDetails.cs
│   │   └── Review.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── appsettings.Development.json
│   ├── appsettings.json       # Application configuration and connection strings
│   ├── MovieApi.csproj
│   ├── MovieApi.csproj.user
│   ├── MovieApi.http          # HTTP client test requests file
│   └── Program.cs             # Application entry point and service registration
├── .editorconfig
├── .gitignore
├── MovieApi.slnx
└── README.md                  # Project documentation file
```

## Core Assignment Features

### Read & Filtering (GET)
* Fetches collections and individual entities asynchronously with optimized relational loading.
* Implements flexible query parameter filtering (e.g., by `genre`, `year`, `actor`, `title`) and global relevance-based search.

### Detailed Views & LINQ Mapping
* Implements dedicated detail endpoints (e.g., `GET /api/movies/{id}/details`) returning aggregated `MovieDetailDto` objects combining movie metadata, details, reviews, and cast lists via LINQ mapping.

### Create & Update (POST / PUT / PATCH)
* Validates incoming payloads using data annotations and distinct DTOs for creation, updates, and patching.
* Manages complex relationships including adding actors to movies with custom roles.

### Statistical Reporting (ReportsController)
* Provides dedicated analytical endpoints using advanced LINQ aggregations (e.g., top-rated movies per genre, average ratings per genre, active actors, longest movies per country, movie with most reviews, and popular genres).

## API Endpoints

### Movies (`/api/movies`)
* `GET /api/movies` — Retrieves all movies with optional filtering and global search.
  * **Query Strings:** 
    * `genre` (string): Filter by movie genre (case-insensitive partial match).
    * `year` (int): Filter by exact release year.
    * `actor` (string): Filter by actor name.
    * `title` (string): Filter by movie title (partial match).
    * `search` (string): Global text search across title, genre, director, actors, roles, reviews, or release year.
* `GET /api/movies/{id}` — Retrieves a specific movie by ID.
* `GET /api/movies/{id}/details` — Retrieves a detailed view (`MovieDetailDto`) combining metadata, reviews, synopsis, language, budget, and cast via LINQ mapping.
* `POST /api/movies` — Creates a new movie with validated payload (`MovieCreateDto`).
* `PUT /api/movies/{id}` — Updates an existing movie completely (`MovieUpdateDto`).
* `PATCH /api/movies/{id}` — Partially updates an existing movie (`MoviePatchDto`).
* `DELETE /api/movies/{id}` — Removes a movie by ID.
* `POST /api/movies/{movieId}/actors` — Assigns an actor to a specific movie with a custom role (`MovieActorCreateDto`).

### Actors (`/api/actors`)
* `GET /api/actors` — Retrieves a list of all actors (`ActorDto`).
* `GET /api/actors/{id}` — Retrieves a specific actor by ID (`ActorDto`).
* `POST /api/actors` — Adds a new actor (`ActorCreateDto`).
* `PUT /api/actors/{id}` — Updates an existing actor (`ActorCreateDto`).

### Reviews (`/api/movies/{movieId}/reviews` & `/api/reviews`)
* `GET /api/movies/{movieId}/reviews` — Retrieves all reviews associated with a specific movie.
* `POST /api/movies/{movieId}/reviews` — Creates a new review for a specific movie (`ReviewCreateDto`).
* `DELETE /api/reviews/{id}` — Deletes a review by its ID.

### Reports (`/api/reports`)
* `GET /api/reports/movies/top5pergenre` — Retrieves the top 5 rated movies per genre based on average review ratings.
* `GET /api/reports/movies/average-ratings` — Retrieves the average review ratings grouped and ordered by genre.
* `GET /api/reports/actors/most-active` — Retrieves a list of actors ordered by their participation count in movies.
* `GET /api/reports/movies/longest-per-country` — Retrieves the longest movie grouped by language/country.
* `GET /api/reports/movies/with-most-reviews` — Retrieves the single movie that has accumulated the highest number of reviews.
* `GET /api/reports/genres/popular` — Retrieves popular genres sorted by their total movie count, including movie details for each genre.

## Getting Started

### 1. Update Database
Apply Entity Framework Core migrations to set up the database:
```bash
dotnet ef database update
```

### 2. Run the Application
Start the ASP.NET Core Web API backend server:
```bash
dotnet run
```
The API backend will start listening at the configured local port.

## Course Information
* **Provider:** Lexicon IT-proffs AB / Luleå Tekniska Universitet (LTU)
* **Class:** Lexicon LTU VT-2026
* **Track:** Backend
* **Course:** ASP.NET Core Web API

**Tags:** `csharp`, `dotnet`, `entity-framework-core`, `webapi`, `rest-api`, `linq`, `fullstack`