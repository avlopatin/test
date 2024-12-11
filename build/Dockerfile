ARG REPO=mcr.microsoft.com/dotnet
ARG SDK=${REPO}/sdk:9.0
ARG RUN=${REPO}/aspnet:9.0

##################### Installer #####################

FROM ${RUN} AS installer

ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080

RUN groupadd --gid 999 dotnet && \
    useradd --create-home --home-dir /app --uid 999 --gid dotnet dotnet

USER 999:999

##################### Build #####################

FROM ${SDK} AS build

ARG Version=0.0.0.0
ARG AssemblyVersion=0.0.0.0
ARG FileVersion=0.0.0.0
ARG InformationalVersion=0.0.0.0

RUN echo Application Version: ${Version}

WORKDIR /app/src
COPY . .

RUN dotnet restore

RUN dotnet build \
    --configuration Release \
    --no-restore \
    --nologo \
    -p:Version="${Version}" \
    -p:AssemblyVersion="${AssemblyVersion}" \
    -p:FileVersion="${FileVersion}" \
    -p:InformationalVersion="${InformationalVersion}"

##################### Test #####################

FROM build AS test

RUN dotnet test \
    --configuration Release \
    --verbosity normal \
    --no-build \
    --no-restore \
    --nologo

##################### Publish #####################

FROM test AS publish

RUN dotnet pack \
    --configuration Release \
    --output "/app/artifacts/nuget" \
    --no-build \
    --no-restore \
    --nologo \
    -p:PackageVersion="${Version}"

RUN dotnet publish \
    "./src/Api.Endpoint/Api.Endpoint.csproj" \
    --configuration Release \
    --output "/app/bin" \
    --no-build \
    --no-restore \
    --nologo

##################### Final #####################

FROM installer AS final

WORKDIR /app
COPY --from=publish /app/bin .

COPY --from=publish /app/artifacts /app/artifacts

ENTRYPOINT ["dotnet", "Api.Endpoint.dll"]
