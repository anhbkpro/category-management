# Dynamic Category Management System

A full-stack application for managing dynamic categories to filter and organize learning sessions. This system allows administrators to create complex filtering conditions and preview which sessions match these conditions.

## 🚀 Features

- **Category Management**: Create, edit, and delete categories with dynamic filtering conditions
- **Complex Filtering**: Filter sessions by tags, location, and date ranges
- **Session Preview**: View sessions that match category conditions with pagination and sorting
- **Responsive UI**: Clean, modern interface that works on all devices

## 🛠️ Tech Stack

### Backend
- ASP.NET Core 6.0 Web API
- Entity Framework Core 6.0
- SQL Server
- Clean Architecture with Core and Infrastructure layers
- Repository Pattern
- AutoMapper

### Frontend
- Vue 3 with Composition API
- Vue Router for navigation
- Modern JavaScript (ES6+)
- CSS3 with Flexbox and Grid

## 📂 Project Structure

### Backend Structure

```
CategoryManagement/
├── CategoryManagement.API/
│   ├── Controllers/
│   │   ├── CategoriesController.cs
│   │   └── SessionsController.cs
│   ├── Program.cs
│   └── appsettings.json
├── CategoryManagement.Core/
│   ├── Domain/
│   │   ├── Entities/
│   │   │   ├── Category.cs
│   │   │   ├── CategoryCondition.cs
│   │   │   ├── Session.cs
│   │   │   ├── Speaker.cs
│   │   │   ├── Tag.cs
│   │   │   ├── SessionTag.cs
│   │   │   └── SessionSpeaker.cs
│   │   └── Interfaces/
│   │       ├── ICategoryRepository.cs
│   │       └── ISessionRepository.cs
│   ├── DTOs/
│   │   ├── CategoryDto.cs
│   │   ├── CategoryConditionDto.cs
│   │   ├── SessionDto.cs
│   │   └── SpeakerDto.cs
│   └── Services/
│       ├── CategoryService.cs
│       └── SessionService.cs
└── CategoryManagement.Infrastructure/
    ├── Persistence/
    │   ├── ApplicationDbContext.cs
    │   ├── Configurations/
    │   │   ├── CategoryConfiguration.cs
    │   │   └── SessionConfiguration.cs
    │   └── Repositories/
    │       ├── Repository.cs
    │       ├── CategoryRepository.cs
    │       └── SessionRepository.cs
    └── Migrations/
        └── [timestamp]_InitialMigration.cs
```

### Frontend Structure

```
src/
├── assets/
│   └── styles/
│       └── main.css
├── components/
│   ├── common/
│   │   ├── Pagination.vue
│   │   └── LoadingSpinner.vue
│   ├── categories/
│   │   ├── CategoryList.vue
│   │   ├── CategoryForm.vue
│   │   └── CategoryPreview.vue
│   └── sessions/
│       └── SessionList.vue
├── composables/
│   ├── useCategories.js
│   └── useSessions.js
├── router/
│   └── index.js
├── services/
│   ├── api.js
│   ├── categoryService.js
│   └── sessionService.js
├── views/
│   ├── HomeView.vue
│   ├── CategoriesView.vue
│   └── CategoryPreviewView.vue
├── App.vue
└── main.js
```

## 🔧 Setup Instructions

### Prerequisites

- .NET 6.0 SDK
- Node.js (v14 or later)
- SQL Server
- Docker and Docker Compose (for containerized deployment)

### Backend Setup

1. Clone the repository
   ```bash
   git clone git@github.com:anhbkpro/category-management.git
   cd category-management/backend
   ```

2. Update the connection string in `appsettings.json`
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=CategoryManagement;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

3. Run EF Core migrations to create the database
   ```bash
   dotnet ef database update --project CategoryManagement.Infrastructure --startup-project CategoryManagement.API
   ```

4. Start the API
   ```bash
   dotnet run --project CategoryManagement.API
   ```

### Frontend Setup

1. Navigate to the frontend directory
   ```bash
   cd ../frontend
   ```

2. Install dependencies
   ```bash
   npm install
   ```

3. Update the API URL in `.env.local`
   ```
   VUE_APP_API_URL=http://localhost:5093/api
   ```

4. Start the development server
   ```bash
   npm run serve
   ```

5. Build for production
   ```bash
   npm run build
   ```

### Docker Compose Setup

