# FBX2FLVER
Experimental FBX model importer for Dark Souls 1.
Supports every single MTD in the base game. (Probably!***)

Originally created by Meowartimus.
https://github.com/Meowmaritus/FBX2FLVER

## Broked Stuff
Improperly exported FBX files will likely crash the tool with no warning.
Most notably, if your MTD name is wrong it will just explode. Please be careful.

## Texture Info
* Diffuse -> Diffuse
* Specular Color -> Specular
* Normal Bump -> Bump                        **(You must specifically use a 'Normal Bump' map and then attach your normal texture to that)*
* Self-Illumination/Emmisive -> Lightmap

Tested with 3DS Max 2015. Best of luck Blender boys~

## Some notes:
* For FLVER/TPF paths ***you can select a .CHRBND.DCX, .TEXBND.DCX, .BND.DCX etc and then select the file inside! WAY EASIER PLEASE USE THIS METHOD I'M BEGGING YOU DON'T EXTRACT THEN IMPORT TO LOOSE FILES AND REPACK I'M BEGGING YOU PLEASE SAVE YOURSELF FROM THIS NONSENSE AND SELECT A BND! A BND FILE!!!!!!***
* Material naming convention: `CustomMaterialName | IngameMTDName` e.g. `Blade | P_Metal[DSB]`
* All textures must be a proper DDS format the game supports otherwise the game will immediately crash upon trying to load the textures.

## Special Thanks
* **TKGP** for his incredible [SoulsFormats library](https://github.com/JKAnderson/SoulsFormats).
* **SiriusTexra** for helping with testing.
* **Dropoff** for helping with testing.
