using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDialogBox : MonoBehaviour
{
    private BTsocket btSoc;
    public GameObject ExitPanel;

    void Awake()
    {
        btSoc = BTsocket.getBTsocket(Constants.bleMicroBit);
    }
    public void YesBtn()
    {
#if UNITY_EDITOR_WIN
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        if (BTsocket.isConnectedBLE(Constants.bleMicroBit))
        {
            btSoc.disConnect();
        }

        Application.Quit();
    }

    public void NoBtn()
    {
        ExitPanel.SetActive(false);
    }
}
