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
    private static TCP_SERVER instance = null;

    TcpListener serverSocket;
    TcpClient clientSocket;
    NetworkStream clientStream;
    [SerializeField] int counter = 0;
    [SerializeField] int port;
    [SerializeField] Text debugText;
    [SerializeField] Text debugText2;
    [SerializeField] ModeManager modeManager;
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

    public static TCP_SERVER GetInstance() => instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

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

        if (currentFunctionType != modeManager.GetCurrentModeMangerFunctionType())
        {
            modeManager.ModeChange(currentFunctionType);
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
                    currentFunctionUpdated = false;
                }
                break;
            case 2:
                debugText.text += "\nmirroring Data"
                                  + "\nbyte Len : " + DataStructs.mirroringData.bytesLen.ToString()
                                  + "\nscreen width : " + DataStructs.mirroringData.screenWidth.ToString()
                                  + "\nscreen height : " + DataStructs.mirroringData.screenHeight.ToString();
                if (currentFunctionUpdated)
                {
                    byteToRawTexture.LoadPNG(DataStructs.mirroringImageData);
                    currentFunctionUpdated = false;
                }
                break;
            case 3:
                if (currentFunctionUpdated)
                {
                    currentFunctionUpdated = false;
                }
                break;
            case 4:
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

    public void ChangeMobileMode(int changeFunctionType)
    {
        if (changeFunctionType == currentFunctionType)
            return;

        if (clientStream == null)
            return;
        clientStream.WriteTimeout = 500;
        byte[] functionTypeBytes = BitConverter.GetBytes(changeFunctionType);
        clientStream.Write(functionTypeBytes, 0, functionTypeBytes.Length);
        currentFunctionType = changeFunctionType;
    }
    
    public void ChangeMobileMode(FunctionTypes changeFunctionType)
    {
        if ((int) changeFunctionType == currentFunctionType)
            return;

        if (clientStream == null)
            return;
        
        clientStream.WriteTimeout = 500;
        byte[] functionTypeBytes = BitConverter.GetBytes((int) changeFunctionType);
        clientStream.Write(functionTypeBytes, 0, functionTypeBytes.Length);
        currentFunctionType = (int) changeFunctionType;
    }


    #region ReadDataFromMobile

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
                    case 4:
                        ReadFunctionD();
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
        
        DataStructs.mirroringData.bytesLen = BitConverter.ToInt32(lengthBytes, 0);

        clientStream.Read(lengthBytes, 0, lengthBytes.Length);
        Array.Reverse(lengthBytes);
        DataStructs.mirroringData.screenWidth = BitConverter.ToInt32(lengthBytes, 0);
        
        clientStream.Read(lengthBytes, 0, lengthBytes.Length);
        Array.Reverse(lengthBytes);
        DataStructs.mirroringData.screenHeight = BitConverter.ToInt32(lengthBytes, 0);

        byte[] textureRawData = new byte[DataStructs.mirroringData.bytesLen];

        int readData = 0;
        while (readData < DataStructs.mirroringData.bytesLen)
        {
            readData += clientStream.Read(textureRawData, readData, textureRawData.Length - readData);
        }
        
        DataStructs.mirroringImageData = textureRawData;

        counter += 1;
        Debug.Log("Read Texture Length : " + readData);
        clientStream.Flush();
        currentFunctionUpdated = true;
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
        currentFunctionUpdated = true;
    }
    
    void ReadFunctionD()
    {
        byte[] dataBytes = new byte[Marshal.SizeOf<DataStructs.VRKeyboardStruct>()];
        clientStream.Read(dataBytes, 0, dataBytes.Length);
        DataStructs.VRKeyboardStruct tempData = DataStructs.ByteToStruct<DataStructs.VRKeyboardStruct>(dataBytes);
        DataStructs.UpdateTouchInfo(tempData);
        counter += 1;
        /*
        Debug.Log("Read VRKeyboard Data Read");
        foreach (DataStructs.VRKeyboardStruct testDebug in DataStructs.vrKeyboardBuffor)
        {
            Debug.Log("Data Sturct buffer : " + testDebug.touchID);
        }
        */
        clientStream.Flush();
        currentFunctionUpdated = true;
    }

    #endregion
}
