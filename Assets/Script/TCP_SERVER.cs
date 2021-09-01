using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.UI;

public class TCP_SERVER : MonoBehaviour
{


    TcpListener serverSocket;
    TcpClient clientSocket;
    NetworkStream clientStream;
    [SerializeField] BonePositionTest boneTest;
    [SerializeField] int counter = 0;
    [SerializeField] int port;
    [SerializeField] Text debugText;
    [SerializeField] Text debugText2;
    bool isConnected = false;
    bool isDataReaded = false;
    string readData;
    float fpsCounter = 1;
    int prevCounter = 0;
    int currentFPS = 0;
    [SerializeField] ByteToRawTexture byteToRawTexture;
    private Thread acceptThread, readClientThread;

    private int currentFunctionType;
    private bool currentFunctionUpdated;
    
    
    // Start is called before the first frame update
    void Start()
    {
        serverSocket = new TcpListener(port);
        clientSocket = default(TcpClient);
        serverSocket.Start();
        Debug.Log("Server Started");
        counter = 0;
        acceptThread = new Thread(AcceptClient);
        acceptThread.Start();
        readClientThread = new Thread(ReadDataFromClient);
        readClientThread.Start();
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if(ip.AddressFamily == AddressFamily.InterNetwork)
                if (debugText.text.CompareTo("Client Connected") != 0)
                    debugText.text = "wating" + "\nIP is : " + ip.ToString();
        }

        currentFunctionType = 0;
        currentFunctionUpdated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (clientSocket == null)
            return;
        
        if (!clientSocket.Connected)
        {
            /*
            debugText2.text = "Client Disconnected";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                    if (debugText.text.CompareTo("Client Connected") != 0)
                        debugText.text = "wating" + "\nIP is : " + ip.ToString();
            }
            acceptThread.Start();
            */
            return;
        }

        debugText.text = "Read Function Type : " + currentFunctionType.ToString();
        switch (currentFunctionType)
        {
            case 1:
                debugText.text += "\npartialTrackingData"
                                  + "\nbyte Len : " + DataStructs.partialTrackingData.bytesLen.ToString()
                                  + "\nisTrack : " + DataStructs.partialTrackingData.isTrack.ToString()
                                  + "\nisRight : " + DataStructs.partialTrackingData.isRight.ToString()
                                  + "\ncamera Resholution : " + DataStructs.partialTrackingData.cameraResolution.ToString()
                                  + "\ntracked Position : " + DataStructs.partialTrackingData.trackedPosition.ToString()
                                  + "\ntracked Rotation : " + DataStructs.partialTrackingData.trackedRotation.ToString();
                if (currentFunctionUpdated)
                {
                    byteToRawTexture.LoadPNG(DataStructs.partialTrackingImageData);
                    boneTest.SetTrackingData();
                    currentFunctionUpdated = false;
                }
                break;
            case 2:
                break;
            case 3:
                if (currentFunctionUpdated)
                {
                    
                    currentFunctionUpdated = false;
                }
                break;
        }
        
        
        
        if (isConnected)
        {
            debugText.text = "Client Connected";
            isConnected = false;
        }