The application can be started using Docker Compose, which will set up the entire stack including the database, API, and frontend.

1. Make sure Docker and Docker Compose are installed on your system
   ```bash
   docker --version
   docker-compose --version
   ```

2. Navigate to the project root directory
   ```bash
   cd category-management
   ```

3. Start the application using Docker Compose
   ```bash
   docker-compose up -d
   ```

4. Check the status of the containers
   ```bash
   docker-compose ps
   ```

5. Access the application:
   - Frontend: http://localhost:8080
   - API: http://localhost:5001
   - API Health Check: http://localhost:5001/health

6. View logs from the containers
   ```bash
   docker-compose logs -f
   ```

7. Stop the application
   ```bash
   docker-compose down
   ```

8. To rebuild the containers after making changes
   ```bash
   docker-compose up -d --build
   ```

The Docker Compose setup includes:
- SQL Server database with persistent volume
- ASP.NET Core API with health checks
- Vue.js frontend
- Proper service dependencies and health checks to ensure services start in the correct order

## 🗃️ Database Schema

The database consists of the following tables:

1. **Sessions**: Stores session information including title, description, dates, location, and audit fields
2. **Speakers**: Contains speaker details with audit fields
3. **SessionSpeakers**: Many-to-many relationship between sessions and speakers with audit fields
4. **Tags**: Stores tag definitions with audit fields
5. **SessionTags**: Many-to-many relationship between sessions and tags
6. **Categories**: Stores category metadata with audit fields
7. **CategoryConditions**: Stores filter conditions for each category with audit fields

## 🏗️ Architecture

This application follows Clean Architecture principles with the following layers:

1. **Core Layer** (`CategoryManagement.Core`):
   - Domain Entities: Core business objects
   - Interfaces: Repository contracts
   - DTOs: Data transfer objects
   - Services: Business logic implementation

2. **Infrastructure Layer** (`CategoryManagement.Infrastructure`):
   - Persistence: Database context and configurations
   - Repositories: Data access implementations
   - Migrations: Database schema changes

3. **API Layer** (`CategoryManagement.API`):
   - Controllers: HTTP endpoints
   - Configuration: Application settings
   - Dependency Injection setup

### Key Architectural Decisions

- **Clean Architecture**: Separation of concerns with Core and Infrastructure layers
- **Repository Pattern**: Abstracts data access and enables testability
- **DTOs**: Separate data transfer objects to control what data is exposed to the client
- **Entity Configurations**: Fluent API configurations for entity relationships and constraints
- **Audit Fields**: Tracking of creation and modification timestamps across entities

## 🔒 Performance Considerations

1. **Indexed Filtering**: Database indexes on commonly filtered fields (StartDate, Location)
2. **Pagination**: All session queries support pagination to limit data transfer
3. **Eager Loading**: Optimized loading of related entities to avoid N+1 query problems
4. **Caching**: Client-side caching of category data to reduce API calls
5. **Optimized Frontend**: Efficient Vue components that only re-render when necessary

## 📱 SEO Considerations

1. **Semantic HTML**: Proper HTML structure for better crawling
2. **Meta Description**: Dynamic meta descriptions for each route
3. **Title Tags**: Customized title tags for each page
4. **Clean URLs**: Semantic URL structure
5. **Mobile Responsive**: Fully responsive design for all devices

## 🧪 Testing Strategy

1. **Unit Tests**: Test individual components and services
2. **Integration Tests**: Test API endpoints and data flow
3. **E2E Tests**: Test complete user journeys with Cypress
4. **Performance Testing**: Benchmark query performance with large datasets

## 🚀 Deployment

1. **Backend**: Deploy as a standalone .NET Core application or containerize with Docker
2. **Frontend**: Build static files and serve from CDN or containerize with Nginx
3. **Database**: Use SQL Server in production with proper backup strategies

## 📊 Example Use Cases

1. **Cloud Learning (May):**
   - Include Tags: aws, azure
   - Exclude Tags: beginner, basic
   - Location: Online
   - Time Range: 2025-05-01 to 2025-05-31

2. **Advanced DevOps Training:**
   - Include Tags: kubernetes, docker, devops
   - Exclude Tags: beginner
   - Location: Any
   - Time Range: Any

3. **In-Person Database Sessions:**
   - Include Tags: database
   - Exclude Tags: None
   - Location: Not Online
   - Time Range: Any

## 📄 License

This project is licensed under the MIT License
