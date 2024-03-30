using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ViewVoc : MonoBehaviour
{
    public static int index; // 記錄使用者上一次看到的單字

    [Header("Show English & Chinese")]
    public Text EnglishText; // 顯示英文
    public Text ChineseText; // 顯示中文
    //public Button PreBtn;    // 顯示上一個單字
    //public Button NextBtn;   // 顯示下一個單字

    [Header("Turn The Page Btn")]
    public GameObject PreBtn;    // 顯示上一個單字
    public GameObject NextBtn;   // 顯示下一個單字

    [Header("Show Star")]
    public GameObject QuestionMark;
    public GameObject[] Star = new GameObject[5];
    

    void Start()
    {
        index = PlayerPrefs.GetInt("Page");
        ShowVoc();
    }

    void Update()
    {
        BtnEnable();
        VocFamiliarity();
    }

    public void ShowNextVoc()
    {
        index++;
        PlayerPrefs.SetInt("Page", index);
        ShowVoc();
    }

    public void ShowPreVoc()
    {
        index--;
        PlayerPrefs.SetInt("Page", index);
        ShowVoc();
    }

    public void ShowVoc()
    {
        EnglishText.text = GetVoc.Voc[index].English;
        ChineseText.text = GetVoc.Voc[index].Chinese;
    }

    void VocFamiliarity()
    {
        for (int i = 0; i < 5; i++)
        {
            Star[i].SetActive(false);
        }
        QuestionMark.SetActive(false);

        string Voc = EnglishText.text.ToUpper();
        if (PlayerPrefs.GetInt(Voc + "Total") > 0)
        {
            for(int i=0;i< Level(Voc); i++)
            {
                Star[i].SetActive(true);
            }
        }
        else
        {
            QuestionMark.SetActive(true);
        }
    }

    int Level(string Voc)
    {
        Voc = Voc.ToUpper();
        int StartNum = 0;
        if (PlayerPrefs.GetInt(Voc + "Total") == 0) return StartNum;

        float Accuracy = (float)PlayerPrefs.GetInt(Voc + "Correct") / PlayerPrefs.GetInt(Voc + "Total");

        if (Accuracy >= 0.8) StartNum = 5;
        else if (Accuracy >= 0.6) StartNum = 4;
        else if (Accuracy >= 0.4) StartNum = 3;
        else if (Accuracy >= 0.2) StartNum = 2;
        else if (Accuracy >= 0) StartNum = 1;

        return StartNum;
    }

    void BtnEnable()
    {
        if (index < 1)
        {
            PreBtn.SetActive(false);
        }
        else if (index > Constants.VocNum - 2)
        {
            NextBtn.SetActive(false);
        }
        else
        {
            PreBtn.SetActive(true);
            NextBtn.SetActive(true);
        }
    }
}
