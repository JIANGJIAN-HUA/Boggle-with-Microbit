using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scenes : ushort
{
    MainMenu, Boggle, ViewVoc, BLEConnect
}

public class Screens : MonoBehaviour
{
    private BTsocket btSoc;
    public Button GameBtn;
    public GameObject ExitPanel;
    public Image StartBtnImage;

    void Awake()
    {
        btSoc = BTsocket.getBTsocket(Constants.bleMicroBit);
    }

    void Update()
    {
        if (BTsocket.isConnectedBLE(Constants.bleMicroBit))
        {
            GameBtn.interactable = true;
            StartBtnImage.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            GameBtn.interactable = false;
            StartBtnImage.color = new Color(1f, 1f, 1f, 0.1960784f);
        }
        
        if (Input.GetKey(KeyCode.Escape))
        {
            ExitPanel.SetActive(true);
        }
    }

    public void MainMenuBtn()
    {
        SceneManager.LoadSceneAsync((ushort)Scenes.MainMenu);
    }

    public void BoggleBtn()
    {
        PlayerPrefs.SetInt("Restart", 0);
        FindObjectOfType<ProgressSceneLoader>().LoadScene((ushort)Scenes.Boggle);
        //SceneManager.LoadSceneAsync((ushort)Scenes.Boggle);
    }

    public void ViewVocBtn()
    {
        //FindObjectOfType<ProgressSceneLoader>().LoadScene((ushort)Scenes.ViewVoc);
        SceneManager.LoadSceneAsync((ushort)Scenes.ViewVoc);
    }

    public void BLEConnectBtn()
    {
        //FindObjectOfType<ProgressSceneLoader>().LoadScene((ushort)Scenes.BLEConnect);
        SceneManager.LoadSceneAsync((ushort)Scenes.BLEConnect);
    }

    public void ExitBtn()
    {
    #if UNITY_EDITOR_WIN
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        if(BTsocket.isConnectedBLE(Constants.bleMicroBit))
        {
            btSoc.disConnect();
        }

        Application.Quit();
    }
}
