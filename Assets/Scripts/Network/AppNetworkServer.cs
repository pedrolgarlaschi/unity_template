using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

namespace inition.network
{
    [RequireComponent(typeof(NetworkDiscovery))]
    public class AppNetworkServer : NetworkManager
    {

        private NetworkDiscovery m_discovery;
        private List<NetworkConnection> m_connections;

        void Awake()
        {
            m_discovery = gameObject.GetComponent<NetworkDiscovery>();
            m_connections = new List<NetworkConnection>();
        }

        void Start()
        {
            StartServer();
            m_discovery.Initialize();
            m_discovery.StartAsServer();
        }


        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);
            m_connections.Add(conn);

        }

     
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            m_connections.Remove(conn);

        }


        void OnApplicationQuit()
        {
            Shutdown();
        }

    }
}
