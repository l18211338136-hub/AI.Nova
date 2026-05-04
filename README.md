# AI.Nova

AI.Nova is an intelligent e-commerce platform leveraging AI semantic search and vector embeddings to enhance product discovery and management. Built with modern .NET technologies, it provides a comprehensive solution for building scalable, production-ready e-commerce applications with advanced AI capabilities.

## 🌟 Core Features

### AI-Powered Semantic Search
- **Vector Embeddings**: Advanced semantic search using text-embedding models for meaning-based product discovery
- **Cross-Language Support**: Find products across different languages with semantic understanding
- **Hybrid Search**: Combines full-text search with vector-based search for optimal results
- **Multiple AI Models**: Support for OpenAI, Azure OpenAI, Hugging Face, and local embedding models

### Modern Authentication & Authorization
- **JWT Token-Based Authentication**: Stateless authentication with access and refresh tokens
- **Multi-Factor Authentication (2FA)**: Enhanced security with two-factor authentication
- **External Identity Providers**: Built-in support for Google, Microsoft, GitHub, Facebook, Apple, Twitter, and Keycloak
- **WebAuthn & Passkeys**: Passwordless authentication using biometrics (fingerprint, Face ID)
- **Role & Permission-Based Authorization**: Fine-grained access control with claims and policies
- **Session Management**: Server-side session tracking with device-specific controls

### Comprehensive E-Commerce Features
- **Product Management**: Full CRUD operations with categories, inventory, and reviews
- **Shopping Cart & Orders**: Complete e-commerce workflow from cart to order fulfillment
- **Multi-Language Support**: Built-in localization with support for multiple languages
- **Push Notifications**: Real-time notifications using SignalR and AdsPush
- **File Storage**: Flexible storage supporting S3, Azure Blob, and local file systems

### Advanced Development Features
- **Blazor Multi-Platform**: Single codebase for Web, Windows, and MAUI (mobile & desktop)
- **Real-Time Communication**: SignalR for live updates and chatbot functionality
- **Background Jobs**: Hangfire integration for scheduled and background tasks
- **API Documentation**: Scalar/OpenAPI for comprehensive API documentation
- **OData Support**: Advanced querying capabilities for efficient data retrieval
- **Health Checks**: Built-in health monitoring endpoints

### Observability & Monitoring
- **Structured Logging**: Comprehensive logging with OpenTelemetry integration
- **Distributed Tracing**: Track requests across services with ActivitySource
- **Metrics Collection**: Performance metrics and custom counters
- **Aspire Dashboard**: Real-time monitoring during development
- **Sentry Integration**: Production error tracking
- **Azure Application Insights**: Cloud-native telemetry

## 🛠 Technology Stack

### Backend
- **.NET 10.0** with C# 14.0
- **ASP.NET Core 10.0**: Web API and server-side rendering
- **Entity Framework Core**: Data access with PostgreSQL/SQL Server support
- **ASP.NET Core Identity**: Authentication and authorization
- **Hangfire**: Background job processing
- **SignalR**: Real-time communication
- **OData**: Advanced querying
- **Mapperly**: High-performance object mapping

### Frontend
- **Blazor**: Component-based web UI framework
- **Bit.BlazorUI**: Primary UI component library
- **TypeScript**: Type-safe JavaScript development
- **SCSS**: Advanced CSS preprocessing with theme variables

### AI & ML
- **Microsoft.Extensions.AI**: AI integration framework
- **OpenAI / Azure OpenAI**: Production embedding models
- **Hugging Face**: Alternative AI model provider
- **LocalTextEmbeddingGenerationService**: Development-friendly local model

### DevOps & Infrastructure
- **.NET Aspire**: Cloud-ready orchestration and deployment
- **Docker**: Containerization support
- **GitHub Actions**: CI/CD pipelines
- **PostgreSQL 18** / **SQL Server 2025**: Primary databases with vector support
- **Redis**: Caching and distributed locking

## 📁 Project Structure

