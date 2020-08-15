# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_12.x | bash - && \
    apt-get install -y build-essential nodejs
RUN npm install @angular/cli -g
# End setup

WORKDIR /source

# copy csproj and restore as distinct layers
#COPY *.sln .
COPY src/*.csproj ./aspnetapp/
WORKDIR /source/aspnetapp
RUN dotnet restore



# copy everything else and build app
COPY src/  /source/aspnetapp/
RUN ls -al /source/aspnetapp/

WORKDIR /source/aspnetapp
RUN dotnet publish -c release -o /app --no-restore


# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "dotnet-test-core-app.dll"]

#docker build --pull -t mytestcoreapp .
#docker run --rm -it -p 8000:80 mytestcoreapp:latest