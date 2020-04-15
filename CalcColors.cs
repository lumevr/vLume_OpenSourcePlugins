///
///CalcDensity for vLume 
///A. Spark, A. Kitching (16/01/2020)

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;


//Vector3's and Vector4's are classes provided by Unity:
//https://docs.unity3d.com/ScriptReference/Vector3.html
//https://docs.unity3d.com/ScriptReference/Vector4.html


//Do not modify this class. This is needed by the Lume application to run your code
public static class ExecuteProcess
{
    //Int array to hold the PointIndexes to return to the Lume applicaiton
    public static int[] Indexes;
    //Vector4 list to hold the colors to return to the Lume applicaiton
    public static List<Vector4> Colors;

    //do not modify this method. This is needed by the Lume application to run your code
    public static void Execute(List<Vector3> PosList)//'PosList' is the isolated array of positions as xyz values, as they appear in the dataset's CSV
    {
        //This is where you would call your custom function
        ColorData(); // a custom function
    }
    
    //do not modify this method. This is needed by the Lume appliction to return results
    public static void ReturnResults()
    {
        LumeRuntime.API.ColorIsolation(Indexes, Colors.ToArray()); // pass the indexes and colors as arrays to Lume application to apply colors to indexed data
        LumeRuntime.API.ReturnResults("Coloring " + Indexes.Length + " data points."); // return how many data indexes will be colored to script menu in Lume application
    }

    //'ColorData' is an example of a custom function. It iterates through the isolated data indexes and sets arbitrary colors
    static void ColorData()
    {
        Indexes = LumeRuntime.API.GetRawDataIndexes().ToArray(); // Get list of int's (data indexes) cast to an array
        Colors = new List<Vector4>(); // Initialize list


        for (int i = 0; i < Indexes.Length; i++) // Cycle through array and set colors for each index
        {
            if (LumeRuntime.API.ScriptExecuteCancelled()) // check the Lume Application if this script has been cancelled prematurely
            {
                return;
            }
            Colors.Add(Vector4.Lerp(new Vector4(1f, 0f, 0f, 1f), new Vector4(0f, 0f, 1f, 1f), (float)i / (float)Indexes.Length)); //lerp from red to blue over the whole array
            //Colors.Add(new Vector4(0f, 0f, 1f, 1f)); //color every point blue (r, g, b, a)
        }
    }
}


//Quick Lume application API refrence functions

//These function can be called during custom function executions:
    //LumeRuntime.API.GetPositionData(); //returns a list of vector3's. (The xyz positions as they appear in the data's CSV)
    //LumeRuntime.API.GetTransformedPositionData(); //returns a list of vector3's. (The xyz positions as they appear in the Lume application transformation space)
    //LumeRuntime.API.GetRawData(); //returns a list of strings. (Each point as its raw CSV value. For example one point could be: "0.1,0.2,0.3,55,High")
                                                                                                 //You will have to manually split up each string by its comma separators
    //LumeRuntime.API.GetRawDataIndexes(); // returns a list of ints. (Each int is a corresponding index to an isolated data point in Lume)
    //LumeRuntime.API.GetPositionDataRemaining(); // returns a list of vector3's (the non-isolated data). (The xyz positions as they appear in the data's CSV)
    //LumeRuntime.API.GetTransformedPositionDataRemaining(); // returns a list of vector3's (the non-isolated data). (The xyz positions as they appear in the Lume application transformation space)
    //LumeRuntime.API.GetRawDataRemaining(); // returns a list of strings (the non-isolated data).
    //LumeRuntime.API.GetRawDataIndexesRemaining(); // returns a list of ints (the non-isolated data).

    //LumeRuntime.API.UpdateProgress("..."); //Takes 1 string parameter. Visually Updates script menu in Lume application with given parameter as the progress value
    //LumeRuntime.API.ScriptCanStillExecute(); // Allows the script to break out of a loop early if the user stoppeds the executing script early

//These function can ONLY be called in the 'ReturnResults' function
    //LumeRuntime.API.ReturnResults("..."); //Takes 1 string parameter (return results to script menu)
    //LumeRuntime.API.ShowResult("..."); //Takes 1 string parameter (creates panel with results on it)
    //LumeRuntime.API.ColorIsolation({...}, {...}); // Takes 2 parameters, int[] array and a Vector4[] array (PointIndexes and colors) (Colors data that match the PointIndexes)
    //LumeRuntime.API.DeselectData(); // De-Isolates any data that has been isolated
    //LumeRuntime.API.OutputToFile({...}); // Takes 1 parameter, object[] array. Can parse in strings or numbers, and they will be output to a .txt file in 'Output' folder in the 'scripts' folder