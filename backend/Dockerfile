# =====================================================================
# Finance Control - API Dockerfile (multi-stage)
# =====================================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia o csproj de cada projeto para aproveitar cache de restore
COPY Directory.Build.props ./
COPY src/FinanceControl.Api/FinanceControl.Api.csproj                   src/FinanceControl.Api/
COPY src/FinanceControl.Application/FinanceControl.Application.csproj   src/FinanceControl.Application/
COPY src/FinanceControl.Domain/FinanceControl.Domain.csproj             src/FinanceControl.Domain/
COPY src/FinanceControl.CrossCutting/FinanceControl.CrossCutting.csproj src/FinanceControl.CrossCutting/
COPY src/FinanceControl.Data/FinanceControl.Data.csproj                 src/FinanceControl.Data/
COPY src/FinanceControl.Infrastructure/FinanceControl.Infrastructure.csproj src/FinanceControl.Infrastructure/

RUN dotnet restore src/FinanceControl.Api/FinanceControl.Api.csproj

# Copia o restante e publica
COPY . .
RUN dotnet publish src/FinanceControl.Api/FinanceControl.Api.csproj \
    -c Release -o /app --no-restore /p:UseAppHost=false

# ---------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
    CMD curl --fail http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "FinanceControl.Api.dll"]
