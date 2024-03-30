using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BTManager : MonoBehaviour {
	public GameObject linkButton;					//按鈕預製
	private int linkButtonPos, linkBtnFragment; 	//按鈕出現位置y,每個按鈕的間距
	public RectTransform conBtnPanel;				//scroll view panel
	public Transform conBtnArch;                    //按鈕錨點與父節點

	private string BTlog;

	public Text connectStatus;

	private BTsocket btSoc;

	public string preConnectAddr;

	public static bool DisConnectBtnClick = false;

	public Button DisConnectBtn;

	public GameObject SelectSizePanel;
	bool isSelectSize = false;

    //private static int cnt = 0, recvCnt = 0;
    // Use this for initialization
    void Start ()
	{
		if(BTsocket.isConnectedBLE(Constants.bleMicroBit))
		{
			btSoc = BTsocket.getBTsocket(Constants.bleMicroBit); // get BTSocket
			connectStatus.text = "Connected";
            preConnectAddr = PlayerPrefs.GetString("preConnectBLE");                                                                                  
            DisConnectBtn.interactable = true;
        }
		else
		{
            linkButtonPos = 100;
            linkBtnFragment = 100;
            connectStatus.text = "";
			DisConnectBtn.interactable = false;

            preConnectAddr = PlayerPrefs.GetString("preConnectBLE"); // get previous connect address

            // BLE initialization
            //BTsocket.requestAndroidPermission();    //get Location Permission.                       
            btSoc = BTsocket.getNewBTsocket(Constants.bleMicroBit, new BTprofile(Constants.bleMicroBit, true)); //creat new ble connect socket gameobject...
            Invoke(nameof(delayScan), Constants.delayScan); //delay 2s
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
        /*
		if (Input.GetKeyDown(KeyCode.A))	// 測試用
		{
			addPeripheralButton("123","addr");
			conBtnPanel.sizeDelta = new Vector2(0, conBtnPanel.sizeDelta.y + linkBtnFragment);
		}
		*/

		// Return MainMenu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			if(BTsocket.isConnectedBLE(Constants.bleMicroBit) && preConnectAddr != btSoc.bleLinkData.address)
			{
				if(!isSelectSize)
				{
                    SelectSizePanel.SetActive(true);
                }
				else
				{
                    SceneManager.LoadSceneAsync((ushort)Scenes.MainMenu);
                }
            }
			else
			{
                SceneManager.LoadSceneAsync((ushort)Scenes.MainMenu);
            }
        }

	    // 藍芽 Log
        BTlog = btSoc.BTLog;
		//connectStatus.text = BTlog;
		if(BTlog.IndexOf("Initialising bluetooth") != -1)
		{
			connectStatus.text = "Initialising bluetooth";
		}
		else if(BTlog.IndexOf("Starting scan") != -1)
		{
            connectStatus.text = "Starting scan";
        }
		else if (BTlog.IndexOf("Found") != -1)
		{
            connectStatus.text = "Scanning...";
            if (BTsocket.isConnectedBLE(Constants.bleMicroBit))
			{
                connectStatus.text = "Connected";
            }
        }
        else if (BTlog.IndexOf("Connecting to") != -1)
        {
            connectStatus.text = "Connecting...";
        }
        else if (BTlog.IndexOf("Connected!") != -1)
		{
            connectStatus.text = "Connected";
            btSoc.subscribe();
            DisConnectBtn.interactable = true;
            //BluetoothLEHardwareInterface.StopScan();
        }
		else if(BTlog.IndexOf("Disconnect") != -1)
		{
            connectStatus.text = "Disconnected";
        }
		


        //read BLE data... 有新資料時將自動呼叫委派函數，receiveData為收到的新資料
        //讀取頻率將與Update相同，如需更高頻率需自行建立執行緒或使用其他方式
        /*
        btSoc.getReceiveText((receiveData) =>
        {
            //收到新資料時自動執行該處的code，可自行加入所需功能

        });
		*/
    }

    /*
	public void testMethod()
	{
		btSoc.getReceiveText((data) => {recvCnt = int.Parse(data);});
		btSoc.writeCharacteristic("#");
		cnt++;		
		connectHint.text = "傳送資料次數：" + cnt.ToString() + " 回傳次數：" + recvCnt.ToString();
	}
	*/

    // 新增按鈕
    public void addPeripheralButton(string addr, string name) 
	{
		GameObject newPeripheral = Instantiate(linkButton);
		if(preConnectAddr == addr.ToUpper() && !DisConnectBtnClick)
		{
			newPeripheral.GetComponentInChildren<Text>().color = Color.blue;
        }
		newPeripheral.transform.SetParent(conBtnArch);
		newPeripheral.transform.localScale = new Vector3(1, 1, 1);
		newPeripheral.transform.localPosition = new Vector2(0, linkButtonPos);
		newPeripheral.GetComponent<LinkButton>().address = addr;
		newPeripheral.GetComponent<LinkButton>().name = name;
		newPeripheral.GetComponentInChildren<Text>().text = name + "\n" + addr.ToUpper();

		linkButtonPos -= linkBtnFragment;
	}

	// 掃描周圍藍芽裝置，並取得藍芽位址
	private void delayScan()
	{
		btSoc.scan((addr, name) =>
		{
			addPeripheralButton(addr, name); // add Button
            if (preConnectAddr == addr.ToUpper() && !DisConnectBtnClick) // if have previous connect address record
			{
                btSoc.connect(preConnectAddr);
				//Invoke("subscribe", Constants.canSubscribe);
            }
            conBtnPanel.sizeDelta = new Vector2(0, conBtnPanel.sizeDelta.y + linkBtnFragment);
		});
	}

	// 如果按下按鈕，執行連接的動作
	public void connectAct(LinkButton data, System.Action connectedAct = null)
	{		
		btSoc.connect(data.address);

		//-----測試功能-----
        PlayerPrefs.SetString("preConnectBLE", data.address);
		DisConnectBtnClick = false;

        if (connectedAct != null)
			StartCoroutine(waitLoop(connectedAct));
	}

	public IEnumerator waitLoop(System.Action connectedAct)
	{
		while(!BTsocket.isConnectedBLE(Constants.bleMicroBit))
		{
			yield return 0;
		}
		//btSoc.subscribe();
		connectedAct();
		//----------------------------
		/*cnt = 0;
		recvCnt = 0;
		InvokeRepeating("testMethod", 2f, 1f);*/
	}

	/*
    void subscribe()
    {
        //read notify data
        btSoc.subscribe();
    }
	*/

    public void disConnected()
	{
		btSoc.disConnect(); // 斷開以連接裝置
		DisConnectBtnClick = true;
        DisConnectBtn.interactable = false;
        //SceneManager.LoadSceneAsync((ushort)Scenes.BLEConnect);
        Invoke(nameof(ReloadSceneDelay),Constants.reloadSceneDelay);
    }

	private void ReloadSceneDelay()
	{
        SceneManager.LoadSceneAsync((ushort)Scenes.BLEConnect);
    }

    /*
    public void sendBtn(Text sendStr)
    {
        btSoc.writeCharacteristic(sendStr.text + "#");
    }
	*/

    public void Btn3x3()
    {
        PlayerPrefs.SetInt("Size", 3);
		if(!isSelectSize) SceneManager.LoadSceneAsync((ushort)Scenes.MainMenu);
		else SelectSizePanel.SetActive(false);
    }
    public void Btn4x4()
	{
        PlayerPrefs.SetInt("Size", 4);
        if (!isSelectSize) SceneManager.LoadSceneAsync((ushort)Scenes.MainMenu);
		else SelectSizePanel.SetActive(false);
    }

	public void ShowSizePanel()
	{
        SelectSizePanel.SetActive(true);
		isSelectSize = true;
    }
}
