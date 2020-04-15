///
///CalcNearestNeighbour for vLume 
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
    //If a public field has the prefix 'PROMPT_' at the start of it, the user will be asked to dynamically input that field in Lume before the 'Execute' function is called.
    public static float PROMPT_ThresholdRadius = 2f;

    //String to hold our custom function results to return to the Lume applicaiton
    public static string Result;

    //String to hold our custom function results to return to the Lume applicaiton
    public static string ReturnMessage;

    //Int array to hold the PointIndexes to return to the Lume applicaiton
    public static int[] Indexes;
    //Vector4 list to hold the colors to return to the Lume applicaiton
    public static List<Vector4> Colors;

    //do not modify this method. This is needed by the Lume application to run your code
    public static void Execute(List<Vector3> PosList)//'PosList' is the isolated array of positions as xyz values, as they appear in the dataset's CSV
    {
        //This is where you would call your custom function
        NearestNeighbours(PosList, PROMPT_ThresholdRadius); // a custom function
    }

    //do not modify this method. This is needed by the Lume appliction to return results
    public static void ReturnResults()
    {
        LumeRuntime.API.ColorIsolation(Indexes, Colors.ToArray()); // pass the indexes and colors as arrays to Lume application to apply colors to indexed data
        LumeRuntime.API.ReturnResults(ReturnMessage); //return 'ReturnMessage' to script menu in Lume application
        LumeRuntime.API.OutputToFile(new object[] { Result }); //writes the values of result and positions to a text file
        LumeRuntime.API.DeselectData(); //Deselects isolated data in the Lume Application to show the false coloring
    }

    //'NearestNeighbours' is an example of a custom function. It finds the maximum number of neighbours for every single data point within a given radius
    static void NearestNeighbours(List<Vector3> PosList, float TargetRadius)
    {
        Indexes = LumeRuntime.API.GetRawDataIndexes().ToArray(); // Get list of int's (data indexes) cast to an array
        Colors = new List<Vector4>(); // Initialize list

        string[] Positions = new string[PosList.Count];

        List<float> res = new List<float>();  //list of pairwise distances of localizations

        float TotalProgress = 0;

        int[] k = new int[PosList.Count];

        int booleansum = 0;

        for (int i = 0; i < PosList.Count; i++)
        {
            if (LumeRuntime.API.ScriptExecuteCancelled()) // check the Lume Application if this script has been cancelled prematurely
            {
                return;
            }
            TotalProgress = (float)i / (float)PosList.Count;
            LumeRuntime.API.UpdateProgress("Calculating Distances: " + (TotalProgress * 100f).ToString("F2") + "%"); // Allows progress of script to be seen in Lume
            booleansum = 0;
            for (int j = 0; j < PosList.Count; j++)
            {
                if (Vector3.Distance(PosList[i], PosList[j]) < TargetRadius)
                {
                    booleansum++;
                }
            }
            k[i] = booleansum-1;
            Positions[i] = PosList[i].x.ToString() + "," + PosList[i].y.ToString() + "," + PosList[i].z.ToString();
        }

        int MostNeighbours = k.Max();

        ReturnMessage = "Checked " + PosList.Count + " points for nearest neighbours within a radius of " + PROMPT_ThresholdRadius + "\nRed represents a low neighbour count and blue represents a high neighbour count.";

        Result = "Number of Nearest Neighbours for " + PosList.Count + " points within a radius of " + PROMPT_ThresholdRadius + "\n";

        for (int i = 0; i < k.Length; i++)
        {
            Colors.Add(Vector4.Lerp(new Vector4(1f, 0f, 0f, 1f), new Vector4(0f, 0f, 1f, 1f), (float)k[i] / (float)MostNeighbours));
            Result += "Neighbours: " + k[i].ToString() + " at " + Positions[i] + "\n";
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