```
AI.Nova/
├── src/
│   ├── Server/
│   │   ├── AI.Nova.Server.Api/          # API controllers, DbContext, mappers, SignalR hubs
│   │   ├── AI.Nova.Server.Web/          # Main startup project (Blazor Server + SSR)
│   │   ├── AI.Nova.Server.Shared/       # Shared server code (ServiceDefaults)
│   │   └── AI.Nova.Server.AppHost/      # .NET Aspire orchestration
│   ├── Client/
│   │   ├── AI.Nova.Client.Core/         # Shared Blazor components, pages, layouts
│   │   ├── AI.Nova.Client.Web/          # Blazor WebAssembly standalone
│   │   ├── AI.Nova.Client.Maui/         # .NET MAUI Blazor Hybrid (mobile & desktop)
│   │   └── AI.Nova.Client.Windows/      # Windows Forms Blazor Hybrid
│   ├── Shared/
│   │   └── AI.Nova.Shared/              # Shared DTOs, enums, services, .resx resources
│   └── Tests/
│       └── AI.Nova.Tests/               # UI and integration tests
├── .docs/                                # 25 comprehensive documentation files
├── .github/
│   ├── workflows/                       # CI/CD pipelines
│   └── agents/                          # GitHub Copilot agent configurations
└── README.md
```

## 🚀 Quick Start

### Prerequisites

