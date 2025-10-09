# IluAmbarMapViewer

## Map generation
### Dependencies
- [Vsmaptools](https://mods.vintagestory.at/vsmaptools) - standalone app(python) to extract a map from a vs generated world
- [Python](https://www.python.org/)
  - [Pillow](https://pypi.org/project/pillow/) - Python library for image editing
  - [Python Scripts/GoogleMapsTileCutter.py](https://github.com/Soulfarmer/IluAmbarMapViewer/blob/master/Python%20Scripts/GoogleMapsTileCutter.py)) - Script that crops the map into zoom levels, rows and columns in the format ``z/x/y.png``
- [imagemagick](https://imagemagick.org/) - cmd image edititing app
- 
### Steps
Locate the world save with the command ``winKey+R`` with ``%APPDATA%/VintageStoryData/Maps``.

Copy the file into the vsmaptools folder - i have the executable version with a config file that i edit accordingly:
```json
{
  "map_file": "<your-save>.db",
  "output": "vs_worldmap01.png",
  "whole_map": true,
  "min_x": -6000,
  "max_x": 6000,
  "min_z": -6000,
  "max_z": 6000,
  "use_relative_coord": true,
  "spawn_abs_x": 0,
  "spawn_abs_z": 0
}

``` 

Zoom the image slightly with Image Magick to have an extra zoom level:
cmd: `` magick vs_worldmap01.png -resize 110% out.png``

With the output image from imagemagick run the script:  
cmd: ``python GoogleMapsTileCutter.py "./out.png" --output_dir "./map/"``  

Grab the contents of ``./map/`` (it should be 6 folders - each corresponding to a zoom level) and put them in the ``mapviewer/public/map/``.

# RedsChatCommands