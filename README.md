Name: Valeri Nossov
Student-code: 233229IADB
Email: vanoss@taltech.ee
Uni-id: vanoss

Firstly install EF
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef

EF Core will not automatically create for you db creation code
Run migrations to build db for WebApp and Console App:

dotnet ef migrations add InitialCreate --project DAL --startup-project ConsoleApp 
dotnet ef database update --project DAL --startup-project ConsoleApp 
dotnet ef database drop --project DAL --startup-project ConsoleApp 

dotnet ef migrations add InitialCreate --project DAL --startup-project WebApp
dotnet ef database update --project DAL --startup-project WebApp
dotnet ef database drop --project DAL --startup-project WebApp 

TO update migrations:
dotnet ef migrations remove --project DAL --startup-project WebApp
dotnet ef migrations add InitialCreate --project DAL --startup-project WebApp
dotnet ef database update --project DAL --startup-project WebApp
