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
        DescribeData(PosList); // a custom function
    }
    
    //do not modify this method. This is needed by the Lume appliction to return results
    public static void ReturnResults()
    {
        LumeRuntime.API.ReturnResults(Result); //return 'Result' to script menu in Lume application
        LumeRuntime.API.ShowResult(Result); //creates a panel with 'Result' value on it in Lume application
    }

    //'DescirbeData' is an example of a custom function. It operates similar to the python function: 'pandas.DataFrame.describe'
    static void DescribeData(List<Vector3> PosList)
    {
        string Description = "";
        Description = "Count: " + PosList.Count;
        Description += "\nMean: " + PosList.Average(x => x.x) + ", " + PosList.Average(y => y.y) + ", " + PosList.Average(z => z.z);
        Description += "\nStd: " + StdDev(PosList).ToString();
        Description += "\nMin: " + PosList.Min(x => x.x) + ", " + PosList.Min(y => y.y) + ", " + PosList.Min(z => z.z);
        Description += "\n25%: " + PosList[(int)((float)PosList.Count * 0.25f)].ToString();
        Description += "\n50%: " + PosList[(int)((float)PosList.Count * 0.5f)].ToString();
        Description += "\n75%: " + PosList[(int)((float)PosList.Count * 0.75f)].ToString();
        Description += "\nMax: " + PosList.Max(x => x.x) + ", " + PosList.Max(y => y.y) + ", " + PosList.Max(z => z.z);

        Result = Description;
    }

    //Helper function called from inside of 'DescribeData'
    public static Vector3 StdDev(this IEnumerable<Vector3> values)
    {
        float Progress = 0;

        // ref: Modified from this http://warrenseen.com/blog/2006/03/13/how-to-calculate-standard-deviation/
        Vector3 mean = new Vector3();
        Vector3 sum = new Vector3();
        Vector3 stdDev = new Vector3();
        int n = 0;
        foreach (Vector3 val in values)
        {
            if (LumeRuntime.API.ScriptExecuteCancelled()) // check the Lume Application if this script has been cancelled prematurely
            {
                return;
            }
            n++;
            float deltaX = val.x - mean.x;
            mean.x += deltaX / n;
            sum.x += deltaX * (val.x - mean.x);

            float deltaY = val.y - mean.y;
            mean.y += deltaY / n;
            sum.y += deltaY * (val.y - mean.y);

            float deltaZ = val.z - mean.z;
            mean.z += deltaZ / n;
            sum.z += deltaZ * (val.z - mean.z);

            Progress = (float)n / (float)values.Count();
            LumeRuntime.API.UpdateProgress("\nProgress: " + (Progress * 100f).ToString("F2") + "%");
        }
        if (1 < n)
        {
            stdDev.x = (float)Math.Sqrt((float)sum.x / (float)(n - 1));
            stdDev.y = (float)Math.Sqrt((float)sum.y / (float)(n - 1));
            stdDev.z = (float)Math.Sqrt((float)sum.z / (float)(n - 1));
        }

        return stdDev;
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