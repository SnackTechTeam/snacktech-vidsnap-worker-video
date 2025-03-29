
#Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /src

COPY src/core.domain/core.domain.csproj core.domain/
COPY src/core.application/core.application.csproj core.application/
COPY src/adapter.amazon.s3/adapter.amazon.s3.csproj adapter.amazon.s3/
COPY src/adapter.amazon.sqs/adapter.amazon.sqs.csproj adapter.amazon.sqs/
COPY src/adapter.video/adapter.video.csproj adapter.video/
COPY src/adapter.api/adapter.api.csproj adapter.api/

RUN dotnet restore adapter.api/adapter.api.csproj

COPY src/core.domain core.domain/
COPY src/core.application core.application/
COPY src/adapter.amazon.s3 adapter.amazon.s3/
COPY src/adapter.amazon.sqs adapter.amazon.sqs/
COPY src/adapter.video adapter.video/
COPY src/adapter.api adapter.api

RUN dotnet build adapter.api/adapter.api.csproj -c Release -o /app/build

#Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /src

# Instala o FFmpeg
RUN apt-get update && apt-get install -y ffmpeg && rm -rf /var/lib/apt/lists/*

COPY --from=build-env /app/build .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "adapter.api.dll"]