using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class LocationPermission : MonoBehaviour
{
    [Header("Location Policy Panel")]
    public GameObject LPP;
    public GameObject WarningPanel;

    [Header("BTManager")]
    public GameObject BTM;
    void Start()
    {
#if PLATFORM_ANDROID
        if (Permission.HasUserAuthorizedPermission(Permission.CoarseLocation) && Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            LPP.SetActive(false);
            BTM.SetActive(true);
        }
        else
        {
            BTM.SetActive(false);
        }
#endif
    }
    public void Allow()
    {
        BTsocket.requestAndroidPermission();    //get Location Permission.
        BTM.SetActive(true);
        LPP.SetActive(false);
    }

    public void Deny()
    {
        WarningPanel.SetActive(true);
    }

    public void Return()
    {
        WarningPanel.SetActive(false);
    }

    public void Exit()
    {
        SceneManager.LoadSceneAsync((ushort)Scenes.MainMenu);
    }

    public void PrivacyLink()
    {
        Application.OpenURL("https://sites.google.com/view/put-it-together-privacy/english");
    }
}
