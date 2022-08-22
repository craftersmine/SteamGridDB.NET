# SteamGridDB.NET
A .NET implementation of API for [SteamGridDB](https://www.steamgriddb.com) written completely in C#

[![GitHub](https://img.shields.io/github/license/craftersmine/SteamGridDB.NET?color=darklime)](/LICENSE) 
[![Nuget](https://img.shields.io/nuget/v/craftersmine.SteamGridDB.Net)](https://www.nuget.org/packages/craftersmine.SteamGridDB.Net) 
[![GitHub Project Wiki](https://img.shields.io/badge/docs-github--wiki-brightgreen)](https://github.com/craftersmine/SteamGridDB.NET/wiki)
![Discord](https://img.shields.io/badge/discord-craftersmine%237441-5865f2)

![Repository Preview](https://raw.githubusercontent.com/craftersmine/SteamGridDB.NET/master/.github/RepositoryPreview.png)

## Supports:
* All types of SteamGridDB items (Grids, Heroes, Icons and Logos)
* Get, Upload and Delete items
* Search for supported games on SteamGridDB

Relied on .NET `HttpClient`

How to use library and XML documentation can be found here:
[Repository Wiki](https://github.com/craftersmine/SteamGridDB.NET/wiki)

If you want a new feature for library [create new feature request issue](https://github.com/craftersmine/SteamGridDB.NET/issues/new?assignees=&labels=enhancement&template=feature_request.md&title=)

## Installation:
* Search for `craftersmine.SteamGridDB` in NuGet explorer in Visual Studio (or your IDE)
* Using NuGet Package Manager: ```PM> Install-Package craftersmine.SteamGridDB.Net```
* Download NuGet package from [Releases](https://github.com/craftersmine/SteamGridDB.NET/releases) page and put it in your [Local NuGet Feed](https://docs.microsoft.com/en-us/nuget/hosting-packages/overview)

## Usage:
* Add `using craftersmine.SteamGridDB` directive
* Instantiate new object of type `SteamGridDb` with your API key
More information [here](https://github.com/craftersmine/SteamGridDB.NET/wiki/Getting-started-and-Using-the-library)
