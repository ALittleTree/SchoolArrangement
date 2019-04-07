FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["ContosoUniversity.csproj", ""]
RUN dotnet restore "/ContosoUniversity.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ContosoUniversity.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ContosoUniversity.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ContosoUniversity.dll"]