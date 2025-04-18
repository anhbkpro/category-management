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
- Repository Pattern
- CQRS with MediatR (optional)
- AutoMapper

### Frontend
- Vue 3 with Composition API
- Vue Router for navigation
- Modern JavaScript (ES6+)
- CSS3 with Flexbox and Grid

## 📂 Project Structure

### Backend Structure

```
CategoryManagement.API/
├── Controllers/
│   ├── CategoriesController.cs
│   └── SessionsController.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── Repositories/
│   │   ├── Repository.cs
│   │   ├── CategoryRepository.cs
│   │   └── SessionRepository.cs
│   └── Migrations/
├── Models/
│   ├── Domain/
│   │   ├── Category.cs
│   │   ├── CategoryCondition.cs
│   │   ├── Session.cs
│   │   ├── Speaker.cs
│   │   ├── Tag.cs
│   │   ├── SessionTag.cs
│   │   └── SessionSpeaker.cs
│   └── DTOs/
│       ├── CategoryDto.cs
│       ├── CategoryConditionDto.cs
│       ├── SessionDto.cs
│       └── SpeakerDto.cs
├── Services/
│   ├── CategoryService.cs
│   └── SessionService.cs
├── Mapping/
│   └── MappingProfile.cs
├── Program.cs
└── appsettings.json
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

### Backend Setup

1. Clone the repository
   ```bash
   git clone https://github.com/yourusername/category-management.git
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
   dotnet ef database update
   ```

4. Start the API
   ```bash
   dotnet run
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

## 🗃️ Database Schema

The database consists of the following tables:

1. **Sessions**: Stores session information including title, description, dates, and location
2. **Speakers**: Contains speaker details
3. **SessionSpeakers**: Many-to-many relationship between sessions and speakers
4. **Tags**: Stores tag definitions
5. **SessionTags**: Many-to-many relationship between sessions and tags
6. **Categories**: Stores category metadata
7. **CategoryConditions**: Stores filter conditions for each category

## 🏗️ Architecture

This application follows a clean, layered architecture:

1. **Presentation Layer**: Vue 3 frontend and ASP.NET Core Web API controllers
2. **Service Layer**: Business logic encapsulated in services
3. **Repository Layer**: Data access implemented with the repository pattern
4. **Domain Layer**: Core entities and business rules

### Key Architectural Decisions

- **Repository Pattern**: Abstracts data access and enables testability
- **DTOs**: Separate data transfer objects to control what data is exposed to the client
- **Composition API**: Vue 3 Composition API for better code organization and TypeScript support
- **Optimized Queries**: Efficient SQL queries using proper indexing and eager loading

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
