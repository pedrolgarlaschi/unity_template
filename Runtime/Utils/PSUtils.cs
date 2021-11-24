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

    
    public static void SetPlayerPrefVector(string name , Vector2 value)
    {
        PlayerPrefs.SetFloat(string.Format("{0}X", name) , value.x);
        PlayerPrefs.SetFloat(string.Format("{0}Y", name), value.y);
    }

    public static void SetPlayerPrefVector(string name, Vector3 value)
    {
        PlayerPrefs.SetFloat(string.Format("{0}X", name), value.x);
        PlayerPrefs.SetFloat(string.Format("{0}Y", name), value.y);
        PlayerPrefs.SetFloat(string.Format("{0}Z", name), value.z);
    }

    public static Vector2 GetPlayerPrefVector(string name, Vector2 defaultValue)
    {
        float x = PlayerPrefs.GetFloat(string.Format("{0}X", name), defaultValue.x);
        float y = PlayerPrefs.GetFloat(string.Format("{0}Y", name), defaultValue.y);

        return new Vector2(x, y);
    }

    public static Vector3 GetPlayerPrefVector(string name , Vector3 defaultValue)
    {

        float x = PlayerPrefs.GetFloat(string.Format("{0}X", name), defaultValue.x);
        float y = PlayerPrefs.GetFloat(string.Format("{0}Y", name), defaultValue.y);
        float z = PlayerPrefs.GetFloat(string.Format("{0}Z", name), defaultValue.z);

        return new Vector3(x, y, z);
    }

    public static JSONObject Vec3ToJson(Vector3 value)
    {
        JSONObject json = new JSONObject();
        json.AddField("x", value.x);
        json.AddField("y", value.y);
        json.AddField("z", value.z);

        return json;
    }


    public static Vector3 JsonToVec3(JSONObject value)
    {
        float x = value["x"].f;
        float y = value["y"].f;
        float z = value["z"].f;


        return new Vector3(x,y,z);
    }

}