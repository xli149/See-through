using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.OpenXR;
using TMPro;
#if WINDOWS_UWP
using Windows.Perception.Spatial;
#endif
using Microsoft.MixedReality.Toolkit.Utilities;

public class TestCoordinateConver : MonoBehaviour
{

    GameObject plane;
    GameObject textObject;
    TextMesh textMesh;
    // Start is called before the first frame update
    void Start()
    {
        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = new Vector3(0, 1, 2);
        plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        plane.transform.rotation = Quaternion.Euler(-90, 0, 0);
        textObject = new GameObject("Text");
        textObject.transform.parent = plane.transform;
        textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = "Your Text Here";
        textObject.transform.position = new Vector3(0, 1, 2);
        textObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        textObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        textMesh.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLocation();
    }

    private void UpdateLocation()
    {
        System.Numerics.Matrix4x4? relativePose = System.Numerics.Matrix4x4.Identity;
#if WINDOWS_UWP

            SpatialCoordinateSystem coordinateSystem = PerceptionInterop.GetSceneCoordinateSystem(UnityEngine.Pose.identity) as SpatialCoordinateSystem;
            // Get the relative transform from the unity origin
            relativePose = coordinateSystem.TryGetTransformTo(coordinateSystem);
#endif
        
        System.Numerics.Matrix4x4 newMatrix = relativePose.Value;
        newMatrix.M13 = -newMatrix.M13;
        newMatrix.M23 = -newMatrix.M23;
        newMatrix.M43 = -newMatrix.M43;

        newMatrix.M31 = -newMatrix.M31;
        newMatrix.M32 = -newMatrix.M32;
        newMatrix.M34 = -newMatrix.M34;

        System.Numerics.Vector3 scale;
        System.Numerics.Quaternion rotation1;
        System.Numerics.Vector3 translation1;
        System.Numerics.Matrix4x4.Decompose(newMatrix, out scale, out rotation1, out translation1);
        var translation = new Vector3(translation1.X, translation1.Y, translation1.Z);
        var rotation = new Quaternion(rotation1.X, rotation1.Y, rotation1.Z, rotation1.W);
        //var pose = new Pose(translation, rotation);
        Vector3 objPos = gameObject.transform.position;
        Quaternion objRot = gameObject.transform.rotation;
        var pose = new Pose(objPos, objRot);
        if (CameraCache.Main.transform.parent != null)
        {
            Debug.Log("get here");
            pose = pose.GetTransformedBy(CameraCache.Main.transform.parent);
        }


        pose.rotation *= Quaternion.Euler(90, 0, 0);
        //CheckPosition(pose);
        //Debug.Log("POSE: " + pose);
        //Debug.Log("loc: " + objPos + ", " + objRot);
        textMesh.text = pose.ToString();
    }

    private Pose? lastPose;

    private void CheckPosition(Pose pose)
    {
        if (lastPose == null)
        {
            lastPose = pose;
            return;
        }

        if (Mathf.Abs(Quaternion.Dot(lastPose.Value.rotation, pose.rotation)) > 0.99f &&
            Vector3.Distance(lastPose.Value.position, pose.position) < 0.5f)
        {
            lastPose = null;
            gameObject.transform.SetPositionAndRotation(pose.position, pose.rotation);
            

        }
        else
        {
            lastPose = pose;
        }
    }


}
