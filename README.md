# Portfolio

Min personlige portfolio-side: en .NET 10 Web API med egen SQL Server-database og en JavaScript-frontend, der kører i to Docker-containere.

## Kom i gang

Kræver Docker.

```sh
cp .env.example .env        
docker compose up --build
```

| Hvad | Adresse |
|---|---|
| Hjemmeside | http://localhost:8080 |
| API-dokumentation (Scalar) | http://localhost:8080/scalar |
| Database (fx VS Code MSSQL) | `localhost,1434`, bruger `sa`, adgangskode fra `.env`, database `Portfolio` |


## Opbygning

```
compose.yml               to containere: db (SQL Server 2022) og api (.NET)
backend/
  Program.cs              opsætning: DI, DbUp, Scalar, statiske filer fra /frontend
  Controllers/            ProjectController: REST-endpoints på /api/project
  Data/                   DapperContext (forbindelse) + ProjectRepository (SQL via Dapper)
  Models/Project.cs       Id, Title, Description, CreatedUtc
  scripts/                SQL-scripts, som DbUp kører ved opstart
  frontend/               index.html, style.css, app.js (serveres af API'et)
```

### Flow

```
browser → GET /api/project → ProjectController → ProjectRepository → SQL Server
```

`app.js` henter projekterne med `fetch` og bygger et kort for hvert projekt på Work-siden.

### Backend

- **Controller:** tager imod HTTP-requests og svarer med statuskoder (200, 201, 204, 404).
- **Repository:** al SQL ligger her. Dapper mapper rækkerne til `Project`-objekter. Parametre (`@Id`, `@Title`) beskytter mod SQL injection.
- **DapperContext:** læser `ConnectionStrings:Default` og laver en `SqlConnection`. Connection stringen sættes i `compose.yml`.
- **DbUp:** opretter databasen, hvis den ikke findes, og kører hvert script i `scripts/` én gang i navnerækkefølge. Hvilke scripts der er kørt, gemmes i tabellen `SchemaVersions`.

### Endpoints

| Metode | Rute | Gør |
|---|---|---|
| GET | `/api/project` | henter alle projekter |
| GET | `/api/project/{id}` | henter ét projekt |
| POST | `/api/project` | opretter et projekt |
| PUT | `/api/project/{id}` | opdaterer titel og beskrivelse |
| DELETE | `/api/project/{id}` | sletter et projekt |

### Frontend

Ren HTML/CSS/JS uden framework. Hver sektion (`#work`, `#process`, `#about`, `#contact`) vises, når dens id står i adresselinjen. Tekst fra databasen indsættes med `textContent`, så den aldrig køres som HTML.

## Tilføj eller ret projekter

Lav et nyt script i `backend/scripts/` med næste nummer, fx `002-seed-projects.sql`:

```sql
INSERT INTO dbo.Projects (Title, Description, CreatedUtc)
VALUES (N'Titel', N'Beskrivelse', '2026-01-01');
```

Kør `docker compose up --build`. Ret aldrig i et script, der allerede er kørt. Lav et nyt script med `UPDATE` i stedet.

## Nulstil databasen

```sh
docker compose down -v      # sletter volumet, så alle data forsvinder
```
