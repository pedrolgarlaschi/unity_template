using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public static class ExtensionMethods {
	

	/// <summary>
	/// Splits strings (the default unity just split chars)
	/// </summary>
	/// <returns>The string.</returns>
	/// <param name="str">String.</param>
	/// <param name="value">Value.</param>
	public static string[] SplitString(this string str, string value)
	{
		string[] list = str.Split(new string[] { value }, StringSplitOptions.None);
		return list;
	}
	
	/// <summary>
	/// Get chield from name
	/// </summary>
	/// <returns>The chield from name.</returns>
	/// <param name="target">Target.</param>
	/// <param name="name">Name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T GetChildFromName<T>(this Transform target, string name) where T : Component
	{
		T[] transforms = target.gameObject.GetComponentsInChildren<T> ();

		foreach (T t in transforms) {

			if(t.name == name)
				return t;
		}

		return null;
	}

	/// <summary>
	/// Get array of chields with the same name
	/// </summary>
	/// <returns>The chields from name.</returns>
	/// <param name="target">Target.</param>
	/// <param name="name">Name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T[] GetChildsFromName<T>(this Transform target, string name) where T : Component
	{
		T[] transforms = target.gameObject.GetComponentsInChildren<T> ();
		List<T> arr = new List<T> ();
		
		foreach (T t in transforms) {
			
			if(t.name == name)
				arr.Add(t);
		}
		
		return arr.ToArray();
	}

	
	/// <summary>
	/// Change layermask from the chields
	/// </summary>
	/// <param name="target">Target.</param>
	/// <param name="layer">Layer.</param>
	public static void ChangeAllChieldLayers(this GameObject target , int layer)
	{
		Transform[] transforms = target.gameObject.GetComponentsInChildren<Transform> ();
		
		foreach (Transform t in transforms)
			t.gameObject.layer = layer;

	}

	/// <summary>
	/// Enable and disable renderers
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="enable">If set to <c>true</c> enable.</param>
	public static void EnableRenderer(this GameObject go , bool enable)
	{
		Renderer[] arrR = go.GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in arrR)
			r.enabled = enable;


		SpriteRenderer[] arrS = go.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer r in arrS)
			r.enabled = enable;


		Image[] arrI = go.GetComponentsInChildren<Image> ();
		foreach (Image i in arrI)
			i.enabled = enable;

		Text[] arrT = go.GetComponentsInChildren<Text> ();
		foreach (Text t in arrT)
			t.enabled = enable;

	}

	/// <summary>
	/// Instance the specified o and name.
	/// </summary>
	/// <param name="o">O.</param>
	/// <param name="name">Name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T Instance<T>(this T o , string name) where T : MonoBehaviour
	{
		GameObject obj = new GameObject ();
		obj.name = name;
		return obj.AddComponent<T> ();
	}

	/// <summary>
	/// Get instance on the scene from name
	/// </summary>
	/// <returns>The instance.</returns>
	/// <param name="obj">Object.</param>
	/// <param name="name">Name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T GetInstance<T>(this T obj , string name) where T : Component
	{
		GameObject go = GameObject.Find (name);

		if (go == null) {


			return null;
		}


		return go.GetComponent<T> ();
		
	}


}
