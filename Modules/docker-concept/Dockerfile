#use the dotnet sdk img to build the and run the app 

#FROM mcr.microsoft.com/dotnet/sdk:10.0

#set the working directory in the container

#WORKDIR /app

#COPY everything from the current directory to the container

#COPY . ./ 

#restore the dependencies

#RUN dotnet restore 

#Build the app in release configuration

#RUN dotnet  publish -c Release -o release 

#Set the working directory to the release folder where the app is published

#WORKDIR /app/release 

#set the entry point to run the app when the container starts

#ENTRYPOINT ["dotnet", "DockerEssentials.dll"]



#FROM: Specifies the base image to build upon. Here, it uses the official .NET SDK 8.0 image from Microsoft’s container registry (mcr.microsoft.com/dotnet/sdk:10.0). This image includes the .NET SDK necessary to build and publish .NET applications.
#WORKDIR: Sets the working directory inside the container to /app. Subsequent instructions will be executed relative to this directory.
#COPY: Copies the current directory (.) from the host machine (where the Docker build command is run) to the /app directory inside the container. This assumes the Dockerfile is in the root of your project directory.
#RUN: Executes a command (dotnet restore) during the build process. This command restores the NuGet packages required by the .NET application, based on the *.csproj files found in the current directory.
#RUN: Executes another command (dotnet publish) to build the .NET application in Release configuration (-c Release) and publish the output (-o release) to the /app/release directory inside the container. This step compiles the application and prepares it for deployment.
#WORKDIR: Changes the working directory to /app/release inside the container. This prepares for the next step to set the entry point.
#ENTRYPOINT: Specifies the command to run when the container starts. Here, it sets the entry point to execute the .NET application by running dotnet DockerEssentials.dll. Adjust DockerEssentials.dll to match the name of your published application’s DLL.





# stage 1: build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

#stage 2: create the runtime image

FROM mcr.microsoft.com/dotnet/aspnet:10.0 
WORKDIR /app
 COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "docker-concept.dll"]