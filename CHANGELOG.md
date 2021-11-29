# Changelog

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
  * A `Greater Smoke` is defined as: 1/3 with `QuadrupleBones` ability.
- Removed abstract class `CustomAbilityBehavior` due to static reference issues where the only NewAbility that can be added to cards is the last one created.
- - Changed list of cards in `Nest` ability that can be tutored for. New cards are: `Tail_Bird`, `Tail_Furry`,`Tail_Insect`,`Boulder`

## v1.1.0 
- Updated to use API v1.12.1
- Corrected logic for checking for if PlayerSlots are full in `Split` ability.
- Created `Flight of the Valkyrie` ability.
  * When the creature is played, fill all open slots on your side of the field with 1/1 `Norse Warrior`s.
  * `Norse Warrior` is defined as 1/1 with `Brittle` ability.
- Added Resource image loading so that users no longer have to copy/paste the artwork folder. Images are built with the .dll