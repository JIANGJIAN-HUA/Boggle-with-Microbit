using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnManager : MonoBehaviour
{
    private BTsocket btSoc;
    public GameObject GamePausePanel;

    Audio Music;
    //PanelBtn PBtn;

    /*
    float SendDelayTimer = 0f;
    bool isReceiveStop = false;
    */

    void Awake()
    {
        Music= FindObjectOfType<Audio>();
        btSoc = BTsocket.getBTsocket(Constants.bleMicroBit);
        //PBtn= FindObjectOfType<PanelBtn>();
    }

    void FixedUpdate()
    {
        // Game Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            GamePausePanel.SetActive(true);
            Music.PauseBG();
            btSoc.writeCharacteristic("Stop\n");

            /*
            isReceiveStop = false;
            SendDelayTimer = Time.realtimeSinceStartup;
            */
        }
    }

    /*
    void Update()
    {
        if(GamePausePanel.activeInHierarchy)
        {
            if (Time.realtimeSinceStartup > SendDelayTimer+0.5f && !isReceiveStop)
            {
                BTManager.btSoc.writeCharacteristic("Stop\n");
                SendDelayTimer = Time.realtimeSinceStartup;
            }

            BTManager.btSoc.getReceiveText((receiveData) =>
            {
                if(receiveData == "Stop")
                {
                    isReceiveStop= true;
                }
                else if(receiveData == "Cont" && isReceiveStop)
                {
                    PBtn.isClickContBtn = false;
                    Music.UnPauseBG();
                    Time.timeScale = 1;
                    GamePausePanel.SetActive(false);
                }
            });
            
        }
    }
    */
}
