using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class CoordinateSender : MonoBehaviour
{
    // Start is called before the first frame update

    private NetworkStream _stream;
    private TcpClient _tcpClient;
    bool isConnected = false;

    Vector3 objectPosition;

    Quaternion objectRotation;
    byte[] dataBytes;

    void Start()
    {
        OnConnectedToServer();

    }

    private float[] VectorConvertToFloatArray(Vector3 vector)
    {
        float[] floatArray1 = new float[3];

        floatArray1[0] = vector.x;
        floatArray1[1] = vector.y;
        floatArray1[2] = vector.z;
        return floatArray1;
    }


    private float[] QuaternionConvertToFloatArray(Quaternion quaternion)
    {
        float[] floatArray2 = new float[4];

        floatArray2[0] = quaternion.x;
        floatArray2[1] = quaternion.y;
        floatArray2[2] = quaternion.z;
        floatArray2[3] = quaternion.w;
        return floatArray2;
    }

    private byte[] PackArrays(float[] array1, float[] array2)
    {
        // ��������ĳ���
        int array1Length = array1.Length;
        int array2Length = array2.Length;

        // ����һ���ֽ����飬���ڴ洢���鳤�Ⱥ���������
        byte[] dataBytes = new byte[8 + array1Length * sizeof(float) + array2Length * sizeof(float)];

        // ���ֽ�����ǰ4���ֽ���д��array1�ĳ���
        BitConverter.GetBytes(array1Length).CopyTo(dataBytes, 0);

        // ���ֽ�����ĵ�4������4+array1Length*4���ֽ���д��array1������
        Buffer.BlockCopy(array1, 0, dataBytes, 4, array1Length * 4);

        // ���ֽ�����ĵ�4+array1Length*4����8+array1Length*4���ֽ���д��array2�ĳ���
        BitConverter.GetBytes(array2Length).CopyTo(dataBytes, 4 + array1Length * 4);

        // ���ֽ�����ĵ�8+array1Length*4����8+array1Length*4+array2Length*4���ֽ���д��array2������
        Buffer.BlockCopy(array2, 0, dataBytes, 8 + array1Length * 4, array2Length * 4);

        return dataBytes;
    }


    private void OnConnectedToServer()
    {
        _tcpClient = new TcpClient("192.168.1.9", 12347);
        //_tcpClient = new TcpClient("localhost", 12347);

        _stream = _tcpClient.GetStream();
        isConnected = _tcpClient.Connected;

        Debug.Log("connected to server");
    }

    public static Matrix4x4 ConvertTransformToMatrix4x4(Transform t)
    {
        return Matrix4x4.TRS(t.localPosition, t.localRotation, t.localScale);
    }


    // Update is called once per frame
    void Update()
    {
        objectPosition = this.transform.position;
        objectRotation = this.transform.rotation;

        //Debug.Log("Transform: " + ConvertTransformToMatrix4x4(this.transform));

        //Debug.Log("position: " + objectPosition + " rotation: " + objectRotation);

        if (isConnected)
        {

            dataBytes = PackArrays(VectorConvertToFloatArray(objectPosition), QuaternionConvertToFloatArray(objectRotation));

            try
            {
                _stream.Write(dataBytes, 0, dataBytes.Length);
            }
            catch (System.Exception e)
            {
                Debug.LogError("��������ʧ��: " + e.Message);

            }
        }
    }
}