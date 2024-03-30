using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public int MinuteCount;
    public int SecondCount;
    public float MilliCount;

    public Text MinuteBox;
    public Text SecondBox;
    public GameObject GameFinishPanel;
    public AudioSource BiBiAudio;

    Audio Music;
    Game BoggleGame;

    bool isnotGameEnd;

    void Awake()
    {
        Music = FindObjectOfType<Audio>();
        BoggleGame = FindObjectOfType<Game>();
    }

    void Start()
    {
        isnotGameEnd = true;
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

        if(MinuteCount <= 0 && SecondCount <= 0)
        {
            //MinuteBox.text = "0:";
            //SecondBox.text = "00";
            if(isnotGameEnd)
            {
                isnotGameEnd= false;
                Music.StopBG();
                Music.PlayGF();
                Time.timeScale= 0;
            }
            BoggleGame.ShowNoAnsVoc();
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
