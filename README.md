# Natural Procreation

Modifies the Iron Teeth faction to use natural procreation instead of breeding pods.

This was just a personal mod, I don't expect too many people will care for it, but I figured I might as well publish it in case anyone else out there wants this.

## Installation
Copy the NaturalProcreations folder into your BepInEx/plugins directory.

## What it does
Technically it modifies all factions, but only the Iron Teeth will be affected. What it does is:

Removes all Dwellings that have the BreedingPod component.
Any Dwellings that don't have the ProcreationHouse component, get one added.

This mod will probably affect balance. 

## Configuration

If you want, you can tweak the birth rate value used by the game on a per-faction basis. 

When you first run the mod, a config file will be created inside the mod folder. 
Edit the values in this file to adjust the birth rate. Lower numbers will result in reduced
chance for kits to be born.

## Things for later (maybe)
* Special building just for procreation, undo changes to barracks
* Maybe re-add the breeding pods, but as a mid or late-game tech building instead.
