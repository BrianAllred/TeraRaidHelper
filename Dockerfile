FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY RaidHelper.csproj .
RUN apt-get update -y
RUN apt-get install -y python3
RUN dotnet workload install wasm-tools
RUN dotnet restore RaidHelper.csproj
COPY . .
RUN dotnet build RaidHelper.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish RaidHelper.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf