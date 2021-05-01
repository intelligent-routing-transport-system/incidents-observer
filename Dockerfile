FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
LABEL version="1.0" maintainer="TCC"
WORKDIR /app
COPY ./dist .
ENV ASPNETCORE_URLS=http://+:5002
EXPOSE 3306
EXPOSE 5002
ENTRYPOINT ["dotnet", "incidents-observer.dll"]