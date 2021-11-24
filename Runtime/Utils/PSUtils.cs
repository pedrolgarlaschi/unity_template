using UnityEngine;
using System.Collections;


public class PSUtils
{

    /// <summary>
    /// Map float value
    /// </summary>
    /// <returns>The map.</returns>
    /// <param name="val">Value.</param>
    /// <param name="inMin">In minimum.</param>
    /// <param name="inMax">In max.</param>
    /// <param name="outMin">Out minimum.</param>
    /// <param name="outMax">Out max.</param>
    public static float FloatMap(float val, float inMin, float inMax, float outMin, float outMax)
    {
        return outMin + (outMax - outMin) * ((val - inMin) / (inMax - inMin));
    }

    public static void DestroyChildes(GameObject go)
    {
        foreach (Transform t in go.transform)
        {
            if (t.transform == go.transform)
                continue;


            GameObject.Destroy(t.gameObject);
        }
    }

    public static T Find<T>(string name) where T : Component
    {
        return GameObject.Find(name).GetComponent<T>();
    }

    public static T Instantiate<T>(string name = "") where T : Component
    {
        GameObject go = new GameObject(name == "" ? typeof(T).ToString() : name);

        return go.AddComponent<T>();
    }

}