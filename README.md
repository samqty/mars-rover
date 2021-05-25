# mars rover photo downloader 
This repository contains a solution for the coding excercise from https://github.com/WatchGuard/coding-exercises, below is quick summary of the solution included in this repo.

## Summary

The solution for this coding exercise is implemented as asp.net web application. The ability to download photos from mars rover is exposed on a simple web interface. And the download images are also displayed on this web application. This is a multi tier/layer application conceptually but all the layers are housed in the same project for simplicity purposes. Below are the layers used to orchestirate the application processing from the UI to the back end API supporting it.

### Controllers
This space primary contains code related to rendering HTML web pages using rasor and http endpoints to support AJAX calls to the server for hydrating the UI or performing long running tasks.

### Services
This layer is the primary component that will be used to bridge the UI needs to what is available in the backend. This is where all the business logic/validations and such go.

### Data
This layer contains the contract and implementation to the source of the data and low level data processing needed by this application. for this coding excercise, it contains implementations that interact directly with Nasa Api endpoints, and the implementation that accesses the file system.

### Infrastructure
This space contain general infrastructure code, like typed appsetting config. And it also contains a utility that reports logs to the client browser using SignarlR.

### Clientside UI
This is a rough implemenation of the UI for a browser. This side of the application is light and contains minimal script and markup to enable client side rendering of data using databinding.

## Getting Started

In order to run the application, 
1) you can do that via visual studio (it is built using visual studio 2019).
2) Command line  ```dotnet run```
3) In docker container - use ```docker-compose.yml``` to build and run.

### Local Folder Location Configs
The local directories for both the list of dates as well as, where the downloaded images are stored is configured via an appsetting entry. To change the location anywhere other than the default value open the appsetting.json file and update these two keys
1) DownloadDestinationDirectory
2) DatesFilesDirectory

## Tools and Languages Used
* Visual Studio 2019 IDE
* .Net 5
* Angularjs
* SignalR
* Refit
* xUnit
* Serilog

## Tests
Unit test coverage for happy path is also include and uses xUnit

## Future Considerations
This project is build to fulfill the acceptance criteria and show case some level of application architecture as well as ability to put together a functioning application. Some area of extending this project include:
1) Additional unit test coverage
2) Directly upload list of dates
3) Ability to partially display photos as they are getting downloaded
4) Better Ability to view individual photos Enlarged
5) client side unit test coverage

This list can keep on going :)
