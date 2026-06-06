# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the specific project file and restore dependencies
COPY BabyfloServer/BabyfloServer.csproj BabyfloServer/
RUN dotnet restore "BabyfloServer/BabyfloServer.csproj"

# Copy the rest of the source files and build
COPY . .
WORKDIR /src/BabyfloServer
RUN dotnet publish "BabyfloServer.csproj" -c Release -o /app/publish

# Use the ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port Render uses
ENV ASPNETCORE_URLS=http://+:10000
ENTRYPOINT ["dotnet", "BabyfloServer.dll"]
