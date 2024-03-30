using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkButton : MonoBehaviour 
{
	public string address;
	//public string BLEname;
	// Use this for initialization
	void Start()
	{
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(OnClick);
	}

	private void OnClick()
	{
		//GameObject.Find(Constants.bleMicroBit).GetComponent<BTsocket>().connect(address);		
		//-----測試功能-----
		if(!BTsocket.isConnectedBLE(Constants.bleMicroBit))
		{
			GameObject.Find("BLEManager").GetComponent<BTManager>().connectAct(this, 
			() =>
			{
				this.GetComponentInChildren<Text>().color = Color.blue;
			});
		}
		else
		{
			BTManager.DisConnectBtnClick = true;
			GameObject.Find("BLEManager").GetComponent<BTManager>().disConnected();
			this.GetComponentInChildren<Text>().color = Color.black;
		}
	}
}
