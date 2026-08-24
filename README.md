# Exercise 3: "Movie API" (EF Core & ASP.NET Core Web API)

## Project Purpose
The purpose of this project is to build a robust fullstack-oriented Movie API using ASP.NET Core Web API and Entity Framework Core. Managing a movie database stored via SQLite/LocalDB, the application implements structured domain models, 1:1, 1:M, and N:M relationships, DTO separation, input validation, robust query filtering, global relevance search, and statistical reporting endpoints.

## Core Technologies
* **.NET & ASP.NET Core Web API:** RESTful architecture serving endpoints for full HTTP CRUD operations (`GET`, `POST`, `PUT`, `DELETE`), status code handling, and attribute-based request validation (`[Required]`, `[Range]`).
* **Entity Framework Core & LINQ:** Code-first database modeling, migrations, fluent API relational configurations, composite keys, and advanced LINQ queries (`Select`, `SelectMany`) for relational mapping and statistical aggregations.
* **DTO Architecture:** Strict separation of concerns using specialized Data Transfer Objects for create, update, summary, and detailed views (`MovieCreateDto`, `MovieUpdateDto`, `MovieDto`, `MovieDetailDto`).
* **Database Seeding:** Automated JSON-backed initialization seeding comprehensive datasets for movies, details, actors, reviews, and relational mappings.

## Project Structure
The repository contains the ASP.NET Core Web API backend structured with clean separation of concerns:

```text
/root
 ├── Controllers/           # API controllers (Actors, Movies, Reports, Reviews)
 ├── Data/                  # DbContext, database files, and initial JSON seed data
 ├── Docs/                  # Assignment instructions and reference material
 ├── DTOs/                  # Data Transfer Objects for requests, responses, and details
 ├── Extensions/            # Extension methods including database seeding logic
 ├── Migrations/            # EF Core database migration history
 ├── Models/                # Domain models and relational entities
 ├── appsettings.json       # Application configuration and connection strings
 ├── MovieApi.http          # HTTP client test requests file
 ├── Program.cs             # Application entry point and service registration
 └── README.md              # Project documentation file
```

## Core Assignment Features

### Read & Filtering (GET)
* Fetches collections and individual entities asynchronously with optimized relational loading.
* Implements flexible query parameter filtering (e.g., by genre, year, actor) and global relevance-based search.

### Detailed Views & LINQ Mapping
* Implements dedicated detail endpoints (e.g., `GET /api/movies/{id}/details`) returning aggregated `MovieDetailDto` objects combining movie metadata, details, reviews, and cast lists via LINQ.

### Create & Update (POST / PUT)
* Validates incoming payloads using data annotations and distinct DTOs for creation and updates.
* Manages complex relationships including adding actors to movies with custom roles.

### Statistical Reporting (ReportsController)
* Provides dedicated analytical endpoints using advanced LINQ aggregations (e.g., top-rated movies per genre, average ratings, most active actors, longest movies per country).

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

**Tags:** `csharp`, `dotnet`, `entity-framework-core`, `webapi`, `rest-api`, `linq`, `fullstack`, `sql-server`