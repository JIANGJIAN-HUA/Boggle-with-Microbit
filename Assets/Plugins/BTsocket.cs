using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text;
using System;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

/*
 * 該結構用於紀錄已連線資料
 */
public struct BluetoothData
{
	public string name;
	public string address;
	public string service;
	public string rCharacteristic;
	public string wCharacteristic;
	private BTprofile profile;
	//private static string full = "6e40****-b5a3-f393-e0a9-e50e24dcca9e";
	public BluetoothData(string _name = "", string _address = "", BTprofile _profile = new BTprofile())
	{
		name = _name;
		address = _address;
		profile = _profile;
		service = _profile.serviceID; //"0001";
		rCharacteristic = _profile.readChID; //"0002";	//RX
		wCharacteristic = _profile.writeChID; //"0003";	//TX
	}
	public string fullUUID(string uuid)
	{
		return profile.full.Replace("****", uuid);
	}
	public bool isEqual(string uuid1, string uuid2)
	{
		if (uuid1.Length == 4)
		{
			uuid1 = fullUUID(uuid1);
		}
		if (uuid2.Length == 4)
		{
			uuid2 = fullUUID(uuid2);
		}
		return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
	}
}


/*
 * 該結構用於建立BTsocket時，預先設置其內部參數
 * 藍芽名稱(與BTsocket名稱需相同)，其餘參數請參考建構子引數
 */
public struct BTprofile
{
	private string name;
	public string full;
    public string serviceID;
	public string readChID;
	public string writeChID;


	public BTprofile(string _profileName, string _full = "6e40****-b5a3-f393-e0a9-e50e24dcca9e",
		string _service = "0001", string _readCh = "0002", string _writeCh = "0003")
	{
		name = _profileName;
		full = _full;
		serviceID = _service;
		readChID = _readCh;
		writeChID = _writeCh;
		fileSave(this);
	}

	/*
	*	用於優先讀取已儲存檔案的建構子，不覆蓋已儲存的資料
	*	useFile:
	*		true 	=> 使用已儲存的預設檔案
	*		false 	=> 以輸入參數覆蓋預設檔案
	*	其餘部分與預設建構子相同
	 */
	public BTprofile(string _profileName, bool useFile, string _full = "6e40****-b5a3-f393-e0a9-e50e24dcca9e",
		string _service = "0001", string _readCh = "0002", string _writeCh = "0003")
	{
		if(useFile)
		{
			if(isEmpty(_profileName))
			{
				name = _profileName;
				full = _full;
				serviceID = _service;
				readChID = _readCh;
				writeChID = _writeCh;
				fileSave(this);
			}
			else
			{
				name = _profileName;
				full = PlayerPrefs.GetString(name + "BTfull");
				serviceID = PlayerPrefs.GetString(name + "BTservice");
				readChID = PlayerPrefs.GetString(name + "BTreadCh");
				writeChID = PlayerPrefs.GetString(name + "BTwriteCh");
			}
		}
		else
		{
			name = _profileName;
			full = _full;
			serviceID = _service;
			readChID = _readCh;
			writeChID = _writeCh;
			fileSave(this);
		}
		
	}
	public static bool isEmpty(string name)
	{
		string full = PlayerPrefs.GetString(name + "BTfull");
		string serviceID = PlayerPrefs.GetString(name + "BTservice");
		string readChID = PlayerPrefs.GetString(name + "BTreadCh");
		string writeChID = PlayerPrefs.GetString(name + "BTwriteCh");
		if (full == "" || serviceID == "" || readChID == "" || writeChID == "")
		{
			return true;
		}
		return false;
	}

