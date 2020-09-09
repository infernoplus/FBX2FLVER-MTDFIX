# FBX2FLVER
Experimental FBX model importer for Dark Souls 1.
Supports every single MTD in the base game. (Probably!***)
Made for importing MapPieces mainly. But could be used for other things probably.

Originally created by Meowartimus.
https://github.com/Meowmaritus/FBX2FLVER

## Broked Stuff
* Improperly exported FBX files will likely crash the tool with no warning.
* Most notably, if your MTD name is wrong it will just explode. Please be careful.
* Materials without textures would probably explode, I don't think there are any in DS1 though.

## Main Changes
* Will properly import meshes using any of the stock MTDs in DS1
* Will import vertex color and alpha
* Will import multiple UV channels (channel 2 is used for *[*][M] materials, and channel 3 is for lightmaps *[L])
* Will generate a seperate TPF for each material in your FBX, and with the proper naming convention for MapPieces (place them in map/tx/**)

## Texture Info
* Diffuse -> Diffuse
* Specular Color -> Specular
* Normal Bump -> Bump                        **(You must specifically use a 'Normal Bump' map and then attach your normal texture to that)*
* Self-Illumination/Emmisive -> Lightmap
* Transparency/Opacity -> EnvMap
* Specular Level/Specular Factor -> Diffuse2        **(Diffuse2, Specular2 and Bump2 are used by multi mats like M[DB][M] and are controlled by vertex color)
* Glossiness / Specular Power -> Specular2
* Reflection -> Bump2                               **(Just pipe the normal texture directly into the Reflection channel. Don't use a 'Normal Bump' map here.)*

If you are missing a texture the import will fail. Tested with 3DS Max 2015. Best of luck Blender boys~

## Some notes:
* For FLVER/TPF paths ***you can select a .CHRBND.DCX, .TEXBND.DCX, .BND.DCX etc and then select the file inside! WAY EASIER PLEASE USE THIS METHOD I'M BEGGING YOU DON'T EXTRACT THEN IMPORT TO LOOSE FILES AND REPACK I'M BEGGING YOU PLEASE SAVE YOURSELF FROM THIS NONSENSE AND SELECT A BND! A BND FILE!!!!!!***
* Material naming convention: `CustomMaterialName | IngameMTDName` e.g. `Blade | P_Metal[DSB]`
* All textures must be a proper DDS format the game supports otherwise the game will immediately crash upon trying to load the textures.

## Special Thanks
* **TKGP** for his incredible [SoulsFormats library](https://github.com/JKAnderson/SoulsFormats).
* **SiriusTexra** for helping with testing.
* **Dropoff** for helping with testing.
