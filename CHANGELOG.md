# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

## [1.9.13](https://github.com/julian-perge/SigilADay/compare/v1.9.12...v1.9.13) (2021-12-07)

### Docs

- Added MD lint file for better styling ([877beff](https://github.com/julian-perge/SigilADay/commits/877beff8f7b396022860115cccaec89648f5aba8))
- Fixed styling of CHANGELOG.md, corrected version in package.json ([9bec898](https://github.com/julian-perge/SigilADay/commits/9bec89830c8622a749434ff1c45d99a3c5dc4c55))

## [1.9.2](https://github.com/julian-perge/SigilADay/compare/v1.9.1...v1.9.2) (2021-12-07)

### Features

- Added custom version controler for Plugin.cs ([29f721c](https://github.com/julian-perge/SigilADay/commits/29f721c5b1cfb7cc3211ab1736418ed7bbeafdd5))

### Bug Fixes

- fixed finding verison number for Plugin.cs ([c9cc949](https://github.com/julian-perge/SigilADay/commits/c9cc9495924d3d54ff4461aa47853a03270498fd))
- fixed package.json version number ([fb793f4](https://github.com/julian-perge/SigilADay/commits/fb793f4b326c25d16be9cf476f7610f475848858))

## [1.9.1](https://github.com/julian-perge/SigilADay/compare/v1.9.0...v1.9.1) (2021-12-07)

### Features

- added custom version_number writer ([16ff52d](https://github.com/julian-perge/SigilADay/commits/16ff52d076270c6197b9c8ab4d797261ac20635b))

## [1.9.0] (2021-12-07)

### Feature

- Added `husky` and `standard-version` npm packages for easier creation and updating of version number.

## [1.8.3]

### Feature

- Added `Hermit` ability icon.

### Refactors

- Removed braces on `Norse Warrior` name. From `[SigilADay_julianperge]_NorseWarrior` to `SigilADay_julianperge_NorseWarrior`
- Added [define:] in Flight of the Valkyrie rulebook description for `Norse Warrior`.
- Fixed `Split` ability description.
- Removed `SlotTargetedForAttack` methods in Brimstone as that was refactored into the OtherCardDie methods.
- Made `CreateAbility` a generic so that the class name could be passed in.
- Updated `EvolveRandomlyConfig` logging to use Plugin.Log.
- Refactored `Hermit` ability to now be `AbilityBehaviour` instead of `SpecialCardBehaviour`.
- Added another check after cards have been loaded to also check the main card pool.
- Made every string a const if I could.

## [1.8.2]

### Bug Fixes

- Corrected static name in `Brimstone` from `_Ability` to `ability`.

## [1.8.1]

### Bug Fixes

- Fixed logic for Brimstone. Forgot to remove an extra check.

## [1.8.0]

### Sigils

- Migrated `Exodia` ability from main repo.
- Added `Left Arm of Exodia` and `Right Arm of Exodia` cards to card pool. Only available through Trader.

### Refactors

- Replaced Brimstone ability icon with a cleaner one.
- Replaced Belligerent ability icon with one provided by Puppetsquid#6087.
- Added more utility methods in SigilUtils for creating abilities.
  - Refactored ablities to use these new methods.
- Changed Brimstone to always have overkill damage, regardless if you kill the opposing card.
  - Updated Brimstone description.

## [1.7.1]

### Refactors

- Updated readme for older ability descriptions to match what they currently do.

## [1.7.0]

### Features

- Created `Hermit` ability (WIP).
  - Similar to Submerge ability, but instead can only be played on Terrain/Pelt cards and hides under them.
- Created `Brimstone` ability.
  - Idea courtesy of Skkull#0393 on discord.
- Added dependency to JSONLoader since this mod is primarily used with that mod.
- Added config for Nest cards!

### Bug Fixes

- Implemented async calls so now EvolveRandomly and Nest configs won't load until the custom cards and abilities have been loaded.

### Refactors

- Added more utility methods in SigilUtils.
- Moved dialogue speech for Nest ability before tutor sequence.

## [1.6.4]

### Features

- Added .cfg style loading for `Nest` ability.

## [1.6.3]

### Features

- Updated EvolveRandomly to now modify multiple sets of cards for random evolution.

### Bug Fixes

- Fixed plugin version being 1.5.0 for some reason...

## [1.6.2]

### Sigils

- Created `Randomly Evolve` ability.

### Bug Fixes

- Disabled `Belligerent` for the time being due to lack of testing (sorry!)
- Set `TargetFramework` in `.csproj` back to `netstandard2.0` from `netstandard2.1` as 2.1 was causing errors when trying to use `string.Split()`

## [1.5.0]

### Sigils

- Created `Belligerent` ability.

## [1.4.1]

### Bug Fixes

- Fixed `Cannibal` property getter for the ability to now correctly return the static ability field.

## [1.4.0]

### Sigils

- Created `Prospect` ability.
- Created `Golden Nugget` ability.

### Features

- Added static imports for all the classes.

### Bug Fixes

- Fixed namespace for SigilUtils.

### Refactors

- Updated `CreateInfoWithDefaultSettings` method to now include setting the dialogue option.
- Updated descriptions of each ability.

## [1.3.0]

### Sigils

- Created `Cannibal` ability.

## [1.2.0]

### Sigils

- Created `Miasma` ability.

### Features

- Changed list of cards in `Nest` ability that can be tutored for. New cards are: `Tail_Bird`, `Tail_Furry`,`Tail_Insect`,`Boulder`

### Bug Fixes

- Removed abstract class `CustomAbilityBehavior` due to static reference issues where the only NewAbility that can be added to cards is the last one created.

## [1.1.0]

### Sigils

- Created `Flight of the Valkyrie` ability.

### Features

- Updated to use API v1.12.1
- Added Resource image loading so that users no longer have to copy/paste the artwork folder. Images are built with the .dll

### Bug Fixes

- Corrected logic for checking for if PlayerSlots are full in `Split` ability.
