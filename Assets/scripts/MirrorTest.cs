using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Runtime.InteropServices;
public class MirrorTest : NetworkManager
{


    private void Start()
    {
        NetworkManager manager = GetComponent<NetworkManager>();
        //manager.StartHost();
#if !UNITY_EDITOR
            manager.StartClient();
#else
        //manager.StartHost();
#endif
    }
    public override void OnClientConnect()
    {
        base.OnClientConnect();

        Debug.Log("I connected to the server");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        Debug.Log("New player Added");
    }

    public static bool isHololens()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
            System.Diagnostics.Debugger.IsAttached;
    }


}