- **.NET 10.0 SDK**: [Download here](https://dotnet.microsoft.com/download/dotnet/10.0)
- **Docker Desktop** (optional, for container-based development)
- **Visual Studio 2022** or **Visual Studio Code**
- **PostgreSQL 18** or **SQL Server 2025** (for vector search support)

### Option 1: Run with .NET Aspire (Recommended)

1. Clone the repository:
```bash
git clone https://github.com/your-org/AI.Nova.git
cd AI.Nova
```

2. Open the solution in Visual Studio or VS Code

3. Set `AI.Nova.Server.AppHost` as the startup project

4. Run the project (F5 or `dotnet run`)

5. Access the application:
   - **Main App**: https://localhost:5001
   - **Aspire Dashboard**: https://localhost:2206
   - **Database (DbGate)**: Available via Aspire Dashboard

### Option 2: Run Individual Projects

1. Restore dependencies:
```bash
dotnet restore
```

2. Run the server project:
```bash
cd src/Server/AI.Nova.Server.Web
dotnet run
```

3. Access the application at https://localhost:5001

### Option 3: Run Blazor WebAssembly Standalone

```bash
cd src/Client/AI.Nova.Client.Web
dotnet run
```

Access at https://localhost:5002

## 📚 Feature Modules

### Product Management
- Full CRUD operations for products
- Category management with hierarchical organization
- Product images with multiple formats
- Inventory tracking
- Product reviews and ratings
- **Semantic Search**: AI-powered product discovery

### User Management
- User registration and authentication
- Profile management
- Session management (view and revoke active sessions)
- Multi-factor authentication
- Password reset functionality

### Admin Dashboard
- Real-time statistics and analytics
- Product and category management
- User and role management
- System monitoring and diagnostics
- Knowledge base management

### Knowledge Base
- Document upload and management
- AI-powered semantic search
- Text chunking for efficient retrieval
- Vector embeddings for content discovery

### Chatbot
- AI-powered conversational interface
- Text-to-SQL for natural language database queries
- Code generation and analysis
- Real-time assistance

## 🌐 Deployment Options

### Azure Cloud (Recommended)

Using .NET Aspire, deploy to:
- **Azure Container Apps**: Serverless container hosting
- **Azure SQL Database**: Managed database service
- **Azure Blob Storage**: Scalable file storage
- **Azure Cache for Redis**: Distributed caching

### Docker Compose

Generate `docker-compose.yml` for VPS and self-hosted environments:
```bash
dotnet aspire generate
```

### Kubernetes

Deploy to any Kubernetes provider (Azure AKS, AWS EKS, Google GKE):
```bash
dotnet aspire generate --output-format kubernetes
```

### Traditional Deployment

Deploy as standalone applications:
- **Server**: Publish as self-contained Linux/Windows executable
- **Client Web**: Publish Blazor WebAssembly to any web server
- **MAUI**: Build native mobile and desktop apps

## 📖 Documentation

This project includes **25 comprehensive documentation files** in the `.docs/` folder:

1. **Entity Framework Core** - Data access and migrations
2. **DTOs, Mappers, and Mapperly** - Object mapping patterns
3. **API Controllers and OData** - RESTful API design
4. **Background Jobs and Cancellation** - Hangfire integration
5. **Localization** - Multi-language support
6. **Exception Handling** - Error management strategies
7. **ASP.NET Core Identity** - Authentication & authorization
8. **Blazor Pages, Components, Styling** - UI architecture
9. **Dependency Injection** - Service registration
10. **Configuration** - appsettings.json management
11. **TypeScript & Build Process** - JavaScript interop
12. **Blazor Modes & PreRendering** - SSR and PWA
13. **Force Update System** - Version management
14. **Response Caching** - Performance optimization
15. **Logging & OpenTelemetry** - Observability
16. **CI/CD Pipeline** - Automated deployment
17. **Automated Testing** - Unit and integration tests
18. **Prompt Templates** - AI integration
19. **Project Files** - Miscellaneous configurations
20. **.NET Aspire** - Orchestration and deployment
21. **.NET MAUI** - Blazor Hybrid apps
22. **Messaging** - SignalR real-time communication
23. **Diagnostic Modal** - In-app troubleshooting
24. **WebAuthn & Passwordless Auth** - Advanced authentication
25. **RAG & Semantic Search** - Vector embeddings

> **💡 Tip**: We recommend using **Visual Studio Code** for reading these documentation files for the best Markdown preview experience.

### Additional Resources

- **Bit.BlazorUI Documentation**: https://blazorui.bitplatform.dev
- **.NET Aspire Documentation**: https://aspire.dev
- **Interactive Wiki**: https://wiki.bitplatform.dev
- **GitHub Issues**: https://github.com/bitfoundation/bitplatform/issues
- **GitHub Discussions**: https://github.com/bitfoundation/bitplatform/discussions

## 🔧 Configuration

### Database Configuration

Edit `appsettings.json` in `AI.Nova.Server.Api`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=AI.Nova;Username=postgres;Password=your_password"
  }
}
```

### AI Configuration

Configure embedding models in `appsettings.json`:

```json
{
  "AI": {
    "EmbeddingModel": "text-embedding-3-small",
    "EmbeddingApiKey": "your-api-key",
    "EmbeddingEndpoint": "https://models.inference.ai.azure.com"
  }
}
```

### Enable Vector Search

To enable semantic search with vector embeddings:

1. Open `src/Server/AI.Nova.Server.Api/Infrastructure/Data/AppDbContext.cs`
2. Change `IsEmbeddingEnabled` to `true`
3. Ensure you're using SQL Server 2025+ or PostgreSQL with `pgvector` extension

## 🧪 Testing

Run all tests:
```bash
dotnet test
```

Run specific test project:
```bash
cd src/Tests/AI.Nova.Tests
dotnet test
```

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Coding Standards

- Use **Bit.BlazorUI** components instead of generic HTML
- Follow the **three-file pattern** for Blazor components (`.razor`, `.razor.cs`, `.razor.scss`)
- Use **theme color variables** for dark/light mode support
- Implement **structured logging** with `ILogger<T>`
- Use **async/await** for all I/O operations
- Follow **nullable reference types** best practices

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **Bit Platform**: For the amazing Blazor UI components and project template
- **Microsoft**: For .NET, ASP.NET Core, and AI tools
- **OpenAI**: For embedding models and AI capabilities
- **.NET Community**: For valuable libraries and tools

---

**Built with ❤️ using .NET 10.0 and Blazor**