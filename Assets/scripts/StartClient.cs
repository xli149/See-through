using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartClient : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager manager = GetComponent<NetworkManager>();    
        manager.StartClient();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
