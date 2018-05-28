FROM microsoft/dotnet
RUN mkdir -p /app
COPY ./bin/Debug/netcoreapp2.0/publish /app
WORKDIR /app
EXPOSE 5000
ENTRYPOINT dotnet DotNetCore.SSO.dll