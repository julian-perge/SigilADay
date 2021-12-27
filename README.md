# SigilADay (except weekends, sometimes)

Collection of custom sigils added to daily by Julian Perge.

This was forked from Cyantist's [repo](https://github.com/ScottWilson0903/SigilADay/) as I wanted to make my own sigils that could be used in the [JSONCardLoader](https://github.com/MADH95/JSONLoaderPlugin)

[Changelog](https://github.com/julian-perge/SigilADay/blob/main/CHANGELOG.md)

## INACTIVE UNTIL FIXED (as of version 1.12.6)

- Exodia
- Hermit

## Example with JSONCardLoader

Using these custom sigils in one of your created cards or modify existing cards.
File name `geck.jldr` in `MADH95Mods-JSONCardLoader` folder.

```json
{
  "name": "Geck",
  "fieldsToEdit": ["customAbilities"],
  "customAbilities": [
    {
      "name": "Evolve Randomly",
      "GUID": "julianperge.inscryption.sigiladay"
    }
  ]
}
```

## Installation (automated)

This is the recommended way to install the API on the game.

- Download and install [Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager) or [r2modman](https://timberborn.thunderstore.io/package/ebkr/r2modman/)
- Click Install with Mod Manager button on top of the [page](https://inscryption.thunderstore.io/package/Cyantist/SigilADay/)
- Run the game via the mod manager

## Installation (manual)

To install this plugin first you need to install BepInEx as a mod loader for Inscryption. A guide to do this can be found [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex). Inscryption needs the 86x (32 bit) mono version.

You will also need to install the [CardLoaderPlugin](https://github.com/ScottWilson0903/InscryptionAPI)
To install Inscryption API you simply need to copy **API.dll** from [releases](https://github.com/ScottWilson0903/InscryptionAPI/releases) to **Inscryption/BepInEx/plugins**.

To install SigilADay_julianperge.dll you simply need to copy **SigilADay_julianperge.dll**folder from the source code zip in [releases](https://github.com/julian-perge/SigilADay/releases) to a**JulianPerge-SigilADay**folder inside**Inscryption/BepInEx/plugins**.

### Sigils so far

#### Day 1

- `Excavator` - When played, remove all `Terrain` cards on your side of the field. For each card removed, place a `Squirrel` in your hand.
- `Nest` - When [creature] is struck, you may select a card from a specific draw pile.
  - In `path_to_your_bepinex_install\BepInEx\config\`, there will be a file called `julianperge.inscryption.sigiladay.cfg`. If you don't see this, run the game for the first time and it should create the default `.cfg` file.
  - Please see [this example .cfg file](https://github.com/julian-perge/SigilADay/blob/main/Examples/julianperge.inscryption.sigiladay.cfg) for setting up the cards to tutor for.
- `Split` - When [creature] is played, create a copy an open space on your side of the field.

#### Day 2

- `Flight Of The Valkyrie` - When [creature] is played, fill all open slots on your side of the field with 1/1 `Norse Warrior`s.
  - `Norse Warriors` is defined as: 1 Power, 1 Health,`Brittle`.

#### Day 3

- `Miasma` - When [creature] dies, spawn a Greater Smoke in its place.
  - A `Greater Smoke` is defined as: 1 Power, 3 Health, `QuadrupleBones`.

#### Day 4

- `Prospect` - When [creature] damages another creature, that creature turns into a Golden Nugget. Gain 1 gold tooth upon destroying a Golden Nugget.
- `Golden Nugget` - When [creature] dies, gain 1 gold tooth
- `Belligerent` - When [creature] dies, create a copy of itself with a cumulative -1 to Power and Health. Has no effect on cards with 1 Health.
  - Idea and icon courtesy of Puppetsquid#6087 on discord.

#### Day 5

- `Evolve Randomly` - A card bearing this sigil will grow into a random form after 1 turn on the board.
  - Ability icon courtesy of Canas (discord name, `Snaketits#1535` is their ID) in the Inscryption modding discord.
  - In `path_to_your_bepinex_install\BepInEx\config\`, there will be a file called `julianperge.inscryption.sigiladay.cfg`. If you don't see this, run the game for the first time and it should create the default `.cfg` file.
  - If no list is provided, a card at pure random is chosen.
  - Please see [this example .cfg file](https://github.com/julian-perge/SigilADay/blob/main/Examples/julianperge.inscryption.sigiladay.cfg) for setting up random evolutions

#### Day 6

- `Hermit` (WIP) - Similar to Submerge ability, but instead can only be played on Terrain/Pelt cards and hides under them.

#### Day 7

- `Exodia` (migrated from main repo)
  - When [creature] has the left and right arm cards of Exodia on each respective side, you win the match.
  - This will add the left and right arm Exodia cards to the card pool. Only available through Trader offers.

#### Day 8

- will add soon (trademark)

#### Day 9

- will add soon (trademark)

#### Day 10

- `Brimstone`
  - Idea and icon courtesy of Skkull#0393 on discord.
  - Does not affect Terrain or Pelts.
  - When [creature] damages another card, overkill damage will always happen, but damage is set to 1.
  - If no card is queued to take overkill damage, your opponent takes 1 damage instead.
