using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelect : MonoBehaviour
{
    public GameObject SingleGame;
    public GameObject BattleGame;
    public GameObject SelectMode;
    public GameObject SelectModePanel;

    public void TimerLimitBtn()
    {
        PlayerPrefs.SetInt("Timer",1);
        SelectModePanel.SetActive(true);
    }

    public void NoTimerLimitBtn()
    {
        PlayerPrefs.SetInt("Timer", 0);
        SelectModePanel.SetActive(true);
    }

    public void SingleBtn()
    {
        SelectMode.SetActive(false);
        BattleGame.SetActive(false);
        SingleGame.SetActive(true);
        //BTManager.btSoc.writeCharacteristic("1\n");
        PlayerPrefs.SetInt("Mode", 1);
    }

    public void BattleBtn()
    {
        SelectMode.SetActive(false);
        SingleGame.SetActive(false);
        BattleGame.SetActive(true);
        PlayerPrefs.SetInt("Mode", 2);
    }

    void Start()
    {
        Time.timeScale = 1;
        
        if (PlayerPrefs.GetInt("Restart") == 1)
        {
            if(PlayerPrefs.GetInt("Mode") == 1)
            {
                SelectMode.SetActive(false);
                BattleGame.SetActive(false);
                SingleGame.SetActive(true);
            }
            else
            {
                SelectMode.SetActive(false);
                SingleGame.SetActive(false);
                BattleGame.SetActive(true);
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync((ushort)Scenes.MainMenu);
        }
    }
}
