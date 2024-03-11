using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using Vuforia;
using UnityEngine.XR.ARFoundation;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.OpenXR;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR;
using Unity.VisualScripting;
#if WINDOWS_UWP
using Windows.Perception.Spatial;
#endif


public class DisableClientCamera : NetworkBehaviour
{
    public GameObject cube;
    public GameObject cube2;
    private GameObject cube_fix;
    public ImageTargetBehaviour imageTarget;
    public ImageTargetBehaviour imageTarget2;
    public ImageTargetBehaviour imageTarget3;

    public ObserverBehaviour imageTarget3Observer;
    private Camera camera;

    public GameObject AnchorCube;
    [SyncVar] Vector3 Position;

    [SyncVar] Quaternion Rotation;

    [SyncVar] Vector3 calculatedPose;
    [SyncVar] Vector3 calculatedPose2;
    [SyncVar] Quaternion calculatedRotation;
    [SyncVar] Quaternion calculatedRotation2;
    [SyncVar]Vector3 ServerCameraPosition;
    [SyncVar]Quaternion ServerCameraRotation;

    [SyncVar] Vector3 positionDiff;
    [SyncVar] Quaternion rotationDiff;


    [SyncVar] Matrix4x4 calibrationMatrix;

    public GameObject cube_anchor;
    //[SyncVar(hook = nameof(OnPositionChanged))] Vector3 CameraPosition = new Vector3(0, 0, 0);

    Vector3 LatestPose = Vector3.zero;
    Quaternion LatestRotation = new Quaternion(0.50000f, 0.00000f, 0.00000f, 0.86603f);
    public EventHandler<Pose> PositionAcquired;
    public EventHandler PositionAcquisitionFailed;
    [SyncVar]Vector3 relativePosition;
    Vector3 relativePosition2;
    Quaternion cubeRotation;
    Quaternion relativeRotation;
    Vector3 initialCameraPosition;
    Quaternion initialCameraRotation;
    double[] cameraMatrixArray = new double[]





    
{
    1063.1483293772912, 0.0, 585.5481902923639,
    0.0, 1055.1397677793466, 356.77826764135676,
    0.0, 0.0, 1.0
};


    double[] distCoeffArray = new double[]
{
    0.0071885835941825574, 0.2903017753709522,
    0.008438313294441951, -0.024883617665220157, 0.0
};

    private async void Start()
    {

        //fixedCameraObject = new GameObject("FixedCameraObject");
        //fixedCameraObject.transform.position = Vector3.zero;
        //fixedCameraObject.transform.rotation = Quaternion.identity;

        //cube = GameObject.Find("Cube");
        cube_fix = GameObject.Find("Cube_fix");
        //imageTarget = GetComponent<ImageTargetBehaviour>();
        //imageTarget2 = GetComponent<ImageTargetBehaviour>();
        camera = Camera.main;
        initialCameraRotation = Camera.main.transform.rotation;

        //imageTarget3Observer.OnTargetStatusChanged += onTargetStatusChanged;

        if (isClient && !isServer)
        {
            //cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //imageTarget.enabled = false;
            imageTarget2.enabled = false;
            initialCameraPosition = Camera.main.transform.position;
            initialCameraRotation = Camera.main.transform.rotation;

            calibrationMatrix = Matrix4x4.identity;
        
        }


    }

    void onTargetStatusChanged(ObserverBehaviour observerbehavour, TargetStatus status)
    {
        Debug.Log("Get Target Image!");

        if (status.Status == Status.TRACKED)
        {
            Debug.Log("Get Target Image!");
            //var anchorPos = imageTarget3.transform.position;
            //AnchorCube.transform.position = anchorPos;
            //imageTarget3.enabled = false;
        }

        if (status.Status == Status.EXTENDED_TRACKED && status.StatusInfo == StatusInfo.NORMAL)
        {
            Debug.Log("Miss Target Image!");
            this.gameObject.SetActive(false);
        }
    }


