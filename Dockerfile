# Runtime imajı
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Build imajı
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Projeyi kopyala ve restore et
COPY ["alacakverecektakip.csproj", "./"]
RUN dotnet restore "alacakverecektakip.csproj"

# Tüm kaynak kodu kopyala ve publish et
COPY . .
RUN dotnet publish "alacakverecektakip.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "alacakverecektakip.dll"]