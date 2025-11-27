[README.md](https://github.com/user-attachments/files/23811294/README.md)

# Hotel Reservation System with AI Chatbot

A full-stack hotel reservation platform with an AI-powered chatbot.
Originally developed during an internship at Span, later expanded and refined as my bachelor’s final project.

## Features

### AI Chatbot (Semantic Kernel + OpenAI)
- Understands user intent through natural language
- Supports booking rooms via chat
- Includes custom Semantic Kernel plugins
  - HotelPlugin
  - SobaPlugin
  - RezervacijaPlugin

### Hotel Reservation System
- Browse hotels and rooms
- View room details and images
- Create and manage reservations
- Automatic reservation creation date
- JWT authentication

### Frontend
- Angular 19 (standalone components)
- PrimeNG UI components
- TailwindCSS styling
- Responsive SPA layout
- Side-panel chat interface

### Backend
- ASP.NET Core 8
- Entity Framework Core 8
- SQL Server
- Serilog logging (Console + MSSQL Sink)
- FluentValidation
- Semantic Kernel 1.60
- Swagger/OpenAPI

## Tech Stack

**Frontend**
- Angular 19.2
- PrimeNG 19
- TailwindCSS 3
- RxJS 7.8
- jwt-decode

**Backend**
- .NET 8
- EF Core 8
- ASP.NET Identity
- Serilog
- Semantic Kernel
- SQL Server
- JWT authentication

## Architecture Overview

Frontend (Angular SPA)
    → REST API (HTTPS)
Backend (ASP.NET Core 8)
    → EF Core Repository Layer
    → SQL Server

AI Subsystem:
Semantic Kernel → Custom Plugins → OpenAI ChatCompletion (gpt-4o)

## Running the Project

### Backend (.NET 8)

1. Navigate to the backend folder:
```
cd HotelAPI
```

2. Restore packages:
```
dotnet restore
```

3. Apply migrations:
```
dotnet ef database update
```

4. Configure User Secrets:
```
dotnet user-secrets set "OpenAi:ApiKey" "your-openai-key"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
dotnet user-secrets set "AppSettings:Token" "your-jwt-key"
```

5. Run the API:
```
dotnet run
```

### Frontend (Angular 19)

1. Navigate to the frontend folder:
```
cd HotelWeb
```

2. Install dependencies:
```
npm install
```

3. Start the dev server:
```
npm start
```

The app runs at:
```
http://127.0.0.1:4200/
```

## Security and Secrets

API keys and connection strings are not stored in the repository.
All sensitive data is stored using .NET User Secrets.

`appsettings.json` includes only placeholders:
```
"OpenAi": {
  "ApiKey": "<defined in secrets>"
}
```

## Logging

Serilog is configured with:
- Console logging
- MSSQL logging
- Automatic creation of the `Logs` table
- Debug global log level
- Warning override for Microsoft namespaces

## Validation

DTO validation is implemented using FluentValidation.
Validators include:
- PostanskiUredDtoValidator
- MjestoDtoValidator
- SobaDtoValidator
- HotelDtoValidator
- SlikaSobeDtoValidator
- RezervacijaDtoValidator

## Summary

This project demonstrates:
- Full-stack development (Angular + ASP.NET Core 8)
- AI integration using Semantic Kernel and OpenAI
- Plugin-based AI architecture
- Clean API design with validation
- Secure secret handling
- Production-ready logging
- A complete hotel reservation flow

Originally started during an internship at Span and completed as a bachelor’s final project.
