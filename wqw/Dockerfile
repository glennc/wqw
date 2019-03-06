FROM microsoft/dotnet:3.0-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:3.0-sdk-stretch AS build
WORKDIR /src
COPY ["wqw/wqw.csproj", "wqw/"]
RUN dotnet restore "wqw/wqw.csproj"
COPY . .
WORKDIR "/src/wqw"
RUN dotnet build "wqw.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "wqw.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "wqw.dll"]