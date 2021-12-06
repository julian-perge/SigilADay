# Changelog

## v1.8.0

- Migrated `Exodia` ability from main repo.
- Added `Left Arm of Exodia` and `Right Arm of Exodia` cards to card pool. Only available through Trader.
- Replaced Brimstone ability icon with a cleaner one.
- Replaced Belligerent ability icon with one provided by Puppetsquid#6087.
- Added more utillity methods in SigilUtils for creating abilities. Refactored ablities to use these new methods.
- Changed Brimstone to always have overkill damage, regardless if you kill the opposing card. Updated Brimstone description.

## v1.7.1

- Updated readme for older ability descriptions to match what they currently do.

## v1.7.0

- Created `Hermit` ability (WIP).
  - Similar to Submerge ability, but instead can only be played on Terrain/Pelt cards and hides under them.
- Created `Brimstone` ability.
  - Idea courtesy of Skkull#0393 on discord.
- Added more utility methods in SigilUtils.
- Added dependency to JSONLoader since this mod is primarily used with that mod.
- Added config for Nest cards!
- Moved dialogue speech for Nest ability before tutor sequence.
- Implemented async calls so now EvolveRandomly and Nest configs won't load until the custom cards and abilities have been loaded.

## v1.6.4

- Cleaned up README.md
- Added .cfg style loading for `Nest` ability.

## v1.6.3

- Fixed plugin version being 1.5.0 for some reason...
- Updated EvolveRandomly to now modify multiple sets of cards for random evolution.

## v1.6.2

- Created `Randomly Evolve` - A card bearing this sigil will grow into a random form after 1 turn on the board.
- Disabled `Belligerent` for the time being due to lack of testing (sorry!)
- Set `TargetFramework` in `.csproj` back to `netstandard2.0` from `netstandard2.1` as 2.1 was causing errors when trying to use `string.Split()`

## v1.5.0

- `Belligerent` - When [creature] dies, create a copy of itself with a cumulative -1 to Power and Health. Has no effect on cards with 1 Health

## v1.4.1

- Fixed `Cannibal` property getter for the ability to now correctly return the static ability field.

## v1.4.0

- `Prospect` - When this attacks another creature, that creature turns into a Golden Nugget. 1 gold tooth is found upon destroying the Golden Nugget.
- `Golden Nugget`: When this creature perishes, gain 1 gold tooth.
- Added static imports for all the classes.
- Updated descriptions of each ability.
- Fixed namespace for SigilUtils.
- Updated `CreateInfoWithDefaultSettings` method to now include setting the dialogue option.

## v1.3.0

- `Cannibal` - At the end of your turn, this creature will steal 1 health from adjacent creatures of the same tribe.

## v1.2.0

- `Miasma` - When this creature dies, spawn a `Greater Smoke` creature in its place.
  - A `Greater Smoke` is defined as: 1/3 with `QuadrupleBones` ability.
- Removed abstract class `CustomAbilityBehavior` due to static reference issues where the only NewAbility that can be added to cards is the last one created.
  - Changed list of cards in `Nest` ability that can be tutored for. New cards are: `Tail_Bird`, `Tail_Furry`,`Tail_Insect`,`Boulder`

## v1.1.0

- Updated to use API v1.12.1
- Corrected logic for checking for if PlayerSlots are full in `Split` ability.
- Created `Flight of the Valkyrie` ability.
  - When the creature is played, fill all open slots on your side of the field with 1/1 `Norse Warrior`s.
  - `Norse Warrior` is defined as 1/1 with `Brittle` ability.
- Added Resource image loading so that users no longer have to copy/paste the artwork folder. Images are built with the .dll
