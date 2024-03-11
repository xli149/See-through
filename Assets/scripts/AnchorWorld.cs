using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class AnchorWorld : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject cube_anchor;
    public ImageTargetBehaviour imageTarget1;
    public ImageTargetBehaviour imageTarget3;

    public GameObject cube;
    Vector3 savedTransform;

    public bool isFound = false;


    //public void AnchorWorldCenter() {

    //    if ( savedTransform != null) {
    //        savedTransform = imageTarget3.transform.position;
    //        cube_anchor.transform.position = savedTransform;
    //        Debug.Log("GET hete");
    //        Debug.Log("Saved Transform: " + savedTransform);
    //    }

    //    isFound = true; 

    //}

    public void onTrackingFound() {

        isFound = true;
        savedTransform = imageTarget3.transform.position;

    
        //cube_anchor.transform.position = savedTransform;
        Debug.Log("Saved Transform: " + savedTransform);
        Debug.Log("is Found: " + isFound);
    }


    private void LateUpdate()
    {

        //Debug.Log("Saved transform: " + savedTransform);
        if (isFound)
        {
            cube_anchor.transform.position = imageTarget3.transform.position;

            //isFound = false;
            //imageTarget3.enabled = false;
        }
    }

    public void OnDisableImageTarget()
    {
        isFound = !isFound;
        imageTarget1.enabled = isFound;
    }
    public void changeFoundStatus() { 
    
        isFound= !isFound;
        imageTarget3.enabled = isFound;

 
     
    }
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFound)
        {
            cube.GetComponent<MeshRenderer>().material.color = Color.white;

        }
        else
        {

            cube.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
