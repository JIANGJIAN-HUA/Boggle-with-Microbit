using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBLEStatus : MonoBehaviour
{
    public GameObject ShowBLEDisConnected;
    void Update()
    {
        if (BTsocket.isConnectedBLE(Constants.bleMicroBit))
        {
            ShowBLEDisConnected.SetActive(false);
        }
        else
        {
            ShowBLEDisConnected.SetActive(true);
        }
    }
}
