# SigilADay
Collection of custom sigils added to daily by Julian Perge.

This was forked from Cyantist's [repo](https://github.com/ScottWilson0903/SigilADay/) as I wanted to make my own sigils that could be used in the [JSONCardLoader](https://github.com/MADH95/JSONLoaderPlugin)

## Sigils so far
### Day 1
- Excavator: When played, remove all `Terrain` cards on your side of the field. For each card removed, place a `Squirrel` in your hand.
- Nest: When [creature] is struck, you may select a card from a specific draw pile.
  * Cards are: `Tail_Bird`, `Tail_Furry`,`Tail_Insect`,`Boulder`
  * These cards cannot currently be changed at the moment (possible feature in future)
- Split: When [creature] is played, create a copy an open space on your side of the field.

### Day 2
- Flight Of The Valkyrie: When [creature] is played, fill all open slots on your side of the field with 1/1 `Norse Warrior`s.
  * `Norse Warriors` is defined as: 1 Power, 1 Health,`Brittle`.

### Day 3
- Miasma - When [creature] dies, spawn a Greater Smoke in its place.
  * A `Greater Smoke` is defined as: 1 Power, 3 Health, `QuadrupleBones`.

### Day 4
- `Prospect`: When [creature] damages another creature, that creature turns into a Golden Nugget. Gain 1 gold tooth upon destroying a Golden Nugget.
- `Golden Nugget`: When [creature] dies, gain 1 gold tooth

# Changelog

## v1.4.1
- Fixed `Cannibal` ability property getter to now correctly return the static field.

## v1.4.0
- Created `Prospect` ability;
- Created `Golden Nugget` ability;
- Updated descriptions of each ability.

## v1.3.0
- Created `Cannibal` ability;

## v1.2.0
- Removed abstract class `CustomAbilityBehavior` due to static reference issues where the only NewAbility that can be added to cards is the last one created.
- Changed list of cards in `Nest` ability that can be tutored for. New cards are: `Tail_Bird`, `Tail_Furry`,`Tail_Insect`,`Boulder`

## v1.1.0
- Updated to use API v1.12.1
- Corrected logic for checking for if PlayerSlots are full in `Split` ability.
- Added Resource image loading so that users no longer have to copy/paste the artwork folder. Images are built with the .dll

## Installation (automated)
This is the recommended way to install the API on the game.

- Download and install [Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager) or [r2modman](https://timberborn.thunderstore.io/package/ebkr/r2modman/)
- Click Install with Mod Manager button on top of the [page](https://inscryption.thunderstore.io/package/Cyantist/SigilADay/)
- Run the game via the mod manager

## Installation (manual)
To install this plugin first you need to install BepInEx as a mod loader for Inscryption. A guide to do this can be found [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex). Inscryption needs the 86x (32 bit) mono version.

You will also need to install the [CardLoaderPlugin](https://github.com/ScottWilson0903/InscryptionAPI)
To install Inscryption API you simply need to copy **API.dll** from [releases](https://github.com/ScottWilson0903/InscryptionAPI/releases) to **Inscryption/BepInEx/plugins**.

To install SigilADay_julianperge.dll you simply need to copy **SigilADay_julianperge.dll** folder from the source code zip in [releases](https://github.com/julian-perge/SigilADay/releases) to a **JulianPerge-SigilADay** folder inside **Inscryption/BepInEx/plugins**.