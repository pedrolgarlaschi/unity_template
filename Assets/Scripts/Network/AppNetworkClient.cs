using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace inition.network
{
    [RequireComponent(typeof(ClientDiscovery))]
    public class AppNetworkClient : NetworkManager
    {

        public bool IsConnected { private set; get; }
        private ClientDiscovery m_discovery;

        private NetworkConnection m_network;
        private bool m_toConnect;


        void Start()
        {
            m_discovery = gameObject.GetComponent<ClientDiscovery>();
            m_discovery.Initialize();
            m_discovery.StartAsClient();

            m_discovery.OnBroadcastReceivedHandler += OnReceivedBroadcast;

        }

        private void OnReceivedBroadcast(string fromAddress, string data)
        {
            if (IsConnected || m_toConnect)
                return;

            m_toConnect = true;

            networkAddress = fromAddress;
            StartClient();

            Debug.Log("Connected");
        }


        public override void OnClientConnect(NetworkConnection conn)
        {
            IsConnected = true;
            base.OnClientConnect(conn);

            m_network = conn;

            m_toConnect = false;
        }



        public override void OnClientDisconnect(NetworkConnection conn)
        {
            m_toConnect = false;
            m_network = null;
            IsConnected = false;
            base.OnClientDisconnect(conn);
        }
      
        void OnApplicationQuit()
        {
            Shutdown();
        }


    }
}