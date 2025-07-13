# Build stage (используем SDK 9.0)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем только файлы проектов для кеширования слоев
COPY *.sln .
COPY Api/*.csproj ./Api/
COPY Client/*.csproj ./Client/
COPY DAL/*.csproj ./DAL/
COPY Domain/*.csproj ./Domain/
COPY Shared/*.csproj ./Shared/
COPY GameBrain/*.csproj ./GameBrain/
COPY DAL.Contracts/*csproj ./DAL.Contracts/

# Восстанавливаем зависимости
RUN dotnet restore

# Копируем весь код и собираем
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage (используем ASP.NET Core 9.0)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Копируем собранное приложение
COPY --from=build /app/publish .

# Устанавливаем зависимости для миграций
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    curl \
    postgresql-client \
    dos2unix && \
    rm -rf /var/lib/apt/lists/*

# Настраиваем entrypoint
COPY entrypoint.sh .
RUN dos2unix entrypoint.sh && \
    chmod +x entrypoint.sh

# Healthcheck для проверки работы API
HEALTHCHECK --interval=30s --timeout=5s \
    CMD curl -f http://localhost/health || exit 1

ENTRYPOINT ["./entrypoint.sh"]