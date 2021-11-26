# Changelog

## v1.2.0
- `Miasma`: When this creature dies, spawn a `Greater Smoke` creature in its place.
  * A `Greater Smoke` is defined as: 1/3 with `QuadrupleBones` ability.
- Removed abstract class `CustomAbilityBehavior` due to static reference issues where the only NewAbility that can be added to cards is the last one created. 

## v1.1.0 
- Updated to use API v1.12.1
- Corrected logic for checking for if PlayerSlots are full in `Split` ability.
- Created `Flight of the Valkyrie` ability.
  * When the creature is played, fill all open slots on your side of the field with 1/1 `Norse Warrior`s.
  * `Norse Warrior` is defined as 1/1 with `Brittle` ability.
- Added Resource image loading so that users no longer have to copy/paste the artwork folder. Images are built with the .dll