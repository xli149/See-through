using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AuthorityController : NetworkBehaviour 
{
    // Start is called before the first frame update

    Vector3 ClientCameraPosition = Vector3.zero;
    Quaternion ClientCameraRotation = Quaternion.identity;
    GameObject cube;
    void Start()
    {
        cube = GameObject.Find("Cube");
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Client in Server: " + ClientCameraPosition + " , " + ClientCameraRotation);

        if (isServer && !isClient) {
            Debug.Log("not local..");
            
            return; }


        Debug.Log("test");
        CmdSyncClientTransform();


        if (Input.GetKey(KeyCode.Q))
        {
           GetAuthority();
        }



    }



    private void GetAuthority()
    {
        var go = GameObject.Find("Cube");
        var id = go.GetComponent<NetworkIdentity>();
        CmdAuthority(id, connectionToClient);

    }

    [Command]
    private void CmdSyncClientTransform() {


        var cameraPosition = Camera.main.transform.position;
        var cameraRotation = Camera.main.transform.rotation;

        ClientCameraPosition = Camera.main.transform.position;
        ClientCameraRotation = Camera.main.transform.rotation;

        var cubePosition = cube.transform.position;
        var cubeRotation = cube.transform.rotation;

        Debug.Log("Client camera: transform: " + cameraPosition + ", " + cameraRotation);
        Debug.Log("Cube client in server: " + cubePosition + ", " + cubeRotation);

    }
    [Command]  //called in client, run in server
    private void CmdAuthority(NetworkIdentity identity, NetworkConnectionToClient connectionToClient)

    {   //ÒÆ³ýÈ¨ÏÞ
        identity.RemoveClientAuthority();
        identity.AssignClientAuthority(connectionToClient);
        var cameraPosition = Camera.main.transform.position;
        var cameraRotation = Camera.main.transform.rotation;

        //Debug.Log("Client: transform: " + cameraPosition + ", " + cameraRotation);
    }
}
