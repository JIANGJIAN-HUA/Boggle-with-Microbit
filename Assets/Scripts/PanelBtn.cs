using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PanelBtn : MonoBehaviour
{
    private BTsocket btSoc;

    public GameObject GamePausePanel;
    public GameObject ViewNoAnsPanel;
    public GameObject GameFinishPanel;
    public GameObject CountDownPanel;
    public GameObject BattleScore;

    public GameObject ShowResultBtn;

    public Text TextAPlayerScore;
    public Text TextBPlayerScore;
    public Text TextResult;
    int AScore, BScore;

    public GameObject GameSingle;
    public GameObject GameBattle;

    Audio Music;

    //public bool isClickContBtn = false;
    //float SendDelayTimer = 0f;

    void Awake()
    {
        Music = FindObjectOfType<Audio>();
        btSoc = BTsocket.getBTsocket(Constants.bleMicroBit);
    }

    void Start()
    {
        /*
        if (PlayerPrefs.GetInt("Timer") == 1) ShowResultBtn.SetActive(false);
        else ShowResultBtn.SetActive(true);
        */
    }

    public void MainMenu()
    {
        Music.StopBG();
        ClearBoggleRecord();
        PlayerPrefs.SetInt("Restart", 0);
        SceneManager.LoadSceneAsync((ushort)Scenes.MainMenu);
    }

    public void Boggle()
    {
        Music.StopBG();
        ClearBoggleRecord();
        PlayerPrefs.SetInt("Restart", 0);
        SceneManager.LoadSceneAsync((ushort)Scenes.Boggle);
    }

    public void Restart()
    {
        Music.StopBG();
        ClearBoggleRecord();
        Time.timeScale = 1;
        PlayerPrefs.SetInt("Restart", 1);
        SceneManager.LoadSceneAsync((ushort)Scenes.Boggle);
    }

    public void Continue()
    {
        btSoc.writeCharacteristic("Cont\n");
        Music.UnPauseBG();
        Time.timeScale = 1;
        GamePausePanel.SetActive(false);

        /*
        if(CountDownPanel.activeInHierarchy)
        {
            GamePausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            isClickContBtn = true;
            SendDelayTimer = Time.realtimeSinceStartup;
        }
        */
    }

    public void CloseShNoAnsPanel()
    {
        ViewNoAnsPanel.SetActive(false);
        if (PlayerPrefs.GetInt("Mode") == 2)
        {
            BattleScore.SetActive(true);
        }
    }

    public void ShowShNoAnsPanel()
    {
        ViewNoAnsPanel.SetActive(true);
        if (PlayerPrefs.GetInt("Mode") == 2)
        {
            GameBattle.GetComponent<Game2V2>().ShowNoAnsVoc();
            BattleScore.SetActive(false);
        }
        else
        {
            GameSingle.GetComponent<Game>().ShowNoAnsVoc();
        }
    }

    public void ResultBtn()
    {
        Music.StopBG();
        Music.PlayGF();
        GamePausePanel.SetActive(false);
        GameFinishPanel.SetActive(true);
        if (PlayerPrefs.GetInt("Mode") == 2)
        {
            AScore = Int32.Parse(TextAPlayerScore.text);
            BScore = Int32.Parse(TextBPlayerScore.text);
            if (AScore < BScore)
            {
                TextResult.text = "B Player win !";
            }
            else if (AScore == BScore)
            {
                TextResult.text = "Draw";
            }
            else
            {
                TextResult.text = "A Player win !";
            }
        }
        Time.timeScale = 0; 
    }

    void ClearBoggleRecord()
    {
        btSoc.writeCharacteristic("R\n");
        Time.timeScale = 1;
    }
    
    void Update()
    {

        if (CountDownPanel.activeInHierarchy)
        {
            ShowResultBtn.SetActive(false);
        }
        else
        {
            if (PlayerPrefs.GetInt("Timer") == 1) ShowResultBtn.SetActive(false);
            else ShowResultBtn.SetActive(true);
        }

        /*
        if (isClickContBtn && GamePausePanel.activeInHierarchy)
        {
            if (Time.realtimeSinceStartup < SendDelayTimer + 0.5f)
            {
                BTManager.btSoc.writeCharacteristic("Cont\n");
                SendDelayTimer = Time.realtimeSinceStartup;
            }
        }
        */
    }
    
}