	public void fileReLoad()
	{
		full = PlayerPrefs.GetString(name + "BTfull");
		serviceID = PlayerPrefs.GetString(name + "BTservice");
		readChID = PlayerPrefs.GetString(name + "BTreadCh");
		writeChID = PlayerPrefs.GetString(name + "BTwriteCh");
	}
	/*public void initialize()
	{
		PlayerPrefs.SetString(name + "BTfull", full);
		PlayerPrefs.SetString(name + "BTservice", serviceID);
		PlayerPrefs.SetString(name + "BTreadCh", readChID);
		PlayerPrefs.SetString(name + "BTwriteCh", writeChID);
		fileReLoad();
	}*/

	public static void fileSave(BTprofile _profile)
	{
		PlayerPrefs.SetString(_profile.name + "BTfull", _profile.full);
		PlayerPrefs.SetString(_profile.name + "BTservice", _profile.serviceID);
		PlayerPrefs.SetString(_profile.name + "BTreadCh", _profile.readChID);
		PlayerPrefs.SetString(_profile.name + "BTwriteCh", _profile.writeChID);
	}
}


public class BTsocket : MonoBehaviour {	
	public BTprofile profile;
	private static bool isInit = false;
	private static bool scanState = false;
	private bool isConnected = false;

	private bool disConnectFlag = true;
	private bool BTStatus = false;

	public bool bleActive
	{
		get{
			return BTStatus;
		}
	}

	private Dictionary<string, BluetoothData> peripheralList;
	private BluetoothData linkData;
	public BluetoothData bleLinkData
	{
		get{
			return linkData;
		}
	}

	private bool readFound = false;
	private bool writeFound = false;
	private string receiveText = "";
	public string BTLog = "";

    /*
     * 靜態初始化函式
     * 傳入引數
     * 1. 藍芽名稱(自訂)
     * 2. 藍芽參數資料(該資料中藍芽名稱須與引數1相同)
     * 回傳
     * Btsocket物件
     */
	public static BTsocket getNewBTsocket(string btName, BTprofile _profile)
	{		
		BTsocket btSoc;
		if(GameObject.Find(btName) != null)
		{
			btSoc = GameObject.Find(btName).GetComponent<BTsocket>();
			btSoc.profile = new BTprofile(btName, _profile.full, _profile.serviceID, _profile.readChID, _profile.writeChID);
			return btSoc;
		}
		GameObject btObj = new GameObject(btName);
		btSoc = btObj.AddComponent<BTsocket>();	
		btSoc.profile = new BTprofile(btName, _profile.full, _profile.serviceID, _profile.readChID, _profile.writeChID);
		btSoc.initialize();
		Debug.Log(btSoc.profile.full);
		DontDestroyOnLoad(btObj);
		return btSoc;
	}

    /*
     * 取得已建立的BTsocket物件
     * 傳入引數
     * 1.藍芽名稱
     * 回傳
     * 1.存在該名稱的BTsocket:該藍芽名稱對應的BTsocket物件
     * 2.不存在該名稱的BTsocket:null
     */
    public static BTsocket getBTsocket(string btName)
    {
        try
        {
            return GameObject.Find(btName).GetComponent<BTsocket>();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("BLE : " + btName + " Not found!\n" + e);
            return null;
        }
    }
    

    /*
     * 檢查BTsocket是否已連線
     * 傳入引數
     * 1.藍芽名稱
     * 回傳
     * 1.已連線:true
     * 2.未連線或不存在該名稱的BTsocket:false
     */
	public static bool isConnectedBLE(string btName)
	{
		if(getBTsocket(btName) == null)
			return false;
		return GameObject.Find(btName).GetComponent<BTsocket>().bleActive;
	}

	private void initialize()
	{
		requestAndroidPermission();
		if (isInit)
			return;
		BTLog = "Initialising bluetooth\n";
		BluetoothLEHardwareInterface.Initialize(true, false, () => {; }, (error) => {; });
		isInit = true;
	}