    void OnPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        Debug.Log("Position changed from " + oldPosition + " to " + newPosition);
    }






    private void Update()
    {

        if(isServer)
        {
    
            ServerCameraPosition= Camera.main.transform.position;
            ServerCameraRotation = Camera.main.transform.rotation;


            relativePosition = (imageTarget.transform.position - Camera.main.transform.position);
            relativePosition2 = (imageTarget2.transform.position - Camera.main.transform.position);
            //var CameraTest = Camera.main.transform.localToWorldMatrix;
            //var CameraTransform = Camera.main.transform;
            //var test =  CameraTest.inverse * imageTarget.transform.localToWorldMatrix;

            //Vector4 controlpoint = new Vector4(CameraTransform.position.x, CameraTransform.position.y, CameraTransform.position.z, 1.0f);

            //Vector4 new_controlPoint = test * controlpoint;

            //relativePosition = new Vector3(new_controlPoint.x, new_controlPoint.y, new_controlPoint.z);

            Debug.Log("----Camera position: " + Camera.main.transform.position + "-------");
            var currentRotation = Camera.main.transform.rotation;


            Debug.Log(" initialCameraRotation: " + initialCameraRotation + " currentRotation: " + currentRotation);

            relativeRotation = Quaternion.Inverse(Camera.main.transform.rotation);
            var testRotation = Quaternion.Euler(10, 0, 0);
            //calculatedPose = relativePosition;
            calculatedPose = relativeRotation * relativePosition;
            calculatedRotation = relativeRotation * imageTarget.transform.rotation;
            //calculatedRotation = imageTarget.transform.rotation;

            calculatedPose2 = relativeRotation * relativePosition2;
            //calculatedRotation =  relativeRotation* imageTarget.transform.rotation;
           
            calculatedRotation2 = relativeRotation * imageTarget2.transform.rotation;
            //calculatedRotation =  imageTarget.transform.rotation;
            //calculatedRotation =  imageTarget2.transform.rotation;
            Debug.Log("calm pos: " + Camera.main.transform.position);


            //cube.transform.position = imageTarget.transform.position;
            //cube.transform.rotation = imageTarget.transform.rotation;

            cube.transform.position = calculatedPose;
            cube.transform.rotation = calculatedRotation;
            cube2.transform.position = imageTarget2.transform.position;

            cube2.transform.rotation = imageTarget2.transform.rotation;
           

        }


        if (isClient && !isServer)
        {
            var param = 0.51f;
            var targetPos = imageTarget.transform.position;
            var targetRot = imageTarget.transform.rotation;
            //var rescaledCalculatedPose = new Vector3(-calculatedPose.z * param, calculatedPose.y * param, calculatedPose.x * param);

            //var rescaledCalculatedRotation = new Quaternion(-calculatedRotation.z, calculatedRotation.y, calculatedRotation.x, calculatedRotation.w);

            var rescaledCalculatedPose = new Vector3(calculatedPose.x * param, calculatedPose.y * param, calculatedPose.z * param);
            var rescaledCalculatedRotation = new Quaternion(calculatedRotation.x, calculatedRotation.y, calculatedRotation.z, calculatedRotation.w);
            if (imageTarget.enabled) { 
                positionDiff = targetPos - rescaledCalculatedPose;
                //rotationDiff =  Quaternion.Inverse(rescaledCalculatedRotation) * targetRot;
                rotationDiff = Quaternion.Inverse(calculatedRotation) * targetRot;

            }

            //calibrationMatrix = Matrix4x4.TRS(positionDiff, rotationDiff, Vector3.one);




            //cube.transform.rotation = rescaledCalculatedRotation * rotationDiff;
            Quaternion rotationTemp = Quaternion.AngleAxis(90f, Vector3.left);
            //cube.transform.rotation = rescaledCalculatedRotation * rotationDiff * rotationTemp;
            cube.transform.rotation = rescaledCalculatedRotation * rotationDiff;




            //if (!imageTarget.enabled)
            //{
            //    var tempPositionOff = new Vector3(positionDiff.z, positionDiff.x, positionDiff.y);
            //    cube.transform.position = rescaledCalculatedPose + tempPositionOff;
            //}
            //else { 

            //var testPos = new Vector3(rescaledCalculatedPose.y, rescaledCalculatedPose.z, rescaledCalculatedPose.x);

            var tempPos = Quaternion.Euler(0f, 45f, 0f) * (calculatedPose + positionDiff);

            cube.transform.position = rescaledCalculatedPose + positionDiff;

            //cube.transform.position = rescaledCalculatedPose + positionDiff;




            //}


            ////cube.transform.rotation = calibrationMatrix.rotation * calculatedRotation;
            //cube.transform.position = calibrationMatrix.MultiplyPoint(rescaledCalculatedPose);


            //var rescaledRotation = new Quaternion(calculatedRotation.x * param, calculatedRotation.y * param,
            //    calculatedRotation.z * param, calculatedRotation.w * param );

            cube_fix.transform.position = new Vector3(1, 1, 1);
          

        }

        //Debug.Log("before late update camera: " + Camera.main.transform.position + " , " + Camera.main.transform.rotation);


    }


    private void LateUpdate()
    {

        //imageTarget.transform.position = LatestPose + relativePosition;
        //imageTarget.transform.rotation = LatestRotation * relativeRotation;

        if (isServer) {
            //calculatedPose = LatestPose + relativePosition;
            
        }

        Debug.Log("imageTarget position: " + imageTarget.transform.position);
        Debug.Log("calculated latest pose: " + calculatedPose + LatestPose +relativePosition);
        //var calculateRot = relativeRotation;
        //cube.transform.position = calculatedPose;
        //cube.transform.rotation = calculateRot;
        Position = imageTarget.transform.position;
        Rotation = imageTarget.transform.rotation;



        //if (isClient && !isServer)
        //{

        //    //Camera.main.transform.position = ServerCameraPosition;
        //    //Camera.main.transform.rotation = ServerCameraRotation;

        //    cube.transform.position = calculatedPose;
        //    ////Debug.Log("Cube : " + cube.transform.position + " , " + cube.transform.position);
        //    //Camera.main.transform.position = ServerCameraPosition;
        //}


        //if (isClient && !isServer)
        //{


        //    camera.transform.position = ServerCameraPosition;
        //    camera.transform.rotation = ServerCameraRotation;
        //    CameraPosition = camera.transform.position;1111111111
        //    CameraRotation = camera.transform.rotation;

        //    Debug.Log("camera Position: " + CameraPosition + "camera Rotation: " + CameraRotation);
        //    cube.transform.position = Position;
        //    cube.transform.rotation = Rotation;

        //}

        //Camera.main.transform.position = Vector3.zero;
        //Camera.main.transform.rotation = new Quaternion(0.5000f, 0.00000f, 0.00000f, 0.86603f);
        //imageTarget.transform.position= Vector3.zero;
        //imageTarget.transform.rotation = new Quaternion(0.50000f, 0.00000f, 0.00000f, 0.86603f);
        Camera.main.transform.rotation = new Quaternion(0.00000f, 0.00000f, 0.00000f, 0.00000f);
        Camera.main.transform.position = new Vector3(0,0,0);
        LatestPose = Camera.main.transform.position;
        LatestRotation = Camera.main.transform.rotation;
        //Debug.Log("after late update camera: " + Camera.main.transform.position + " , " + Camera.main.transform.rotation);
        var eulerAngles = LatestRotation.eulerAngles;
        //Debug.Log("after EularAngle: " + eulerAngles);
    }


}
