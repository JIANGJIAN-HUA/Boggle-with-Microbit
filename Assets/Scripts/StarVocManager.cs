using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class StarVocManager : MonoBehaviour
{
    int OneStar = 0, TwoStar = 0, ThreeStar = 0, FourStar = 0, FiveStar = 0;

    enum Star { OneVoc=1, TwoVoc=2, ThreeVoc=3, FourVoc=4, FiveVoc=5}

    struct Vocabulary
    {
        public string English, Chinese;
    }

    Vocabulary[] OneStarVoc, TwoStarVoc, ThreeStarVoc, FourStarVoc, FiveStarVoc, Voc;

    int VocSize;

    public GameObject StarPanel;
    public GameObject StarSelect;
    public GameObject ViewVoc;

    public string AudioName;
    bool isnotAddAudioSource = true;

    [Header("Audio Stuff")]
    public AudioSource Audio;
    public AudioClip SaveAudio;
    public string SoundPath;

    [Header("Star")]
    public GameObject[] Stars = new GameObject[5];

    [Header("Index")]
    int ShowVocindex = 0;
    int SelectVocIndex;

    [Header("Button")]
    public GameObject PreButton;
    public GameObject NextButton;

    [Header("VocText")]
    public Text EngText;
    public Text ChiText;

    [Header("HintShow")]
    public GameObject HintPanel;
    float HintShowTimer = 0;

    void Start()
    {
        for(int i=0;i<GetVoc.Voc.Length;i++)
        {
            switch (Level(GetVoc.Voc[i].English))
            {
                case 1:
                    OneStar++;
                    break;
                case 2:
                    TwoStar++; 
                    break;
                case 3:
                    ThreeStar++;
                    break;
                case 4:
                    FourStar++;
                    break;
                case 5:
                    FiveStar++;
                    break;
            }
        }

        OneStarVoc = new Vocabulary[OneStar];
        TwoStarVoc = new Vocabulary[TwoStar];
        ThreeStarVoc = new Vocabulary[ThreeStar];
        FourStarVoc = new Vocabulary[FourStar];
        FiveStarVoc = new Vocabulary[FiveStar];

        OneStar = 0; TwoStar = 0; ThreeStar = 0; FourStar = 0; FiveStar = 0;

        for (int i = 0; i < GetVoc.Voc.Length; i++)
        {
            switch (Level(GetVoc.Voc[i].English))
            {
                case 1:
                    OneStarVoc[OneStar].English = GetVoc.Voc[i].English;
                    OneStarVoc[OneStar++].Chinese = GetVoc.Voc[i].Chinese;
                    break;
                case 2:
                    TwoStarVoc[TwoStar].English = GetVoc.Voc[i].English;
                    TwoStarVoc[TwoStar++].Chinese = GetVoc.Voc[i].Chinese;
                    break;
                case 3:
                    ThreeStarVoc[ThreeStar].English = GetVoc.Voc[i].English;
                    ThreeStarVoc[ThreeStar++].Chinese = GetVoc.Voc[i].Chinese;
                    break;
                case 4:
                    FourStarVoc[FourStar].English = GetVoc.Voc[i].English;
                    FourStarVoc[FourStar++].Chinese = GetVoc.Voc[i].Chinese;
                    break;
                case 5:
                    FiveStarVoc[FiveStar].English = GetVoc.Voc[i].English;
                    FiveStarVoc[FiveStar++].Chinese = GetVoc.Voc[i].Chinese;
                    break;
            }
        }

        BtnEnable(ShowVocindex);
        ShowVoc(ShowVocindex);
    }

    
    void Update()
    {
        // ShowHintPanel 顯示時間
        /* 物件(HintPanel)在Hierarchy視窗下實際的Active狀態
         true -> Active on , false -> Active off */
        if (HintPanel.activeInHierarchy == true)
        {
            HintShowTimer += Time.deltaTime;
        }

        if(HintShowTimer > 3f)
        {
            HintPanel.SetActive(false);
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

    public void OneStarBtn()
    {
        if(OneStar == 0)
        {
            ShowHintPanel();
        }
        else
        {
            SelectVocIndex = (int)Star.OneVoc;
            ShowStars();
            LoadVoc();
            StarSelect.SetActive(false);
            ViewVoc.SetActive(true);
        }
    }

    public void TwoStartBtn()
    {
        if (TwoStar == 0)
        {
            ShowHintPanel();
        }
        else
        {
            SelectVocIndex = (int)Star.TwoVoc;
            ShowStars();
            LoadVoc();
            StarSelect.SetActive(false);
            ViewVoc.SetActive(true);
        }
    }

    public void ThreeStarBtn()
    {
        if (ThreeStar == 0)
        {
            ShowHintPanel();
        }
        else
        {
            SelectVocIndex = (int)Star.ThreeVoc;
            ShowStars();
            LoadVoc();
            StarSelect.SetActive(false);
            ViewVoc.SetActive(true);
        }
    }

    public void FourStarBtn()
    {
        if (FourStar == 0)
        {
            ShowHintPanel();
        }
        else
        {
            SelectVocIndex = (int)Star.FourVoc;
            ShowStars();
            LoadVoc();
            StarSelect.SetActive(false);
            ViewVoc.SetActive(true);
        }
    }

    public void FiveStarBtn()
    {
        if(FiveStar == 0)
        {
            ShowHintPanel();
        }
        else
        {
            SelectVocIndex = (int)Star.FiveVoc;
            ShowStars();
            LoadVoc();
            StarSelect.SetActive(false);
            ViewVoc.SetActive(true);
        }
    }

    public void PreBtn()
    {
        ShowVocindex--;
        if (ShowVocindex < 0) ShowVocindex = 0;
        BtnEnable(ShowVocindex);
        ShowVoc(ShowVocindex);
    }

    public void NextBtn()
    {
        ShowVocindex++;
        if (ShowVocindex > VocSize) ShowVocindex = VocSize-1;
        BtnEnable(ShowVocindex);
        ShowVoc(ShowVocindex);
    }

    void BtnEnable(int index)
    {
        PreButton.SetActive(true);
        NextButton.SetActive(true);

        if (index < 1)
        {
            PreButton.SetActive(false);
        }
        if (index > VocSize - 2)
        {
            NextButton.SetActive(false);
        }
    }
    public void ExitBtn()
    {
        StarPanel.SetActive(false);
    }

    public void ReturnSelectStar()
    {
        ViewVoc.SetActive(false);
        StarSelect.SetActive(true);
    }

    public void GotoStartPanel()
    {
        StarPanel.SetActive(true);
        ViewVoc.SetActive(false);
    }
    void ShowVoc(int index)
    {
        EngText.text = Voc[index].English;
        ChiText.text = Voc[index].Chinese;
    }

    void LoadVoc()
    {
        StarSelect.SetActive(false);

        switch(SelectVocIndex)
        {
            case 1:
                Voc = null;
                VocSize = OneStar;
                Voc = new Vocabulary[OneStar];
                for (int i = 0; i < OneStar; i++)
                {
                    Voc[i] = OneStarVoc[i];
                }
                break;
            case 2:
                Voc = null;
                VocSize = TwoStar;
                Voc = new Vocabulary[TwoStar];
                for (int i = 0; i < TwoStar; i++)
                {
                    Voc[i] = TwoStarVoc[i];
                }
                break;
            case 3:
                Voc = null;
                VocSize = ThreeStar;
                Voc = new Vocabulary[ThreeStar];
                for (int i = 0; i < ThreeStar; i++)
                {
                    Voc[i] = ThreeStarVoc[i];
                }
                break;
            case 4:
                Voc = null;
                VocSize = FourStar;
                Voc = new Vocabulary[FourStar];
                for (int i = 0; i < FourStar; i++)
                {
                    Voc[i] = FourStarVoc[i];
                }
                break;
            case 5:
                Voc = null;
                VocSize = FiveStar;
                Voc = new Vocabulary[FiveStar];
                for (int i = 0; i < FiveStar; i++)
                {
                    Voc[i] = FiveStarVoc[i];
                }
                break;
        }
        ShowVocindex = 0;
        ShowVoc(ShowVocindex);
        BtnEnable(ShowVocindex);
    }

    void ShowStars()
    {
        for (int i = 0; i < 5; i++)
        {
            Stars[i].SetActive(false);
        }
        for (int i=0;i<SelectVocIndex;i++)
        {
            Stars[i].SetActive(true);
        }
    }

    void ShowHintPanel()
    {
        HintPanel.SetActive(true);
        HintShowTimer = 0f;
    }

    // Audio Fuction
    void GetAudioFilePath()
    {
        if (isnotAddAudioSource)
        {
            Audio = gameObject.AddComponent<AudioSource>();
            Audio.playOnAwake = false;
            isnotAddAudioSource = false;
        }

        AudioName = Voc[ShowVocindex].English + ".mp3";

#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX
        SoundPath = "file://" + Application.streamingAssetsPath + "/Sound/";
#elif UNITY_ANDROID
        SoundPath = "jar:file://" + Application.dataPath + "!/assets/Sound/";
#elif UNITY_IOS
        SoundPath = "file://"+Application.dataPath + "/Raw/Sound/";
#endif

        StartCoroutine(LoadAudio());
    }
    IEnumerator LoadAudio()
    {
        WWW request = GetAudioFromFile(SoundPath, AudioName);
        yield return request;

        SaveAudio = request.GetAudioClip();
        SaveAudio.name = AudioName;

        PlayAudioFile();
    }

    WWW GetAudioFromFile(string path, string filename)
    {
        string audioToLoad = string.Format(path + "{0}", filename);
        WWW request = new WWW(audioToLoad);
        return request;
    }

    void PlayAudioFile()
    {
        Audio.clip = SaveAudio;
        Audio.Play();
    }

    public void PlayAudio()
    {
        GetAudioFilePath();
    }
}