    /*
     * 啟動掃描周邊設備
     * 傳入引數-void
     * 回傳-void
     */
	public void scan()
	{
		BTLog = "Starting scan\n";
		scanState = true;
		peripheralList = new Dictionary<string, BluetoothData>();
		BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null,
			(address, name) => 
			{ 
				addPeripheral(address, name);
			},
			(address, name, rssi, advertisingInfo) => {; });
	}


    /*
     * 啟動掃描周邊設備，並在掃描到新設備時執行委派
     * 傳入引數
     * 1.委派函數(藍芽位址, 藍芽名稱)，掃描到新設備時執行
     * 回傳-void
     */
	public void scan(System.Action<string, string> newPeripheralAction)
	{
		BTLog = "Starting scan\n";
		scanState = true;
		peripheralList = new Dictionary<string, BluetoothData>();
		BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null,
			(address, name) => 
			{ 
				addPeripheral(address, name);
				newPeripheralAction(address, name);
			},
			(address, name, rssi, advertisingInfo) => {; });
	}

	private void addPeripheral(string address, string name)
	{
		BTLog += ("Found " + address + "\n");
		if (!peripheralList.ContainsKey(address))
		{			
			peripheralList[address] = new BluetoothData(name, address, profile);
		}
	}


    /*
     * 藍芽開始連線
     * 傳入引數
     * 1.藍芽位址
     * 回傳-void
     */
	public void connect(string addr)
	{
		if (isConnected)
			return;
		if (!peripheralList.ContainsKey(addr))
		{
			if(!scanState)
				scan();
			return;
		}
		BTStatus = false;
		linkData = new BluetoothData("", "", profile);
		isConnected = false;
		readFound = false;
		writeFound = false;
		BTLog = "Connecting to \n" + addr + "\n";
        BluetoothLEHardwareInterface.ConnectToPeripheral(addr, (address) => { isConnected = true; disConnectFlag = false;},
			(address, serviceUUID) => {; },
		   	(address, serviceUUID, characteristicUUID) =>
		   	{
				// discovered characteristic
				if (linkData.isEqual(serviceUUID, peripheralList[address].service))
				{				   
					linkData.name = peripheralList[address].name;
					linkData.address = address;
					linkData.service = peripheralList[address].service;
					if (linkData.isEqual(characteristicUUID, peripheralList[address].rCharacteristic))
					{
						readFound = true;
						linkData.rCharacteristic = peripheralList[address].rCharacteristic;
						BTLog += "readTrue \n";
					}
					if (linkData.isEqual(characteristicUUID, peripheralList[address].wCharacteristic))
					{
						writeFound = true;
						linkData.wCharacteristic = peripheralList[address].wCharacteristic;
						BTLog += "writeTrue \n";
					}

					if (readFound && writeFound)
					{
						Invoke("delayConnect", 1f);
						BTLog = address + "\nConnected!\n";
						receiveText = "";
						BluetoothLEHardwareInterface.StopScan();
						scanState = false;
					}
					
				}
			},
			(address) =>
			{
                if(disConnectFlag)
					return;                
				StartCoroutine(reConnectAct(address));
            });
	}

	private void delayConnect()
	{
		BTStatus = true;
	}

	/*
	 * 非預期斷線，自動重新連線功能(使用協程)
	 * !!!建議不要更改，重連後會自動啟動subscribe功能!!!
	 * 傳入引數-連線位址
	 * 回傳-void
	 */
	public IEnumerator reConnectAct(string addr)
	{		
		scanState = false;
		BTStatus = false;
		isConnected = false;				                					
		BTLog = "re connecting...";
		connect(addr);
		while(!BTStatus)
		{
			yield return new WaitForSeconds(1f);
		}
		subscribe();
	}

    /*
     * 從wCharacteristic(建立BTsocket時的BTprofile中設定)送出字串資料
     * 傳入引數-字串資料
     * 回傳-void
     */
    public void writeCharacteristic(string sendStr)
    {
        if (!BTStatus)
            return;
        BTLog = "WriteStart!\n";

        byte[] sendData = Encoding.UTF8.GetBytes(sendStr);
        BluetoothLEHardwareInterface.WriteCharacteristic(
            linkData.address, linkData.fullUUID(linkData.service), linkData.fullUUID(linkData.wCharacteristic),
            sendData, sendData.Length,
            false, (deviceAddress) => { BTLog += "send:" + sendStr + "\n"; });
    }

    /*
     * 啟動notify模式訂閱讀取功能
     * (同步讀取，但資料處理請使用getReceiveText())
     * 傳入引數-void
     * 回傳-void
     */
    public void subscribe()
	{
		if (!BTStatus)
			return;
		BTLog = "SubscribeStart!\n";
		BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(
			linkData.address, linkData.fullUUID(linkData.service), linkData.fullUUID(linkData.rCharacteristic),
			(deviceAddress, notification) => {; },
			(deviceAddress2, characteristic, data) =>
			{				
				if (deviceAddress2.CompareTo(linkData.address) == 0) //讀資料
				{
					if (data.Length > 0)
					{
						string s = Encoding.UTF8.GetString(data); ///Byte to string
						receiveText = s;
					}
				}
			});
	}


    /*
     * 啟動rCharacteristic(建立BTsocket時的BTprofile中設定)讀取功能
     * (同步讀取，但資料處理請使用getReceiveText())
     * 傳入引數-void
     * 回傳-void
     */
    public void readCharacteristic()
	{
		if (!BTStatus)
			return;
		BTLog = "ReadStart!\n";
		BluetoothLEHardwareInterface.ReadCharacteristic(
			linkData.address, linkData.fullUUID(linkData.service), linkData.fullUUID(linkData.rCharacteristic),
			(deviceAddress, data) => 
			{
				if (data.Length > 0)
				{
					string s = Encoding.UTF8.GetString(data); ///Byte to string
					receiveText = s;
				}
			});
	}

    
    /*
     * 取得讀取後的資料(回調函數，需傳入ref string buffer)
     * 調用時將最新取得資料寫至傳入的buffer中，並清空內部buffer
     * 若無及時調用，則新資料將覆蓋舊資料
     * 內部無新資料則傳入buffer不會改變
     * 傳入引數
     * 1.string暫存變數(ref string)
     * 回傳-void
     */
	public void getReceiveText(ref string buffer)   //讀取資料後的操作
	{
		if(receiveText.Length == 0)
			return;
		buffer = receiveText;
		BTLog += "read:" + receiveText + "\n";
		receiveText = "";
	}

    /*
     * 取得讀取後的資料
     * 調用時若有新資料則執行委派函數，無新資料則不動作
     * 若無及時調用，則新資料將覆蓋舊資料
     * 傳入引數
     * 1.委派函數(讀取到的字串)
     * 回傳-void
     */
    public void getReceiveText(System.Action<string> receiveAction)   //讀取資料後的操作
	{
		if(!(receiveText.Length > 0))
			return;
		receiveAction(receiveText);
		BTLog += "read:" + receiveText + "\n";
		receiveText = "";
	}


    /*
     * 中斷連線
     * 傳入引數-void
     * 回傳-void
     */
	public void disConnect()
	{
		BluetoothLEHardwareInterface.StopScan();
		scanState = false;
		if (!isConnected)
			return;
		disConnectFlag = true;
		BluetoothLEHardwareInterface.UnSubscribeCharacteristic(
			linkData.address, linkData.fullUUID(linkData.service), linkData.fullUUID(linkData.rCharacteristic),
			(deviceAddress) => {BTLog = "UnSubscribe " + deviceAddress + " \n";});

		BluetoothLEHardwareInterface.DisconnectPeripheral(linkData.address,
			(deviceAddress) =>
			{
				BTLog = "Disconnect " + deviceAddress + " \n";
				isConnected = false;
				BTStatus = false;
			});
	}

	void OnDestroy()
    {
        disConnect();
    }


    /*
     * 請求Android所需權限
     * 傳入引數-void
     * 回傳-void
     */
	public static void requestAndroidPermission()
	{
		#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
        {
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        #endif
    }

}
