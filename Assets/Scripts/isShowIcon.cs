using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isShowIcon : MonoBehaviour
{
    public GameObject bluecloud, pinkcloud;
    void Awake()
    {
        float SetScreenRatio = (float)Math.Round((double)2000/1200,2, MidpointRounding.AwayFromZero);
        float UserScreenRatio = (float)Math.Round((double)Screen.height / Screen.width, 2, MidpointRounding.AwayFromZero);

        if(SetScreenRatio == UserScreenRatio)
        {
            bluecloud.SetActive(false);
            pinkcloud.SetActive(false);
        }
    }
}
