# Tera Raid Helper (Pokémon Scarlet/Violet)

[![Docker](https://img.shields.io/docker/pulls/brianallred/tera-raid-helper)](https://hub.docker.com/r/brianallred/tera-raid-helper/) [![Website](https://img.shields.io/website?url=https%3A%2F%2Fbrianallred.github.io%2FTeraRaidHelper%2F)](https://brianallred.github.io/TeraRaidHelper/)

Have you ever been preparing for a Tera Raid in Scarlet/Violet and said "Wait... What am I supposed to bring to fight a Fairy Tera type Haxorus?"

Then this tool is for you!

Tera Raid Helper will suggest types to bring and avoid, including an explanation why for each type.

## How it works

Tera Raid Helper pulls detailed type information from [PokeAPI](https://pokeapi.co/) in order to determine what types the raid Pokémon will be weak and resistant to. It will also determine what types are weak to or resist the raid Pokémon's natural typing.

It uses this information to suggest types to bring and which to avoid. For example for a Fairy Tera type Haxorus, it would suggest bringing Poison and Steel type attacks (super effective against Fairy) and avoiding bringing a Dragon type (since Haxorus is Dragon type).

Tera Raid Helper uses [PokeApiNet](https://github.com/mtrdp642/PokeApiNet) internally with its default caching mechanism. This means that issues stemming from API rate limiting are unlikely since the API requests are made from each user's browser indivudally and each user is unlikely to hit the PokeAPI rate limit.

## Use it

### GitHub Pages

Check it out in action [here](https://brianallred.github.io/TeraRaidHelper/)!

### Self-hosted

1. Install Docker.
2. Run `$ docker run --rm -d -p 8080:80 brianallred/tera-raid-helper`
3. Open a browser and navigate to `http://localhost:8080`

Replace `8080` with a different port if necessary.

Replace `localhost` with the IP address of the host if not running locally.

## Roadmap

When I initially started this project, I envisioned users picking a raid Pokémon and getting Pokémon suggestions, as opposed to types. Unfortunately, at the time of release, PokeAPI did not have Generation 9 data.

TODO:

- [ ] Select raid Pokémon instead of types
- [ ] Suggest Pokémon instead of types
- [ ] Use Pokémon move information for suggestions instead of just types
- [ ] Translations

## Contribute

Tera Raid Helper is a Blazor WASM application, so all code is written in C#/Razor/HTML/JS/CSS, compiled ahead-of-time, and running locally on the browser.

The UI is built using [MudBlazor](https://mudblazor.com).

To contribute, you'll need:

- .NET 7 SDK
- Docker/Podman (for building and testing images)

Pull the repo and then run `dotnet restore` to begin. You may need to run `dotnet workload restore` to enable AOT compilation. Linux and Mac users may need to preface the previous command with `sudo`.

All PRs are welcome!
