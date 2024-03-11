using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.PackageManager;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Net.Http;

public class TestServer : MonoBehaviour { 
    

    public int port = 12345;
    // public string debug_ip = "192.168.1.9";
    //public string debug_ip = "192.168.1.7";
    // public string ip = "192.168.1.9";
    //public string ip = "127.0.0.1";

    public string debug_ip = "10.30.3.116";
    public string ip = "10.30.3.116";

    Thread socketThread;
    TcpClient socket;
    bool bConnected = false;

    public GameObject imageDisplay;

    public BinaryReader sizeReader;
    public BinaryReader br;
    public BinaryWriter bw;


    Texture2D receiveTexture;
    Material material;


    int width = 720;
    int height = 1280;


    private void Awake()
    {
        //UnityThread.initUnityThread(); 
    }

    private void Start()
    {
        //Connet();

        receiveTexture = new Texture2D(width, height);
        material = new Material(Shader.Find("Standard"));
    }

    public void Connet(){ 

        Debug.Log("TEST");


        if (socketThread != null) { 
        
            if (socketThread.IsAlive)
            {
                socketThread.Abort();
            }
         
        }

        socketThread = new Thread(new ThreadStart(SocketLoop));

        socketThread.IsBackground = true;

        socketThread.Start();
    }

    private void Update()
    {
        if (bConnected)
        {
            if (ReceiveMessage())
            {

                //byte rectype = GetReceivedMsgType();
                //if (rectype == 0x01)  // initial connection, receive server request about the camera view
                //{
                //    Debug.Log("get here");
                //    ClearSocketBuffer();

                //}
                //if (rectype == 0x02) // receive camera jpg
                //{
                    DecodeVideoMsg();

                //}

            }
        }

    }


    byte GetReceivedMsgType()
    {
        byte[] buffer = new byte[1];
        int inRead = socket.GetStream().Read(buffer, 0, 1);
        return buffer[0];
    }


    bool ReceiveMessage() {

        int socket_buffer_len = socket.Available;

        if (socket_buffer_len != 0) {
            return true;
        }
        else { return false; }
    
    }

    void SocketReadByteArray(out byte[] buf, int len)
    {
        int nBytesToRead = len;
        buf = new byte[nBytesToRead];
        int nBytesRead = 0;
        while (nBytesRead < nBytesToRead)
        {
            Debug.Log("nBytesRead " + nBytesRead + "nBytesToRead " +  nBytesToRead);
            nBytesRead += socket.GetStream().Read(buf, nBytesRead, Math.Min(nBytesToRead - nBytesRead, 64000));  // maximum UDP lenght is 64k


        }

    }

    void ClearSocketBuffer()
    {
        int socket_buffer_len = socket.Available;
        byte[] buffer = new byte[socket_buffer_len];
        int nRead = 0;
        while (nRead < socket_buffer_len)
            nRead += socket.GetStream().Read(buffer, nRead, socket_buffer_len - nRead);
    }

    //void DecodeVideoMsg()
    //{
    //    NetworkStream stream = socket.GetStream();
    //    int msglen = SocketReadInt();

    //    //Debug.Log("msgLen: " + msglen);
    //    //br = new BinaryReader(stream);
    //    //byte[] image = br.ReadBytes(msglen);

    //    byte[] buf = new byte[msglen - 1];
    //    SocketReadByteArray(out buf, buf.Length);




    //}


    void DecodeVideoMsg()
    {


        NetworkStream stream = socket.GetStream();
        byte[] buffer = new byte[4];
        br = new BinaryReader(stream);
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        int receivedData = BitConverter.ToInt32(buffer, 0);
        //print("msg length " + receivedData);
        byte[] image = br.ReadBytes(receivedData);

        //Texture2D  receiveTexture = new Texture2D(width, height);

        receiveTexture.LoadImage(image);

        //Material material = new Material(Shader.Find("Standard"));
        material.mainTexture = receiveTexture;
        Renderer renderer = imageDisplay.GetComponent<Renderer>();
        renderer.material = material;

        
    }


    int SocketReadInt() {
        byte[] buf = new byte[4]; 
        
        int len = 0;

        while (len < 4) {

            len += socket.GetStream().Read(buf, len, 4 - len);
        }

        return BitConverter.ToInt32(buf, 0);

    
    }


    private void  SocketLoop() { 
        
        if (socket != null)
        {
            socket.Close();

        }
        try
        {
            Debug.Log("GETHERE");
            // socket = new TcpClient("192.168.1.9", 12345);
            socket = new TcpClient("10.30.1.117", 12345);

            //socket.Connect("192.168.1.7", 9999);

            //IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            //socket.Connect(ipEndPoint);
            bConnected = socket.Connected;
            Debug.Log("Feedback Socket Connected: " + bConnected.ToString());
        
            while (bConnected) {


                //NetworkStream stream = socket.GetStream();



                ////Debug.Log("msg len " + msg_size);
                //br = new BinaryReader(stream);
                //int msgSize = BitConverter.ToInt32(br.ReadBytes(4), 0);
                //byte[] img_buf = new byte[msgSize - 1 - 2 * sizeof(int)];
                //SocketReadByteArray(out img_buf, img_buf.Length);

                //Debug.Log("msg len: " + msgSize);

                //Debug.Log("test running");

                //NetworkStream stream = socket.GetStream();
                //sizeReader = new BinaryReader(stream);

                //int width = sizeReader.ReadInt32();
                //int height = sizeReader.ReadInt32();

                //Debug.Log($"{width} {height}");



                //br = new BinaryReader(stream);

                //byte[] data = br.ReadBytes(width * height * 3);

                //receiveTexture = new Texture2D(width, height);

                //receiveTexture.LoadImage(data);

                //Material material = new Material(Shader.Find("Standard"));
                //material.mainTexture = receiveTexture;  
                //Renderer renderer = imageDisplay.GetComponent<Renderer>();
                //renderer.material = material;

                //sizeReader.Close();
                //br.Close();
                //stream.Close();


            }
        }

        catch (Exception e)
        {
            Debug.Log(e);
        }
        
    
    }



}