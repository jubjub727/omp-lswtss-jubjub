# Open Modding Platform for LEGO Star Wars: The Skywalker Saga

## Support

At this moment, discussion about all project's related stuff is held on [**TTGames LEGO Modding Discord Server**](https://discord.gg/9gYXPka). Feel free to join and find the discussion in `modding_forum/Open Modding Platform - LSW:TSS (Development Updates)` thread.

## Contributing

### Requirements

- [Rust](https://www.rust-lang.org/tools/install)
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [NodeJS](https://nodejs.org/en/download/package-manager)
- [Yarn Classic](https://classic.yarnpkg.com/lang/en/docs/install)
- [LEGO Star Wars: The Skywalker Saga (Steam Version)](https://store.steampowered.com/app/920210/LEGO_Star_Wars_The_Skywalker_Saga/)
- [LEGO Star Wars: The Skywalker Saga (EGS Version)](https://store.epicgames.com/en-US/p/lego-star-wars-the-skywalker-saga)

### Building

Navigate to `workspaces/dotnet/dev-tools` and execute:
```sh
dotnet run -- build-all [PATH_TO_STEAM_GAME_DIRECTORY] [PATH_TO_EGS_GAME_DIRECTORY]
```

### Testing

Navigate to `workspaces/dotnet/dev-tools` and execute:
```sh
dotnet run -- install-all [PATH_TO_GAME_DIRECTORY]
```

Now, start the game from Steam/EGS.

For Steam version, you can use this command to inspect process stdout:
```sh
dotnet run -- run-steam-game [PATH_TO_STEAM_GAME_DIRECTORY]
```

## Special Thanks

To all members of **TTGames LEGO Modding Discord Server** for unparallel help and guidance ❤️
