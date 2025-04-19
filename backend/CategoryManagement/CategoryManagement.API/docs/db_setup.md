# Setting Up SQL Server on Mac M1 and Connecting with DBeaver
1. Create and Run the SQL Server Container
First, set up an Azure SQL Edge container (optimized for M1 Macs) using Docker:
```bash
docker run -d --name sql-server \
  -e 'ACCEPT_EULA=Y' \
  -e 'SA_PASSWORD=YourStrongPassword123!' \
  -p 1433:1433 \
  -v sqlvolume:/var/opt/mssql \
  mcr.microsoft.com/azure-sql-edge
```
1. Connect to SQL Server Using DBeaver
Open DBeaver on your Mac
Create a new connection:

Click on the "New Database Connection" icon (plug/socket icon)
Search for "Microsoft SQL Server" and select it
Click "Next"


Configure connection details:

Host: `localhost`
Port: `1433`
Database: Leave `empty` (or use "master")
Authentication: SQL Server Authentication
Username: `sa`
Password: `YourStrongPassword123!`


Set security properties:

Click on the "Driver properties" tab
Find "Trust Server Certificate" and set it to "true"


Test the connection:

Click the "Test Connection" button
If successful, you'll see a "Connected" message
Click "Finish" to save the connection

1. Configure Your Application

Update your connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=CategoryManagement;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
