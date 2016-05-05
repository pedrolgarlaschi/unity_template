using UnityEngine;
using System.Collections;


public class Utils : MonoBehaviour {

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
			if(t.transform == go.transform)
				continue;


			Destroy(t.gameObject);
		}
	}




	public static T Find<T> (string name) where T : Component
	{
		return GameObject.Find (name).GetComponent<T> ();
	}

}
