using UnityEngine;

public class LocalCoordinate : MonoBehaviour
{

    public float size = 1f;

    private void Update()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        Vector3 xAxis = rotation * Vector3.right * size;
        Vector3 yAxis = rotation * Vector3.up * size;
        Vector3 zAxis = rotation * Vector3.forward * size;

        Debug.DrawRay(position, xAxis, Color.red);
        Debug.DrawRay(position, yAxis, Color.green);
        Debug.DrawRay(position, zAxis, Color.blue);
    }
}