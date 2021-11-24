using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDelegate : MonoBehaviour
{
    public delegate void OnTriggerEnterDelegate(GameObject go, GameObject col);
    public delegate void OnTriggerExitDelegate(GameObject go, GameObject col);

    public event OnTriggerEnterDelegate OnTriggerEnterEvent;
    public event OnTriggerEnterDelegate OnTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(gameObject, other.gameObject);
    }


    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(gameObject, other.gameObject);
    }

}