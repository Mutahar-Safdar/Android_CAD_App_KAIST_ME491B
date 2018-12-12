*****This Project is a part of Feature-based modeling class offered by Prof. Han of KAIST*****                   [https://sites.google.com/view/kaist-2018f-me491b/home]
                           
Code Explanation:                      
___________________________________________________________________________________________________________________________________                          
                           A simple feature based 3D CAD modeler that can run on your smartphone.
                                                 ===============================
New objects (sketch, rectangle on sketch, plane) are made by duplicating a prefab (ex: planePrefab) that contains the components 
(ex : planeProperties script) we want for an each object of that type. (ex: plane). This is better than creating a new gameobject 
from scratch and then giving it the scripts every time.

All planes have a selected property set to true if the plane is selected. Many functions of the program search for this selected plane 
to get its position, normal… for the camera movements, sketch position, extrusion direction etc…

Menus move vertically to go in and off screen. They move to set positions set in the script planeProperties. This script contains 
multiple functions that are called by buttons in the UI. The scripts a button calls in set in the button properties in the editor.

For coloring the face of a geometry we have selected, we use a custom shader that allows use to paint the triangles of an object with 
different colors. This is not possible with Unity default shaders.

For the extrusion process, multiple scripts are used. First a script takes the points of the sketch shape and triangulates it 
(Triangulator), then another one takes that triangulated shape and creates the extrusion (MeshExtrusion). Theses scripts where provided
to us. In the script extrude we use them for extrude and cut extrude functions. The cut extrude functions also relies on other scripts 
from the CSG folder found on the web.

                                                 
Implementation Environment:
_______________________________________________________________________________________________________________________________________

Computer Operating System: Windows 10 64 bit

Development Engine: Unity 3D Version 2018.2.2f1

Integrated Development Environment: Visual Studio 2017

Programming Language: C#


Contact:
_______________________________________________________________________________________________________________________________________

Mutahar Safdar (mutaharsafdar@kaist.ac.kr)

Alexandre Chopin (alexandre59147@gmail.com)

                                                 ===========================
