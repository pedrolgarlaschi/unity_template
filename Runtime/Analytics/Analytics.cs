using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Analytics : MonoBehaviour
{
    private static string Path
    {

        get
        {
            System.DateTime date = System.DateTime.Now;

            string folder = Application.streamingAssetsPath + "/Analytics";
            string path = folder + "/" + date.Day + "_" + date.Month + "_" + date.Year + ".txt";

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!File.Exists(path))
                File.WriteAllText(path, "events,params,date\n");

            return path;
        }
    
    }

    public static void Event(string name , string param = "")
    {
        string str = string.Format("{0},{1},{2}", name, param, System.DateTime.Now.ToString());
            
        Save(str);
    }

    public static void Save(string str)
    {
        using (StreamWriter sw = File.AppendText(Path))
        {
            sw.WriteLine(str);
           
        }
    }

}
