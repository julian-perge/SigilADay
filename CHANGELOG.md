# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

### [1.21.2](https://github.com/julian-perge/SigilADay/compare/v1.21.1...v1.21.2) (2021-12-25)


### Refactors

* Removed API version in .csproj. Updated API to use 1.13 ([b2c7d8c](https://github.com/julian-perge/SigilADay/commits/b2c7d8cc8658c826a99dec1e1455a5a0e04446d8))

## [1.21.1](https://github.com/julian-perge/SigilADay/compare/v1.21.0...v1.21.1) (2021-12-25)


### Bug Fixes

* Moved tools back to main directory to avoid all the issues that keep coming up with with relative and parent dirs. ([15337e4](https://github.com/julian-perge/SigilADay/commits/15337e4c25fb1ab15adbdfc24e60ac9d65729423))
* Refactored checking if card exists in card pool to now ONLY check ScriptableObjectLoader<CardInfo>.allData ([155e572](https://github.com/julian-perge/SigilADay/commits/155e572887009dc01dd882d91bb48b448f51c64b))
* Removed `cd ./tools` call in scripts ([4f7925f](https://github.com/julian-perge/SigilADay/commits/4f7925fa0fd955892546c3ba67d53e1f0008061a))

## [1.20.0](https://github.com/julian-perge/SigilADay/compare/v1.19.0...v1.20.0) (2021-12-11)


### Bug Fixes

* Corrected dir to check for CHANGELOG ([c5f8ecd](https://github.com/julian-perge/SigilADay/commits/c5f8ecd3fe39ae78c5e5cd1b5cce034e9a264522))
* Updated package.json to cd into tools to run commands. ([dabe53c](https://github.com/julian-perge/SigilADay/commits/dabe53c0e3156256d108735441e96f448e1aa9b9))
* Updated paths for versioning ([bc231b2](https://github.com/julian-perge/SigilADay/commits/bc231b22437d31becd55e2d9242b987799a2c7c4))


## [1.11.0](https://github.com/julian-perge/SigilADay/compare/v1.10.0...v1.11.0) (2021-12-11)


### Bug Fixes

* Added back older Brimstone logic as Leshy was not getting damaged correctly. ([240208d](https://github.com/julian-perge/SigilADay/commits/240208de02bc68dd359fa5b5ff177bca38a2e359))

## [1.10.0](https://github.com/julian-perge/SigilADay/compare/v1.9.18...v1.10.0) (2021-12-09)


### Bug Fixes

* Made InitializeConfigNest async and wait for all cards to be loaded. ([ecada89](https://github.com/julian-perge/SigilADay/commits/ecada89bbcdb21947fcf9fdb81844242e38a33d8))
* Made initializing nest config go first over Evolve Randomly. ([df942a8](https://github.com/julian-perge/SigilADay/commits/df942a8190b520dd1546cb0b8a25a47bab465cd5))


### Docs

* added extra sed line in fix-header-levels-in-changelog.sh to replace hyphens at the start of the line with asterisks. ([60ec052](https://github.com/julian-perge/SigilADay/commits/60ec0520f2335aca7ea943bf0613d8cc5334dcdf))
* updated comments and removed checking if the slot is null in Split ability loop. ([804c13b](https://github.com/julian-perge/SigilADay/commits/804c13be6d7fd40570cbcbc4d0be42c0ceeb50e8))


### Refactors

* Made WaitForAllCardsToBeLoaded public so that it could be used in Nest config class. ([c5963ce](https://github.com/julian-perge/SigilADay/commits/c5963ce992a37e345e46d4c596e0a9e976185170))

## [1.9.18](https://github.com/julian-perge/SigilADay/compare/v1.9.17...v1.9.18) (2021-12-07)

### Bug Fixes

* Added script for fixing title level issue. Cleanedup changelog. ([addab65](https://github.com/julian-perge/SigilADay/commits/addab65b74370e8686c4772de260858ceb788fa0))

## [1.9.13](https://github.com/julian-perge/SigilADay/compare/v1.9.12...v1.9.13) (2021-12-07)

### Docs

* Added MD lint file for better styling ([877beff](https://github.com/julian-perge/SigilADay/commits/877beff8f7b396022860115cccaec89648f5aba8))
* Fixed styling of CHANGELOG.md, corrected version in package.json ([9bec898](https://github.com/julian-perge/SigilADay/commits/9bec89830c8622a749434ff1c45d99a3c5dc4c55))

## [1.9.2](https://github.com/julian-perge/SigilADay/compare/v1.9.1...v1.9.2) (2021-12-07)

### Features

* Added custom version controler for Plugin.cs ([29f721c](https://github.com/julian-perge/SigilADay/commits/29f721c5b1cfb7cc3211ab1736418ed7bbeafdd5))

### Bug Fixes

* fixed finding verison number for Plugin.cs ([c9cc949](https://github.com/julian-perge/SigilADay/commits/c9cc9495924d3d54ff4461aa47853a03270498fd))
* fixed package.json version number ([fb793f4](https://github.com/julian-perge/SigilADay/commits/fb793f4b326c25d16be9cf476f7610f475848858))

## [1.9.1](https://github.com/julian-perge/SigilADay/compare/v1.9.0...v1.9.1) (2021-12-07)

### Features

* added custom version_number writer ([16ff52d](https://github.com/julian-perge/SigilADay/commits/16ff52d076270c6197b9c8ab4d797261ac20635b))

## [1.9.0] (2021-12-07)

### Feature

* Added `husky` and `standard-version` npm packages for easier creation and updating of version number.

## [1.8.3]

### Feature

* Added `Hermit` ability icon.

### Refactors

* Removed braces on `Norse Warrior` name. From `[SigilADay_julianperge]_NorseWarrior` to `SigilADay_julianperge_NorseWarrior`
* Added [define:] in Flight of the Valkyrie rulebook description for `Norse Warrior`.
* Fixed `Split` ability description.
* Removed `SlotTargetedForAttack` methods in Brimstone as that was refactored into the OtherCardDie methods.
* Made `CreateAbility` a generic so that the class name could be passed in.
* Updated `EvolveRandomlyConfig` logging to use Plugin.Log.
* Refactored `Hermit` ability to now be `AbilityBehaviour` instead of `SpecialCardBehaviour`.
* Added another check after cards have been loaded to also check the main card pool.
* Made every string a const if I could.

## [1.8.2]

### Bug Fixes

* Corrected static name in `Brimstone` from `_Ability` to `ability`.

## [1.8.1]

### Bug Fixes

* Fixed logic for Brimstone. Forgot to remove an extra check.

## [1.8.0]

### Sigils

* Migrated `Exodia` ability from main repo.
* Added `Left Arm of Exodia` and `Right Arm of Exodia` cards to card pool. Only available through Trader.

### Refactors

* Replaced Brimstone ability icon with a cleaner one.
* Replaced Belligerent ability icon with one provided by Puppetsquid#6087.
* Added more utility methods in SigilUtils for creating abilities.
  - Refactored ablities to use these new methods.
* Changed Brimstone to always have overkill damage, regardless if you kill the opposing card.
  - Updated Brimstone description.

## [1.7.1]

### Refactors

* Updated readme for older ability descriptions to match what they currently do.

## [1.7.0]

### Features

* Created `Hermit` ability (WIP).
  - Similar to Submerge ability, but instead can only be played on Terrain/Pelt cards and hides under them.
* Created `Brimstone` ability.
  - Idea courtesy of Skkull#0393 on discord.
* Added dependency to JSONLoader since this mod is primarily used with that mod.
* Added config for Nest cards!

### Bug Fixes

* Implemented async calls so now EvolveRandomly and Nest configs won't load until the custom cards and abilities have been loaded.

### Refactors

* Added more utility methods in SigilUtils.
* Moved dialogue speech for Nest ability before tutor sequence.

## [1.6.4]

### Features

* Added .cfg style loading for `Nest` ability.

## [1.6.3]

### Features

* Updated EvolveRandomly to now modify multiple sets of cards for random evolution.

### Bug Fixes

* Fixed plugin version being 1.5.0 for some reason...

## [1.6.2]

### Sigils

* Created `Randomly Evolve` ability.

### Bug Fixes

* Disabled `Belligerent` for the time being due to lack of testing (sorry!)
* Set `TargetFramework` in `.csproj` back to `netstandard2.0` from `netstandard2.1` as 2.1 was causing errors when trying to use `string.Split()`

## [1.5.0]

### Sigils

* Created `Belligerent` ability.

## [1.4.1]

### Bug Fixes

* Fixed `Cannibal` property getter for the ability to now correctly return the static ability field.

## [1.4.0]

### Sigils

* Created `Prospect` ability.
* Created `Golden Nugget` ability.

### Features

* Added static imports for all the classes.

### Bug Fixes

* Fixed namespace for SigilUtils.

### Refactors

* Updated `CreateInfoWithDefaultSettings` method to now include setting the dialogue option.
* Updated descriptions of each ability.

## [1.3.0]

### Sigils

* Created `Cannibal` ability.

## [1.2.0]

### Sigils

* Created `Miasma` ability.

### Features

* Changed list of cards in `Nest` ability that can be tutored for. New cards are: `Tail_Bird`, `Tail_Furry`,`Tail_Insect`,`Boulder`

### Bug Fixes

* Removed abstract class `CustomAbilityBehavior` due to static reference issues where the only NewAbility that can be added to cards is the last one created.

## [1.1.0]

### Sigils

* Created `Flight of the Valkyrie` ability.

### Features

* Updated to use API v1.12.1
* Added Resource image loading so that users no longer have to copy/paste the artwork folder. Images are built with the .dll

### Bug Fixes

* Corrected logic for checking for if PlayerSlots are full in `Split` ability.
