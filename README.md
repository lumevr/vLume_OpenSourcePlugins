## *v*LUME Scripts

An important feature of *v*LUME the ability to perform local analysis on a subregion of point-cloud data (RoI). C#
scripts can be programmed to perform any custom analysis once uploaded to the folder
\vLume_Data\StreamingAssets\Scripts within the *v*LUME installation directory (.cs file extension). You can find
further information on how program and use the scripts in the software manual.
We include four useful functions as .cs scripts for the *v*LUME interpreter that can be used for local cloud data-point
analysis and that are often used in SMLM. You can apply them by selecting a RoI and toggling to the scripting menu
(see manual). Note: depending on the complexity of the script you might see a busy indicator (out of the virtual
environment) when called which is normal. Also be aware that some operations require more than the available memory
particularly in large datasets (i.e. density plots with millions of points that need to compute every single distance from
one point to another).
We include four scripts however it is our hope that we will nucleate communities to develop and share their own,
included are:

## Ripley’s K function

Ripley’s K function (filename: CalcRipleysK.cs). is a spatial point statistics analysis commonly used to
evaluate clustering in SMLM data. The script computes the Euclidian distance from a series of pairwise
points in a RoI and counts the number of neighbours from a single point to a moving 3D radius (defined by
the user and prompted as RegionSize and RegionStep). The input units need to be the same as the dataset. The output of the script is the L function, the linearized and localized Ripley’s K function11, for every RegionStep from 0 to RegionSize. The script also outputs a .txt file (named after the type of analysis and the time it was performed) in StreamingAssets\Scripts\Output saving the number of points in the RoI, the volume, region size and the step value. Then the computed, L(radius), and finally the position of every single point within the RoI. Note: The function is only taken within the RoI as an isolated region and does not take into account any points
in the periphery. Be aware that every single distance from point to point is computed, therefore large RoIs
should be executed only in systems with large amounts of memory. The volume is computed as the smallest
bounding box containing the whole dataset, which may be a suitable approximation for irregular volumes.

## Nearest Neighbour

Nearest Neighbour Plot (filename: CalcNearestNeighbour.cs). A widely used analysis tool in SR. The
user is prompted for the value of the ThresholdRadius (that has to be inputted in the same units as the dataset)
which will be used to compute the number of neighbours of every single point in the RoI up to the
ThresholdRadius. The script then assigns a false colour depending on that number. These numbers are
normalized with reference to the maximum number of neighbours, red being the lower density and blue the
highest within a colour gradient (note: the colours will be plotted on top of the selection). The script also
outputs a .txt file (named after the type of analysis and the time it was performed) in
StreamingAssets\Scripts\Output, saving the number of points of the RoI, the radius tested, and the number of
neighbours of every single point within the RoI together with its position. Note: The function is only taken within the RoI as an isolated region and does not take into account any pointsin the periphery. Be aware that every single distance from point to point is computed therefore, large RoIs should be executed only in systems with large amounts of memory.

## Calculate the Density of Points in RoI

Calculate the Density of Points in RoI (filename: CalcDensity.cs). We provide a very simple
implementation that calculates the points within the RoI and divides this value by the volume of RoI to create
the localization density. The result is printed within vLUME as points/volume using the units of the .csv
dataset. Note: The volume is computed as the smallest bounded box that can contain the selected dataset, this is only
an approximation for irregular volumes.

## Calculate the Maximum and the Minimum Distances between Points in RoI

Calculate the Maximum and the Minimum Distances between points in RoI (filename:
CalcShortAndFarDistances.cs). For syntax purposes, we provide a very simple script that calculates the
maximum and minimum distance between all the points within the RoI. The result is printed within vLUME
using the units of the .csv dataset. Note: Be aware that every single distance from point to point is computed therefore, large RoIs should be executed only in systems with large amounts of memory.

## Video Sample

As a demonstration of the capabilities of *v*LUME we show an example video corresponding to the key benefits using scripts for cutting edge SMLM samples. This and other supplementary videos can be found in the BioRXiv preprint repository.

|                |Link                          |Description                         |
|----------------|---------------------------------|-----------------------------|
|Local bespoke analysis          |[Video 1](https://www.biorxiv.org/content/biorxiv/early/2020/01/21/2020.01.20.912733/DC6/embed/media-6.zip?download=true) |Nearest Neighbour script application to a NPC dataset|
