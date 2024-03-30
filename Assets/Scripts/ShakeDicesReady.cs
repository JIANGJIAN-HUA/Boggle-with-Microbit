using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeDicesReady : MonoBehaviour
{
    private BTsocket btSoc;
    public GameObject ReadyBtn;
    public GameObject ShakeDicesPanel;
    public GameObject GameManager;
    public Text HintText;
    public Transform DeviceLogoTran;
    public Animator ShakeDeviceAnim;

    Audio Music;

    void Awake()
    {
        Music = FindObjectOfType<Audio>();
        btSoc = BTsocket.getBTsocket(Constants.bleMicroBit);
    }

    void Start()
    {
        ReadyBtn.SetActive(false);
        GameManager.SetActive(false);
        Music.PlaySD();
    }

    void Update()
    {
        //read BLE data... 有新資料時將自動呼叫委派函數，receiveData為收到的新資料
        //讀取頻率將與Update相同，如需更高頻率需自行建立執行緒或使用其他方式
        btSoc.getReceiveText((receiveData) =>
        {
            if (receiveData == "RD")
            {
                btSoc.writeCharacteristic("RDOK\n");
            }
            else if(receiveData == "RDOK")
            {
                HintText.text = "Ready ?";
                ReadyBtn.SetActive(true);
                StopAnim();
            }
        });
    }

    public void PressBtn()
    {
        Music.StopSD();
        ShakeDicesPanel.SetActive(false);
        GameManager.SetActive(true);
    }

    void StopAnim()
    {
        ShakeDeviceAnim.enabled= false;
        DeviceLogoTran.eulerAngles = new Vector3(0, 0, 0);
    }
}
