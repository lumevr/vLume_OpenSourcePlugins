///
///CalcDensity for vLume 
///A. Spark, A. Kitching (16/01/2020)

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;


//Vector3's are a class provided by Unity:
//https://docs.unity3d.com/ScriptReference/Vector3.html


//Do not modify this class. This is needed by the Lume application to run your code
public static class ExecuteProcess
{
    //String to hold our custom function results to return to the Lume applicaiton
    public static string Result;

    //do not modify this method. This is needed by the Lume application to run your code
    public static void Execute(List<Vector3> PosList)//'PosList' is the isolated array of positions as xyz values, as they appear in the dataset's CSV
    {
        //This is where you would call your custom function
        FurthestAndShortestDistances(PosList); // a custom function
    }

    //do not modify this method. This is needed by the Lume appliction to return results
    public static void ReturnResults()
    {
        LumeRuntime.API.ReturnResults(Result); //return 'Result' to script menu in Lume application
        LumeRuntime.API.ShowResult(Result); //creates a panel with 'Result' value on it in Lume application
    }

    //'FurthestAndShortestDistances' is an example of a custom function. It finds the shortest and farthest distance values between the isolated data points
    static void FurthestAndShortestDistances(List<Vector3> PosList)
    {
        float max = -1;
        float min = float.MaxValue;

        float Progress = 0;

        //nested loop to iterate through poslist and test each point against every other point
        for (int i = 0; i < PosList.Count - 1; i++)
        {
            if (LumeRuntime.API.ScriptExecuteCancelled()) // check the Lume Application if this script has been cancelled prematurely
            {
                return;
            }
            for (int j = i + 1; j < PosList.Count; j++)
            {
                // Calculate the distance and set the max if highest
                if (Vector3.Distance(PosList[i], PosList[j]) > max)
                {
                    max = Vector3.Distance(PosList[i], PosList[j]);
                }
                if (Vector3.Distance(PosList[i], PosList[j]) < min)
                {
                    min = Vector3.Distance(PosList[i], PosList[j]);
                }
            }
            Progress = (float)i / (float)PosList.Count;
            LumeRuntime.API.UpdateProgress("\nItems left: " + (PosList.Count - i) + "\nProgress: " + (Progress * 100f).ToString("F2") + "%"); // Allows progress of script to be seen in Lume
        }

        Result = "\nBiggest distance: " + max + "\nSmallest distance: " + min;
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