        if (fpsCounter <= 0)
        {
            currentFPS = (counter - prevCounter);
            debugText2.text = "\nFPS : " + currentFPS.ToString();
            prevCounter = counter;
            fpsCounter = 1;
        }
        else
        {
            debugText2.text = "\nFPS : " + currentFPS.ToString();
            fpsCounter -= Time.deltaTime;
        }
    }


    void AcceptClient()
    {
        clientSocket = serverSocket.AcceptTcpClient();
        if (clientSocket != null)
        {
            Debug.Log("Client Connected");
            clientSocket.ReceiveBufferSize = 12000000;
            clientStream = clientSocket.GetStream();
            isConnected = true;
        }
    }

    void ReadDataFromClient()
    {
        while (true)
        {
            if (clientSocket == null || clientStream == null)
                continue;

            if (clientStream.DataAvailable)
            {
                byte[] functionTypeBytes = BitConverter.GetBytes((int) 0);
                clientStream.Read(functionTypeBytes, 0, functionTypeBytes.Length);
                Array.Reverse(functionTypeBytes);
                currentFunctionType = BitConverter.ToInt32(functionTypeBytes, 0);

                switch (currentFunctionType)
                {
                    case 1:
                        ReadFunctionA();
                        break;
                    case 2:
                        ReadFunctionB();
                        break;
                    case 3:
                        ReadFunctionC();
                        break;
                }

            }
            
            
        }
        if (clientSocket == null)
            return;
        
        while (clientSocket.Connected)
        {
            if (clientSocket != null && clientSocket.Connected && clientStream.DataAvailable)
            {
                byte[] functionTypeBytes = BitConverter.GetBytes((int) 0);
                clientStream.Read(functionTypeBytes, 0, functionTypeBytes.Length);
                Array.Reverse(functionTypeBytes);
                int functionType = BitConverter.ToInt32(functionTypeBytes, 0);

                debugText.text = "Read Function Type : " + functionType.ToString();

                switch (functionType)
                {
                    case 1:
                        ReadFunctionA();
                        break;
                    case 2:
                        ReadFunctionB();
                        break;
                    case 3:
                        ReadFunctionC();
                        break;
                }
            }
        }
    }
    

    void ReadFunctionA()
    {
        byte[] dataBytes = new byte[Marshal.SizeOf<DataStructs.partialTrackingStruct>()];
        clientStream.Read(dataBytes, 0, dataBytes.Length);
        DataStructs.partialTrackingData = DataStructs.ByteToStruct<DataStructs.partialTrackingStruct>(dataBytes);
        
        byte[] textureRawData = new byte[DataStructs.partialTrackingData.bytesLen];

        int readDataLength = 0;
        while (readDataLength < DataStructs.partialTrackingData.bytesLen)
        {
            readDataLength += clientStream.Read(textureRawData, readDataLength, DataStructs.partialTrackingData.bytesLen - readDataLength);
        }

        DataStructs.partialTrackingImageData = textureRawData;
        
        counter += 1;
        Debug.Log("Read Texture Length : " + readDataLength.ToString());
        
        clientStream.Flush();
        currentFunctionUpdated = true;
    }

    void ReadFunctionB()
    {
        byte[] lengthBytes = BitConverter.GetBytes((int)0);
        clientStream.Read(lengthBytes, 0, lengthBytes.Length);
        Array.Reverse(lengthBytes);
        int textureLength = BitConverter.ToInt32(lengthBytes, 0);
        
        debugText.text += "\nRead Length = : " + textureLength.ToString() + "  Byte Data : " + BitConverter.ToString(lengthBytes);
        
        clientStream.Read(lengthBytes, 0, lengthBytes.Length);
        Array.Reverse(lengthBytes);
        int screenWidth = BitConverter.ToInt32(lengthBytes, 0);
        debugText.text += "\nRead width = : " + screenWidth.ToString() + "  Byte Data : " + BitConverter.ToString(lengthBytes);
        
        clientStream.Read(lengthBytes, 0, lengthBytes.Length);
        Array.Reverse(lengthBytes);
        int screenHeight = BitConverter.ToInt32(lengthBytes, 0);
        debugText.text += "\nRead height = : " + screenHeight.ToString() + "  Byte Data : " + BitConverter.ToString(lengthBytes);
        
        
        byte[] textureRawData = new byte[textureLength];
        
        
        int readData = 0;
        while (readData < textureLength)
        {
            readData += clientStream.Read(textureRawData, readData, textureRawData.Length - readData);
            //debugText.text += "\nRead Texture Length : " + textureLength + "\nRead Image Counter : " + counter + "\nRead Data Length : " + readData;
        }
        
        
        debugText.text += "\nRead complete : " + readData.ToString();
        debugText2.text = BitConverter.ToString(textureRawData);
        
        counter += 1;
        Debug.Log("Read Texture Length : " + readData);
        byteToRawTexture.LoadPNG(textureRawData);
        clientStream.Flush();
    }
    
    
    void ReadFunctionC()
    {
        byte[] dataBytes = new byte[Marshal.SizeOf<DataStructs.VRControllersStruct>()];
        clientStream.Read(dataBytes, 0, dataBytes.Length);
        DataStructs.vrControllersData = DataStructs.ByteToStruct<DataStructs.VRControllersStruct>(dataBytes);
        
        InputSystem.SetControllerData(DataStructs.vrControllersData.rightController);
        InputSystem.SetControllerData(DataStructs.vrControllersData.leftController);
        
        counter += 1;
        Debug.Log("Read VRController Data");

        clientStream.Flush();
    }
    
}
