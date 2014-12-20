# Road Layout Template

This template should allow anyone to create the layout for a road by simply dragging tiles onto a grid. Here are the steps to get it set up:

- Install Inkscape
	- If you are using Windows, install like normal
	- If you are using Mac, you'll need to first install XQuartz, an advanced replacement for OSX's default X11 environment
		- http://xquartz.macosforge.org/landing/
- Set up your preferences
	- Once Inkscape is started up, find "Inkscape Preferences" under the File menu.
	- The most important option is to use a geometric bounding box under "Tools"
- Copy the template
	- Before starting your own road layout, I recommend copying the template. This will make sure it doesn't get overwritten when you save.
	- Name the file "Road Name.svg"
- Learn about Inkscape controls
	- The section below includes some basic controls you'll need to know when using Inkscape
- Create your layout
	- Put the tiles together to make whatever layout you want
	- Save your file often
- Create a description file
	- In addition to the svg file, you should make a text file called "Road Name.txt"
	- This text file should include any important details about the road
		- Custom pieces needed
		- Important locations along the road (buildings, event sites, etc.)
		- Why you chose the name, and any inspirations for this particular road
- Submit your layout
	- Upload the svg and the txt file to google drive, under "Environment/Road Layouts"
	
# Key

- Dark grey
	- Pavement
- Medium grey
	- Sidewalk
- Light grey
	- Parking Lanes
- Green
	- Medians and Parks
- Blue
	- Water
- Yellow
	- Road Lines and Crosswalks
- Cyan Arrows
	- Slopes. Arrow points uphill.

# Inkscape Controls

## Grid
Grid settings are saved to the file. The layout comes with a grid with lines every 10 meters, with major lines every 4. 40x40 meters is the maximum size for the default tiles, anything larger would be a custom tile.

## Snapping
You'll want to make sure that the snapping settings are correct, or it will be a nightmare to move tiles around. Snap controls are on the right side of the window. You want to turn ON "Snap bounding box corners" and turn OFF "Snap nodes or handles".

## Selection
Click once on an object to select it and show the move/scale arrows. Clicking on the object again will toggle to move/rotate arrows. Drag an object to move it, and it will automatically snap to the grid. The point on the object it uses to snap is the point closest to where you clicked to drag.

## Duplication
Objects can be duplicated by selecting them and pressing Ctrl-D. You may not notice any difference, but if you then move the object you will see that you are moving a copy, and the original is still in place. You can also use copy-paste, and these controls will work the same for groups of objects.

## Tools
The toolbar is on the left side of the window. The most important tool is the "Select" tool, which is the arrow at the top. Also of note are the "Create Rectangles" and "Create Circles" tools, which you can use to indicate buildings, trees, garbage piles, manhole covers, and people. The text tool is tricky to use, and you'll want to make the font size very large to see it, but it can be used to add notes to the layout.

# Zoom
The zoom tool in the toolbar is something you'll use a lot. Click to zoom in, and Shift-Click to zoom out. Drag a box to zoom into a particular area.

## Fill and Stroke
When making rectangles and circles, you'll want to be able to change the fill and stroke settings. Press Shift-Ctrl-F to bring up the Fill and Stroke box, or press the button in the toolbar at the top. I recommend using primary colors, or colors that are easy to remember. That will make sure the layout is uniform and easy to read.

# Extra Information
These layouts can be as simple as you want, and this section includes any optional details you want to include. These are details regarding how the layout will be implemented in Unity.

## Zone Borders
You can use a red outline (Box with no fill, 2mm red stroke) to define the bounds of a zone.

## Zone Centers
A Red circle with a small radius will indicate where the center of a zone should be placed. This is the point the zone will rotate around when moved, and may effect calculations.

## Procedural Objects
Objects which are meant to be procedural can be indicated with an orange outline (full red, half green, no blue). These objects will be pulled from a pool of objects when the zone is loaded, and will be returned when unloaded. Most of the time, procedural objects are too small to be noted on the layout, and will be added as visual details when the road is created.