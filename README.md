# SomaShare - Textbook Marketplace Platform

A comprehensive ASP.NET Core web application for buying, selling, and managing textbooks with African-focused features including community trust scoring and multilingual support.

## Features

### Core Functionality
- **Textbook Management**: List, edit, and delete textbooks
- **Offer Management**: Place and respond to textbook offers
- **Transaction Tracking**: Complete transaction history
- **Review System**: Rate and review transactions
- **Wanted Ads**: Create and manage wanted ads for specific books

### Enhanced Features
- **Advanced Search & Filtering**: Full-text search, category filtering, price ranges
- **Pagination**: Efficient data loading for large datasets
- **Dashboard**: User analytics and activity overview
- **Community Trust Scoring**: User reputation system
- **Multilingual Support**: Support for English, Xhosa, Zulu, Afrikaans, and more

### Security Enhancements
- **Strong Password Policies**: Enforced password requirements
- **Account Lockout**: Protection against brute force attacks
- **Email Verification**: Confirmed email addresses
- **Two-Factor Authentication**: Optional 2FA support
- **Password History**: Track password changes

## Prerequisites

- .NET 10 or higher
- SQL Server 2019+
- Visual Studio 2022+ or VS Code

## Installation

1. Clone the repository
2. Update database connection in `appsettings.json`
3. Run migrations: `dotnet ef database update`
4. Start the application: `dotnet run`

## Project Structure

```
SomaShare/
├── Models/           # Data models and view models
├── Controllers/      # MVC controllers
├─�� Data/            # Database context and initializer
├── Views/           # Razor views
├── Services/        # Business logic services
├── Utilities/       # Helper classes and extensions
└── wwwroot/         # Static files
```

## License

MIT License
