using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private BTsocket btSoc;

    /*
    public const int row= 4;
    public const int col= 4;
    public const int DiceNum = row*col;
    public static char[,] boggle = new char[row, col];
    */

    int row;
    int col;
    int DiceNum;
    char[,] boggle;

    //public Text[] BtnText = new Text[DiceNum];
    List<string> VocList = new List<string>();
    List<string> VocChiList = new List<string>();
    List<Vocabulary> FindVocList = new List<Vocabulary>();
    bool[] CorrectVoc;
    public GameObject LoadingPanel;
    public GameObject Counter;
    //public static int x = -1, y = -1;
    //public static char Alph;
    bool isStart = true;
    //bool isPressHintBtn;
    bool isGenNoAnsVoc;
    int Score = 0;
    int FullScore = 0;

    // GameObject GamePausePanel;
    public GameObject GameFinishPanel;
    public GameObject RestartPanel;

    public Button HintBtn;

    public Text TextHint;
    public Text TextScore;
    public Text TextChiHint;
    public Text TextFinishScore;
    public Text TextNoCurVocNum;
    public Text TextFullMarks;
    public Text TextReceiveVoc;

    /*
    public int SecondCount;
    public float MilliCount;
    */

    // ShowCorrectVoc
    public GameObject ShowVoc;					    //按鈕預製
    public RectTransform conBtnPanel;               //scroll view panel
    public Transform conBtnArch;					//按鈕錨點與父節點
    private int linkButtonPos, linkBtnFragment;     //按鈕出現位置y,每個按鈕的間距

    // ShowNoFindVoc
    public RectTransform N_conBtnPanel;               //scroll view panel
    public Transform N_conBtnArch;					//按鈕錨點與父節點
    private int N_linkButtonPos, N_linkBtnFragment;     //按鈕出現位置y,每個按鈕的間距

    public GameObject CountDown;
    public AudioSource GetReady;
    public AudioSource GoAudio;
    public GameObject CountdownPanel;

    public AudioSource CorrectAudio;
    public AudioSource ErrorAudio;
    public AudioSource WarningAudio;

    //bool isCheck1v1 = true;
    //bool isReceiveAlph = true;

    public Button ViewNoAnsBtn;

    public Text TextAns;

    [Header("DialogBox")]
    public GameObject ChiHintDialog;
    public GameObject HintDialog;
    public GameObject RecVocDialog;

    //ShowHintAndRecVocDelayTime
    const float DelayTime = 3f;
    float ShowRVTimer = 0f;
    bool isEnterShRefun = false;

    // CheckReceive
    bool isnotRecAlph = true;

    // AvoidReceiveAlphMisjudgment
    string ReceiveAlph = "";

    // GetReceiveDelay
    float RequestDataDelay = 0f;

    // Background Music
    Audio Music;

    [Header("ChangeTimerStatus")]
    public GameObject MinShowBox;
    public GameObject SecShowBox;
    public GameObject ShowUnlimitedBox;

    int HintChoice;
    int PreChice = -1;

    /*
    public class dice
    {
        private string alphabet;
        
        public void SetAlph(string Alph)
        {
            this.alphabet = Alph;
        }

        public char GetAlph(int index)
        {
            return alphabet[index];
        }
    }
    

    dice[] dices = new dice[DiceNum];
    */

    //Boggle BogGame = new Boggle();
    Boggle BogGame;

    void Awake()
    {
        Music = FindObjectOfType<Audio>();
        btSoc = BTsocket.getBTsocket(Constants.bleMicroBit);

        // 遊戲初始設定
        row = col = PlayerPrefs.GetInt("Size");
        DiceNum = row * col;
        boggle = new char[row, col];
        BogGame = new Boggle(row,col);
    }

    void Start()
    {
        /*
        for(int i=0;i<DiceNum;i++)
        {
            dices[i] = new dice();
        }
        // Dices Settings
        dices[00].SetAlph("ETUKNO");
        dices[01].SetAlph("EVGTIN");
        dices[02].SetAlph("DECAMP");
        dices[03].SetAlph("IELRUW");
        dices[04].SetAlph("EHIFSE");
        dices[05].SetAlph("RECALS");
        dices[06].SetAlph("ENTDOS");
        dices[07].SetAlph("OFXRIA");
        dices[08].SetAlph("NAVEDZ");
        dices[09].SetAlph("EIOATA");
        dices[10].SetAlph("GLENYU");
        dices[11].SetAlph("BMAQJO");
        dices[12].SetAlph("TLIBRA");
        dices[13].SetAlph("SPULTE");
        dices[14].SetAlph("AIMSOR");
        dices[15].SetAlph("ENHRIS");

        ShakeDice();
        */

        /*
        BogGame.FindWord();

        CorrectVoc = new int[VocList.Count];

        for (int i = 0; i < VocList.Count; i++)
        {
            for(int j=0;j<GetVoc.VocNum;j++)
            {
                if (VocList[i] == GetVoc.Voc[j].English.ToUpper())
                {
                    VocChiList.Add(GetVoc.Voc[j].Chinese);
                    break;
                }
            }
        }
        
        for(int i=0;i<VocList.Count;i++)
        {
            Debug.Log("EngList: " + VocList[i] + "\n" + "ChiList: " + VocChiList[i]);
        }
        */
        /*
        foreach (string item in VocList)
        {
            Debug.Log("EngList: "+item);
        }

        foreach (string item in VocChiList)
        {
            Debug.Log("ChiList: " + item);
        }
        */

        linkButtonPos = 100;
        linkBtnFragment = 100;
        N_linkButtonPos = 100;
        N_linkBtnFragment = 100;
        TextHint.text = "";
        TextChiHint.text = "";
        TextReceiveVoc.text = "";

        ChiHintDialog.SetActive(false);
        HintDialog.SetActive(false);
        RecVocDialog.SetActive(false);

        //BTManager.btSoc.writeCharacteristic("RAL\n");
        //BTManager.btSoc.writeCharacteristic("1\n");
    }

    void FixedUpdate()
    {
        // Show Hint Time
        /*
        MilliCount += Time.deltaTime * 10;
        if (MilliCount >= 10)
        {
            MilliCount = 0;
            SecondCount += 1;
        }

        if(TextHint.text.Length > 0)
        {
            if(SecondCount > 3)
            {
                TextHint.text = "";
            }
        }
        */
        
        // 如果玩家找到所有的單字，遊戲提早結束
        if(Int32.Parse(TextNoCurVocNum.text) == 0)
        {
            if(!isStart)
            {
                //MinText.text = "0:";
                //SecText.text = "00";
                ShowNoAnsVoc();
                Time.timeScale = 0;
                Music.StopBG();
                Music.PlayGF();
                GameFinishPanel.SetActive(true);
            }
        }

        /*
        // Game Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        */

        /*
        if (Input.GetKeyDown(KeyCode.A))    // 測試用
        {
            addCorrectVoc("123", "addr");
            conBtnPanel.sizeDelta = new Vector2(0, conBtnPanel.sizeDelta.y + linkBtnFragment);
        }
        */

        //read BLE data... 有新資料時將自動呼叫委派函數，receiveData為收到的新資料
        //讀取頻率將與Update相同，如需更高頻率需自行建立執行緒或使用其他方式
        btSoc.getReceiveText((receiveData) =>
        {
            //收到新資料時自動執行該處的code，可自行加入所需功能
            
            if (isStart)
            {
                // 確保 APP 和 Micro:bit 能夠同步
                if(receiveData.Length == DiceNum)
                {
                    isnotRecAlph = false;
                }

                if (isnotRecAlph) return;

                // 接收英文字母
                int count = 0;
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        ReceiveAlph += receiveData[count];
                        boggle[i, j] = receiveData[count++];
                    }
                }

                // Search and Return Vocabulary 
                VocList = BogGame.FindWord(boggle);

                // If not found words or few words
                if (VocList.Count < 6)
                {
                    RestartPanel.SetActive(true);
                    Time.timeScale = 0;
                }
                
                // 計算單字出現的頻率
                for(int i=0;i< VocList.Count;i++)
                {
                    PlayerPrefs.SetInt(VocList[i] +"Total",PlayerPrefs.GetInt(VocList[i] + "Total")+1);
                }

                CorrectVoc = new bool[VocList.Count];
                TextNoCurVocNum.text = VocList.Count.ToString();

                // Caculate Full Marks
                for(int i=0;i<VocList.Count;i++)
                {
                    CounterFullMarks(VocList[i]);
                }
                TextFullMarks.text = FullScore.ToString();

                // Get Chinese Hint
                foreach (var FindVoc in VocList)
                {
                    Vocabulary searchVocabulary = new Vocabulary { English = FindVoc };
                    int index = Array.BinarySearch(GetVoc.Voc, searchVocabulary);

                    VocChiList.Add(GetVoc.Voc[index].Chinese);
                    FindVocList.Add(new Vocabulary { English = GetVoc.Voc[index].English, Chinese = GetVoc.Voc[index].Chinese });
                }
                FindVocList.Sort();
                
                LoadingPanel.SetActive(false);

                // For Debug
                /*
                TextAns.text = "";
                for (int i=0;i<VocList.Count;i++)
                {
                    TextAns.text += VocList[i] + "," + VocChiList[i]+"\n";
                }
                */

                // Countdown Start
                StartCoroutine(CountStart());

                // For Debug
                /*
                for (int i = 0; i < VocList.Count; i++)
                {
                    //Debug.Log("EngList: " + VocList[i] + "\n" + "ChiList: " + VocChiList[i]);
                    TextAns.text += "EngList: " + VocList[i] + ", " + "ChiList: " + VocChiList[i] + "\n";
                }
                */
                isStart = false;
            }
            else
            {
                // Read send Vocabulary
                string Voc = receiveData;
                
                // 避免驗證資料被當作單字去判斷
                if (Voc == ReceiveAlph) return;  // 取得 3*3 或 4*4 字母資料
                //if (Voc == "Cont") return;  // 解鎖 Micro:bit 驗證

                // StepError
                if(Voc == "AAAAA")
                {
                    ShowHintAndRecVocText("Press Error", "");
                    ErrorAudio.Play();
                    return;
                }

                // 判斷 Voc 是否在答案裡面
                int index = VocList.IndexOf(Voc);
                if (index == -1) // Not Found word 
                {
                    //TextHine.text = "Not a word";
                    ShowHintAndRecVocText("Not a word", Voc);
                    ErrorAudio.Play();
                }
                else
                {
                    if (CorrectVoc[index])
                    {
                        //TextHint.text = "Word repeat";
                        ShowHintAndRecVocText("Word repeat", Voc);
                        WarningAudio.Play();
                    }
                    else
                    {
                        //TextHint.text = "Found a word";
                        ShowHintAndRecVocText("Found a Word", Voc);
                        CorrectAudio.Play();
                        //CounterScore(Voc);
                        CorrectVoc[index] = true;
                        TextNoCurVocNum.text = (Int32.Parse(TextNoCurVocNum.text) - 1).ToString();

                        // 添加已答對的單字
                        Vocabulary searchVocabulary = new Vocabulary { English = Voc };
                        int i = FindVocList.BinarySearch(searchVocabulary);
                        addCorrectVoc(FindVocList[i].English, FindVocList[i].Chinese);
                        conBtnPanel.sizeDelta = new Vector2(0, conBtnPanel.sizeDelta.y + linkBtnFragment);

                        PlayerPrefs.SetInt(Voc + "Correct", PlayerPrefs.GetInt(Voc + "Correct") + 1);

                        // 是否是根據提示答對
                        //TextAns.text = VocChiList[index] + "," +index;
                        if (TextChiHint.text == FindVocList[i].Chinese)
                        {
                            TextChiHint.text = "";
                            ChiHintDialog.SetActive(false);
                            //isPressHintBtn = true;
                        }
                        /*
                        else
                        {
                            isPressHintBtn = false;
                        }
                        */
                        CounterScore(Voc);

                        VocChiList.Remove(FindVocList[i].Chinese);
                    }
                }
            }
        });
    }

    void Update()
    {
        RequestDataDelay += Time.deltaTime;
        // 確保與 Micro:bit 同步
        if (RequestDataDelay > 2.0f && isStart)
        {
            if (isnotRecAlph)
            {
                btSoc.writeCharacteristic("1RAL\n");
            }
            RequestDataDelay = 0f;
        }

        // 顯示時間重置
        if (isEnterShRefun)
        {
            ShowRVTimer = 0f;
            isEnterShRefun = false;
        }
        // 顯示 Voc 和 Hint 的持續時間
        if(HintDialog.activeInHierarchy)
        {
            ShowRVTimer += Time.deltaTime;
            if(ShowRVTimer > DelayTime)
            {
                TextReceiveVoc.text = "";
                RecVocDialog.SetActive(false);
                HintDialog.SetActive(false);
                TextHint.text = "";
                ShowRVTimer = 0f;
            }
        }
    }

    /*
    void ShakeDice()
    {
        int count = 0;
        for(int i=0;i<row;i++)
        {
            for(int j=0;j<col;j++)
            {
                boggle[i, j] = dices[count].GetAlph(UnityEngine.Random.Range(0,6));
                BtnText[count++].text = boggle[i,j].ToString();
            }
        }

    }
    */

    void CounterScore(string Voc)
    {
        /*
        if(isPressHintBtn)
        {
            return;
        }
        */

        switch (Voc.Length)
        {
            case 2:
            case 3:
            case 4:
                Score += 1;
                break;
            case 5:
                Score += 2;
                break;
            case 6:
                Score += 3;
                break;
            case 7:
                Score += 4;
                break;
            case 8:
                Score += 11;
                break;
        }

        TextScore.text = Score.ToString();
        TextFinishScore.text = Score.ToString();
    }

    void CounterFullMarks(string Voc)
    {
        switch (Voc.Length)
        {
            case 2:
            case 3:
            case 4:
                FullScore += 1;
                break;
            case 5:
                FullScore += 2;
                break;
            case 6:
                FullScore += 3;
                break;
            case 7:
                FullScore += 4;
                break;
            case 8:
                FullScore += 11;
                break;
        }
    }

    // HintBtn
    public void GetChiHint()
    {
        do
        {
            HintChoice = UnityEngine.Random.Range(0, VocChiList.Count);
        } while (PreChice == HintChoice);
        PreChice = HintChoice;
        TextChiHint.text = VocChiList[HintChoice];
        ChiHintDialog.SetActive(true);

        //isPressHintBtn = true;
        /*
        if(Score > 0)
        {
            Score--;
        }
        */
        StartCoroutine(HintBtnAbleDelay());
    }

    void addCorrectVoc(string VocEng, string VocChi)
    {
        GameObject newPeripheral = Instantiate(ShowVoc);
        newPeripheral.transform.SetParent(conBtnArch);
        newPeripheral.transform.localScale = new Vector3(1, 1, 1);
        newPeripheral.transform.localPosition = new Vector2(0, linkButtonPos);
        newPeripheral.GetComponent<ShowVoc>().name = VocEng;
        newPeripheral.GetComponentInChildren<Text>().text = VocEng + "\n" + VocChi;

        linkButtonPos -= linkBtnFragment;
    }

    void addNoAnswerVoc(string VocEng, string VocChi)
    {
        GameObject newPeripheral = Instantiate(ShowVoc);
        newPeripheral.transform.SetParent(N_conBtnArch);
        newPeripheral.transform.localScale = new Vector3(1, 1, 1);
        newPeripheral.transform.localPosition = new Vector2(0, N_linkButtonPos);
        newPeripheral.GetComponent<ShowVoc>().name = VocEng;
        newPeripheral.GetComponentInChildren<Text>().text = VocEng + "\n" + VocChi;

        N_linkButtonPos -= N_linkBtnFragment;
    }

    IEnumerator CountStart()
    {
        //CountdownPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        CountDown.GetComponent<Text>().text = "3";
        GetReady.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1f);
        CountDown.SetActive(false);
        CountDown.GetComponent<Text>().text = "2";
        GetReady.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1f);
        CountDown.SetActive(false);
        CountDown.GetComponent<Text>().text = "1";
        GetReady.Play();
        CountDown.SetActive(true);
        yield return new WaitForSeconds(1f);
        CountDown.SetActive(false);
        GoAudio.Play();
        CountdownPanel.SetActive(false);
        if (PlayerPrefs.GetInt("Timer") == 1)
        {
            Counter.SetActive(true);
            MinShowBox.SetActive(true);
            SecShowBox.SetActive(true);
            ShowUnlimitedBox.SetActive(false);
        }
        else
        {
            Counter.SetActive(false);
            MinShowBox.SetActive(false);
            SecShowBox.SetActive(false);
            ShowUnlimitedBox.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        Music.PlayBG();
    }

    void ShowHintAndRecVocText(string HintStr,string RecVoc)
    {
        isEnterShRefun = true;
        TextHint.text = HintStr;
        HintDialog.SetActive(true);
        if(RecVoc.Length > 0)
        {
            TextReceiveVoc.text = RecVoc;
            RecVocDialog.SetActive(true);
        }
    }

    IEnumerator HintBtnAbleDelay()
    {
        HintBtn.interactable = false;
        yield return new WaitForSeconds(10f);
        HintBtn.interactable = true;
    }

    // 遊戲結束顯示未答對的單字
    public void ShowNoAnsVoc()
    {
        if(!isGenNoAnsVoc)
        {
            bool isnotAllCorrectVoc = false;

            for (int i = 0; i < VocList.Count; i++)
            {
                if (!CorrectVoc[i])
                {
                    Vocabulary searchVocabulary = new Vocabulary { English = VocList[i] };
                    int index = FindVocList.BinarySearch(searchVocabulary);
                    addNoAnswerVoc(FindVocList[index].English, FindVocList[index].Chinese);
                    N_conBtnPanel.sizeDelta = new Vector2(0, N_conBtnPanel.sizeDelta.y + N_linkBtnFragment);
                    isnotAllCorrectVoc = true;
                }
            }

            if (isnotAllCorrectVoc)
            {
                ViewNoAnsBtn.interactable = true;
            }
            else
            {
                ViewNoAnsBtn.interactable = false;
            }

            isGenNoAnsVoc= true;
        }
    }
}