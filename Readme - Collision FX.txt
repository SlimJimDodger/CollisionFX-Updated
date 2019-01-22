-------------------------------------------------------------
 Collision FX
 Author:    pizzaoverhead
 Version:   4.0
 Released:  2017-03-09
 KSP:       v1.2.2

 Thread:    http://forum.kerbalspaceprogram.com/threads/101496
 Licence:   GNU v2, http://www.gnu.org/licenses/gpl-2.0.html
 Source:    https://github.com/pizzaoverhead/CollisionFX
-------------------------------------------------------------

Collision FX adds sound, light and particle effects when colliding or scraping your craft. Check the forum thread for updates:
http://forum.kerbalspaceprogram.com/threads/101496

This plugin makes use of ialdabaoth and Sarbian's Module Manager to avoid needing part.cfg edits. Get it here:
http://forum.kerbalspaceprogram.com/threads/55219


Installation
------------
Extract the zip to the root KSP folder, merging with the GameData folder.
Download and install the latest version of ModuleManager:
http://forum.kerbalspaceprogram.com/threads/55219


Uninstallation
--------------
Delete the CollisionFX folder inside GameData.


Configuration options
---------------------
volume - How loud the sound effects should be played. 1 is normal volume, 0.5 is half volume.
scrapeSparks - Whether the part should create sparks and a metallic scraping sound when sliding across the ground. Disable for wheels.
collisionSound - The sound file to play when the part bumps into something.
wheelImpactSound - The sound file to play when a wheeled part bumps into something.
scrapeSound - The sound file to play when the part scrapes against terrain. This sound should be loopable.
sparkSound - The sound file to play when the part scrapes against hard surfaces. This sound should be loopable.
sparkLightIntensity - How bright the light produced by sparks should be.
minScrapeSpeed - The minimum velocity at which scraping sounds are produced, in m/s.


Audio credits
-------------
GroundSkid.wav - Modified from "Rockslide.wav" by juskiddink, available here: https://www.freesound.org/people/juskiddink/sounds/77931/
SparkSqueal.wav - Modified from "md1trk4.AIF" by alienistcog, available here: https://www.freesound.org/people/alienistcog/sounds/123473/
TyreSqueal.wav - Modified from "Chrysler LHS tire squeal 01 (04-25-2009).wav" by audible-edge, available here: https://www.freesound.org/people/audible-edge/sounds/71736/
Bang1.ogg - Modified from "industrial_skipbin_hit_reverb_side" by gilly11, available here: https://www.freesound.org/people/gilly11/sounds/143597/
Oof.wav - Modified from "Male_Grunts.aiff" by sketchygio, available here: http://freesound.org/people/sketchygio/sounds/144907/
Squeak.wav - Modified from rubber duck big 2 by ermfilm, available here: http://freesound.org/people/ermfilm/sounds/130013/
RoadNoise.wav - Modified from "Tires car without an engine." by petruchio_ru, available here: https://www.freesound.org/people/petruchio_ru/sounds/188507/


Version history
---------------
4.0 (2017-03-09)
- Rebuilt for KSP 1.2.2.

3.3 (2015-10-27)
- Added biome-specific dust effects.
- Fixed bug with light intensity being much higher than intended.
- Lowered light intensity and made it adjustable.
- Added a new sound.

3.2 (2015-05-14)
- Wheels now have dust effects.

3.1 (2015-05-13)
- Fixed spark sounds never stopping.

3.0.2 (2015-05-07)
- Support for Adjustable Landing Gear v1.1.0.
- Support for Kerbal Foundries v1.8G.
-Updated to Module Manager 2.6.3.

3.0.1 (2015-05-02)
- Fixed wheels causing sparks.

3.0 (2015-04-30)
- Rebuilt for KSP 1.00
- Fixed EVA kerbals causing sparks.
- Added collision sound for EVA kerbals.
- Updated to Module Manager 2.6.2

2.2 (2015-01-14)
- Added different effects for different biomes around Kerbin.
- Now includes Module Manager.

2.1 (2014-12-18)
- Rebuilt for KSP 0.90.
- Modified collision detection.
- Added Gaalidas' fix for Kerbal Foundries parts.

2.0 (2014-12-08)
- Fixed wheel, repulsor and track parts from Modular Multiwheels and Kerbal Foundries showing sparks.
- Improved collision detection.
- Fixed stuck lights and sounds.

1.2 (2014-12-02)
- Disabled effects for EVA kerbals and Kerbal Foundries wheels and tracks.

1.1 (2014-11-29)
- Fixed issues with wheels producing sparks in some cases.
- Disabled effects for Firespitter wheels as they have their own effects.

1.0 (2014-11-29)
- Initial release.