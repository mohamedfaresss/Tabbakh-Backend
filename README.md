# Tabbakh – Recipe & Meal Planning Platform

Graduation Project (2024 – 2025)

Backend service built with ASP.NET Core Web API, powering a cross-platform recipe and meal planning system consumed by a React web client and a Flutter mobile client.

## Overview

Tabbakh is a recipe and meal planning platform. Core functionality includes:

- Multilingual recipes (English and Arabic)
- Recipe search, ingredient-based search, and filtering
- Favorites and shopping cart management
- User profiles with image upload
- JWT-based authentication and authorization
- A single backend consumed by both the web and mobile clients

## Tech Stack

- **Backend:** ASP.NET Core 8
- **Database:** SQL Server, Entity Framework Core
- **Mapping:** AutoMapper
- **Auth:** JWT (JSON Web Token)
- **Containerization:** Docker
- **Deployment:** Azure
- **API Documentation:** Swagger (OAS 3.0)

## Project Structure

```
Tabbakh-Backend/
├── IdentityManagerAPI/       # Main API project (controllers, Program.cs, config)
├── DataAcess/                 # EF Core DbContext, migrations, repositories
├── Models/                    # Entities and DTOs
├── IdentityManager.Services/  # Business logic and controller services
├── IdentityManagerAPI.sln     # Solution file
└── README.md
```

## Running Locally

1. Clone the repository:
   ```bash
   git clone https://github.com/mohamedfaresss/Tabbakh-Backend.git
   ```

2. Copy the example settings file and fill in your own values:
   ```bash
   cd IdentityManagerAPI
   cp appsettings.Example.json appsettings.json
   ```
   Edit `appsettings.json` with your own SQL Server connection strings and JWT secret. This file is git-ignored and will not be committed.

3. Restore, migrate, and run:
   ```bash
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

## API Endpoints (v1)

**Authentication**
- `POST /api/AuthUser/Login` — Authenticate user
- `POST /api/AuthUser/RegisterUser` — Register new user
- `POST /api/AuthUser/RequestPasswordReset` — Request password reset
- `POST /api/AuthUser/ResetUserPassword` — Reset user password

**Cart**
- `POST /api/Cart/{recipeId}` — Add recipe to cart
- `DELETE /api/Cart/{recipeId}` — Remove recipe from cart
- `GET /api/Cart` — Get all items in cart

**Favorites**
- `POST /api/Favorites/{recipeId}` — Add recipe to favorites
- `DELETE /api/Favorites/{recipeId}` — Remove recipe from favorites
- `GET /api/Favorites` — Get all favorite recipes
- `DELETE /api/Favorites` — Clear all favorites

**Food**
- `GET /api/Food/recipes` — Get all recipes
- `GET /api/Food/recipes/preview` — Get recipe previews
- `GET /api/Food/recipes/search/by-name/{name}` — Search recipes by name
- `POST /api/Food/recipes/search/by-ingredient-ids` — Search recipes by ingredients
- `GET /api/Food/ingredients` — Get all ingredients
- `GET /api/Food/ingredients/search/by-name/{ingredientNames}` — Search ingredients by name

**User**
- `POST /api/User/uploadUserImage` — Upload user image

**User Profile**
- `GET /api/UserProfile` — Get profile
- `PUT /api/UserProfile` — Update profile

## Screenshots

<p align="center">
  <img src="https://i.ibb.co/Q7NLfKVK/Whats-App-Image-2025-09-19-at-18-25-01-0d3c3b77.jpg" alt="Login" width="250"/>
  <img src="https://i.ibb.co/Fk5SXSfj/Whats-App-Image-2025-09-19-at-18-25-59-05d19a3e.jpg" alt="Recipes" width="250"/>
  <img src="https://i.ibb.co/fctZMtr/Whats-App-Image-2025-09-19-at-18-26-22-a74056cc.jpg" alt="Cart" width="250"/>
</p>

<p align="center">
  <img src="https://i.ibb.co/60Sntk4t/Whats-App-Image-2025-09-19-at-18-27-27-b97bcff7.jpg" alt="Details" width="250"/>
  <img src="https://i.ibb.co/99H79nMc/Whats-App-Image-2025-09-19-at-18-28-20-446cd751.jpg" alt="Settings" width="250"/>
</p>

## Future Enhancements

- Meal planning calendar with weekly view
- AI-based recipe recommendations
- Grocery list and payment gateway integration

## Author

**Mohamed Gamal**
Backend Developer | .NET
GitHub: [Tabbakh-Backend](https://github.com/mohamedfaresss/Tabbakh-Backend)
