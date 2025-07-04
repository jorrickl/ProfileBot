# ProfileBot

ProfileBot is a .NET 9 based Discord bot for tracking RuneMetrics profiles from RuneScape, based on [NetCord](https://netcord.dev/).


## Features
- **Track RuneMetrics profiles**: Use Discord slash commands to track and view player activities.

## Getting Started
1. Clone the repository using the following commands:
``` 
   git clone https://github.com/jorrickl/ProfileBot.git
   cd ProfileBot 
```
2. Set your Discord bot token in `src/ProfileBot.Api/appsettings.json`:
 ```json
   "Discord": {
     "Token": "<your_discord_token>"
   }
 ```
3. Run the bot in Visual Studio 2022 (or higher)

## Development
`appsettings.json` uses the official RuneMetrics API by default. 
For local development against the official API, you can change the `ProfileBaseUrl` in `appsettings.Development.json`.

## Tech Stack
- .NET 9
- [NetCord](https://netcord.dev/)
- [MediatR](https://github.com/LuckyPennySoftware/MediatR)
- [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- [WireMock.NET](https://github.com/wiremock/WireMock.Net) (stub)
- MSTest, AutoFixture, Shouldly (tests)
