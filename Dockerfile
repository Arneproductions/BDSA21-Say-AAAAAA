FROM mcr.microsoft.com/dotnet/sdk:6.0 as base
# Multi-stage enviroment variables
ENV ASPNETCORE_ENVIRONMENT="Production"
ENV ConnectionStrings:SELearning=""
ENV ConnectionStrings:ProductionConnectionString=""
ENV PATH="/root/.dotnet/tools:${PATH}"
ENV ASPNETCORE_URLS=http://localhost:80

# Multi-stage dependencies
RUN apt-get update && apt-get install -y python3
RUN dotnet workload install wasm-tools

# Restore
FROM base as restore
WORKDIR /source

COPY . .

RUN dotnet restore

# Build, based on restored
FROM restore as build
RUN dotnet build -c Release

# Publish, based on build
FROM build as publish 
RUN dotnet publish -c Release -o /app

# Serve built files
FROM base as serve
WORKDIR /app
COPY --from=publish /app .
COPY ./scripts/entrypoint.sh .

EXPOSE 80

CMD /bin/bash ./entrypoint.sh
