# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# Setup NodeJs
RUN apt-get update && \
    apt-getinstall -y wget && \
    apt-getinstall -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_8.x | bash - && \
    apt-getinstall -y build-essential nodejs
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
ENTRYPOINT ["dotnet", "aspnetapp.dll"]

#docker build --pull -t mytestcoreapp .