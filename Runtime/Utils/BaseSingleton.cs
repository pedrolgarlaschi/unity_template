using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance != null)
                return instance;

            instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
            return instance;
        }
    }
}
