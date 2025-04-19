```bash
dotnet ef database update --project backend/CategoryManagement/CategoryManagement.Infrastructure --startup-project backend/CategoryManagement/CategoryManagement.API
```
- It looks like the migration is already applied, but the stored procedure might not have been created correctly. Let's try to remove the migration and create a new one:
```bash
dotnet ef migrations remove --project backend/CategoryManagement/CategoryManagement.Infrastructure --startup-project backend/CategoryManagement/CategoryManagement.API
```
- Then, create a new migration:
```bash
dotnet ef migrations add AddGetSessionsByCategoryStoredProcedure --project backend/CategoryManagement/CategoryManagement.Infrastructure --startup-project backend/CategoryManagement/CategoryManagement.API
```
