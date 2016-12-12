using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


namespace inition.network
{
    public delegate void OnBroadcastReceived(string fromAddress, string data);

    [RequireComponent(typeof(NetworkDiscovery))]
    public class ClientDiscovery : NetworkDiscovery
    {

        public OnBroadcastReceived OnBroadcastReceivedHandler;


        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            OnBroadcastReceivedHandler(fromAddress, data);
            base.OnReceivedBroadcast(fromAddress, data);
        }
    }
}