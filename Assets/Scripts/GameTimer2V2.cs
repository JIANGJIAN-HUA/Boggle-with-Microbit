using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer2V2 : MonoBehaviour
{
    public int MinuteCount;
    public int SecondCount;
    public float MilliCount;

    public Text MinuteBox;
    public Text SecondBox;
    public GameObject GameFinishPanel;

    public Text TextAPlayerScore;
    public Text TextBPlayerScore;
    public Text TextResult;

    public AudioSource BiBiAudio;

    int AScore,BScore;

    bool isnotGameFinish;

    Audio Music;

    void Awake()
    {
        Music = FindObjectOfType<Audio>();
    }

    void Start()
    {
        isnotGameFinish = true;
        MinuteBox.text = "3";
        SecondBox.text = "00";
        MinuteCount = 3;
        SecondCount = 0;
    }
    void Update()
    {
        MilliCount += Time.deltaTime * 10;
        if (MilliCount >= 10)
        {
            MilliCount = 0;
            SecondCount -= 1;

            if (MinuteCount == 0 && (SecondCount > 0 && SecondCount < 10))
            {
                MinuteBox.color = SecondBox.color = Color.red;
                BiBiAudio.Play();
            }
        }

        if (SecondCount <= -1)
        {
            SecondCount = 59;
            MinuteCount -= 1;
        }

        if (MinuteCount <= 0 && SecondCount <= 0)
        {
            MinuteBox.text = "0:";
            SecondBox.text = "00";
            if (isnotGameFinish)
            {
                isnotGameFinish= false;
                Music.StopBG();
                Music.PlayGF();
                Time.timeScale = 0;
            }
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
            GameFinishPanel.SetActive(true);
        }

        if (SecondCount <= 9)
        {
            SecondBox.GetComponent<Text>().text = "0" + SecondCount;
        }
        else
        {
            SecondBox.GetComponent<Text>().text = "" + SecondCount;
        }

        MinuteBox.GetComponent<Text>().text = MinuteCount + ":";
        
    }
}
