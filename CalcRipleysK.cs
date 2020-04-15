///
///CalcRipleysK for vLume, based on 
///A. Spark, A. Kitching (16/01/2020)

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;


//Vector3's are a class provided by Unity:
//https://docs.unity3d.com/ScriptReference/Vector3.html
//Mathf is a class provided by Unity:
//https://docs.unity3d.com/ScriptReference/Mathf.html


//Do not modify this class. This is needed by the Lume application to run your code
public static class ExecuteProcess
{
    //String to hold our custom function results to return to the Lume applicaiton
    public static string Result;
    //String to hold our data point xyz values to return to the Lume applicaiton
    public static string Positions;
    //String to hold our custom function results to return to the Lume applicaiton
    public static string ResultMessage;


    public static float PROMPT_RegionStep = 0.05f;
    public static float PROMPT_RegionSize = 4f;

    //do not modify this method. This is needed by the Lume application to run your code
    public static void Execute(List<Vector3> PosList)//'PosList' is the isolated array of positions as xyz values, as they appear in the dataset's CSV
    {
        //This is where you would call your custom function
        RipleysK(PosList); // a custom function
    }

    //do not modify this method. This is needed by the Lume appliction to return results
    public static void ReturnResults()
    {
        LumeRuntime.API.ReturnResults(Result); //return 'Result' to script menu in Lume application
        LumeRuntime.API.OutputToFile(new object[] { ResultMessage, Result, "Data points operated on.", Positions }); //writes the values of ResultMessage, result, a string and positions to a text file
    }

    //'RipleysK' is an example of a custom function. It returns calculated L values from a selected ROI
    static void RipleysK(List<Vector3> PosList)
    {
        List<float> region = new List<float>();

        float Cumulative = 0;

        float TotalProgress = 0;

        int RegionLimit = Mathf.CeilToInt(PROMPT_RegionSize / PROMPT_RegionStep);
        for (int i = 0; i < RegionLimit; i++) //change for varible - user input
        {
            if (LumeRuntime.API.ScriptExecuteCancelled()) // check the Lume Application if this script has been cancelled prematurely
            {
                return;
            }
            TotalProgress = (float)i / (float)(PROMPT_RegionSize / PROMPT_RegionStep);
            LumeRuntime.API.UpdateProgress("Creating Regions: " + (TotalProgress * 100f).ToString("F2") + "%"); // Allows progress of script to be seen in Lume
            region.Add(Cumulative);
            Cumulative += PROMPT_RegionStep;

            if (Cumulative >= PROMPT_RegionSize)
            {
                break;
            }
        }

        int n = PosList.Count;

        float Xdist = Math.Abs(PosList.Max(x => x.x) - PosList.Min(x => x.x));
        float Ydist = Math.Abs(PosList.Max(x => x.y) - PosList.Min(x => x.y));
        float Zdist = Math.Abs(PosList.Max(x => x.z) - PosList.Min(x => x.z));

        float v = Xdist * Ydist * Zdist; // volume for Region of data

        float[] k = new float[region.Count];
        List<float> res = new List<float>();  //list of pairwise distances of localizations

        for (int i = 0; i < PosList.Count - 1; i++)
        {
            if (LumeRuntime.API.ScriptExecuteCancelled()) // check the Lume Application if this script has been cancelled prematurely
            {
                return;
            }
            TotalProgress = (float)i / (float)PosList.Count;
            LumeRuntime.API.UpdateProgress("Calculationg Distances: " + (TotalProgress * 100f).ToString("F2") + "%"); // Allows progress of script to be seen in Lume
            for (int j = i + 1; j < PosList.Count; j++)
            {
                res.Add(Vector3.Distance(PosList[i], PosList[j]));
            }
            Positions += PosList[i].x.ToString() + "," + PosList[i].y.ToString() + "," + PosList[i].z.ToString() + "\n";
        }

        int BooleanSum = 0;

        for (int i = 0; i < region.Count; i++)
        {
            if (LumeRuntime.API.ScriptExecuteCancelled()) // check the Lume Application if this script has been cancelled prematurely
            {
                return;
            }
            BooleanSum = 0;
            TotalProgress = (float)i / (float)region.Count;
            LumeRuntime.API.UpdateProgress("Calculating Regions: " + (TotalProgress * 100f).ToString("F2") + "%"); // Allows progress of script to be seen in Lume
            for (int j = 0; j < res.Count; j++)
            {
                if (res[j] < region[i])
                {
                    BooleanSum++;
                }
            }
            k[i] = BooleanSum * 2;
        }

        List<double> L = new List<double>();

        for (int i = 0; i < k.Length; i++)
        {
            if (LumeRuntime.API.ScriptExecuteCancelled()) // check the Lume Application if this script has been cancelled prematurely
            {
                return;
            }
            TotalProgress = (float)i / (float)k.Length;
            LumeRuntime.API.UpdateProgress("Calculating L Values: " + (TotalProgress * 100f).ToString("F2") + "%"); // Allows progress of script to be seen in Lume
            L.Add(System.Math.Pow(k[i] * 3f * v / (double)n / (4f * System.Math.PI * ((double)n - 1f)), 1f / 3f));
        }
        // L = K
        //3D Bayesian cluster analysis of super-resolution data reveals LAT recruitment to the T cell synapse
        // Scientific reports 2017

        ResultMessage = "L values result for RipleysK carried out on " + PosList.Count + " points with a volume of " + v + " and region size of " + PROMPT_RegionSize + " and a step value of " + PROMPT_RegionStep;

        for (int i = 0; i < L.Count; i++)
        {
            if (LumeRuntime.API.ScriptExecuteCancelled()) // check the Lume Application if this script has been cancelled prematurely
            {
                return;
            }
            Result += "L[" + i + "]: " + L[i] + "\n";
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