# SigilADay
Collection of custom sigils added to daily by Julian Perge.

This was forked from Cyantist's [repo](https://github.com/ScottWilson0903/SigilADay/) as I wanted to make my own sigils that could be used in the [JSONCardLoader](https://github.com/MADH95/JSONLoaderPlugin)

## Sigils so far
### Day 1
- Excavator: When played, remove all `Terrain` cards on your side of the field. For each card removed, place a `Squirrel` in your hand.
- Nest: When attacked, this creature will `Tutor` for a card from a specific set of cards that is not your main deck.
- Split: When played, this creature will create a copy of itself in an open space on your side of the field.

### Day 2
- Flight Of The Valkyrie: When the creature is played, fill all open slots on your side of the field with 1/1 `Norse Warrior`s
  * `Norse Warriors` are defined as: 1/1 with `Brittle` ability.

### Day 3
- Miasma - When this creature dies, spawn a Greater Smoke creature in its place.
  * A `Greater Smoke` is defined as: 1/3 with `QuadrupleBones` ability.

# Changelog

## v1.2.0
- `Miasma`: When this creature dies, spawn a `Greater Smoke` creature in its place.
  * A `Greater Smoke` is defined as: 1/3 with `QuadrupleBones` ability.
- Removed abstract class `CustomAbilityBehavior` due to static reference issues where the only NewAbility that can be added to cards is the last one created.
- Changed list of cards in `Nest` ability that can be tutored for. New cards are: `Tail_Bird`, `Tail_Furry`,`Tail_Insect`,`Boulder`

## v1.1.0
- Updated to use API v1.12.1
- Corrected logic for checking for if PlayerSlots are full in `Split` ability.
- Created `Flight of the Valkyrie` ability.
  * When the creature is played, fill all open slots on your side of the field with 1/1 `Norse Warrior`s.
  * `Norse Warrior` is defined as 1/1 with `Brittle` ability.
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