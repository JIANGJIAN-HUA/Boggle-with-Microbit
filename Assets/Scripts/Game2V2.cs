using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game2V2 : MonoBehaviour
{
    private BTsocket btSoc;

    /*
    public const int row = 4;
    public const int col = 4;
    public const int DiceNum = row * col;
    public static char[,] boggle = new char[row, col];
    */
    int row;
    int col;
    int DiceNum;
    char[,] boggle;

    //public Text[] BtnText = new Text[DiceNum];
    List<string> VocList = new List<string>();
    List<Vocabulary> FindVocList = new List<Vocabulary>();
    bool[] CorrectVoc;
    public GameObject LoadingPanel;
    public GameObject Counter;
    //public static int x = -1, y = -1;
    //public static char Alph;
    bool isStart = true;
    //bool isPressHintBtn = false;
    char Player;
    int AScore = 0,BScore = 0;
    int ShowHintFunNum = 0;

    //public GameObject GamePausePanel;
    public GameObject GameFinishPanel;
    public GameObject RestartPanel;

    //public Button HintBtn;

    public Text TextHint;
    public Text ATextScore;
    public Text BTextScore;
    public Text TextNoCurVocNum;
    public Text TextResult;
    public Text TextARecVoc;
    public Text TextBRecVoc;

    /*
    public int SecondCount;
    public float MilliCount;
    */
    public GameObject ShowVoc;					    //按鈕預製
    public RectTransform AconBtnPanel;               //scroll view panel
    public Transform AconBtnArch;					//按鈕錨點與父節點
    public RectTransform BconBtnPanel;               //scroll view panel
    public Transform BconBtnArch;					//按鈕錨點與父節點
    private int AlinkButtonPos, AlinkBtnFragment; 	//按鈕出現位置y,每個按鈕的間距
    private int BlinkButtonPos, BlinkBtnFragment; 	//按鈕出現位置y,每個按鈕的間距

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

    public Button ViewNoAnsBtn;

    [Header("DialogBox")]
    public GameObject HintDialog;
    public GameObject ADialog;
    public GameObject BDialog;

    [Header("TimerText")]
    public Text MinText;
    public Text SecText;
    bool isGenNoAnsVoc;

    //public Text TextAns;

    // Hint avoid close
    bool AHintOpen;
    bool BHintOpen;

    // ReceiveAlphJudge
    bool isnotReceiveAlph = true;

    // AvoidReceiveAlphMisjudgment
    string ReceiveAlph = "";

    // GetReceiveDelay
    float RequestDataDelay = 0f;

    [Header("ChangeTimerStatus")]
    public GameObject MinShowBox;
    public GameObject SecShowBox;
    public GameObject ShowUnlimitedBox;

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

    Boggle BogGame;

    Audio Music;

    void Awake()
    {
        Music = FindObjectOfType<Audio>();
        btSoc = BTsocket.getBTsocket(Constants.bleMicroBit);

        // 遊戲初始設定
        row = col = PlayerPrefs.GetInt("Size");
        DiceNum = row * col;
        boggle = new char[row, col];
        BogGame = new Boggle(row, col);
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

        AlinkButtonPos = 100;
        AlinkBtnFragment = 100;

        BlinkButtonPos = 100;
        BlinkBtnFragment = 100;

        N_linkButtonPos = 100;
        N_linkBtnFragment = 100;

        HintDialog.SetActive(false);
        ADialog.SetActive(false);
        BDialog.SetActive(false);
        TextARecVoc.text = "";
        TextBRecVoc.text = "";
        TextHint.text = "";

        //BTManager.btSoc.writeCharacteristic("RAL\n");
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

        if (TextHint.text.Length > 0)
        {
            if (SecondCount > 3)
            {
                TextHint.text = "";
            }
        }
        */

        if (Int32.Parse(TextNoCurVocNum.text) == 0)
        {
            if (!isStart)
            {
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
            addCorrectVocA("address", "地址");
            AconBtnPanel.sizeDelta = new Vector2(0, AconBtnPanel.sizeDelta.y + AlinkBtnFragment);
        }

        if (Input.GetKeyDown(KeyCode.B))    // 測試用
        {
            addCorrectVocB("mail", "郵件");
            BconBtnPanel.sizeDelta = new Vector2(0, BconBtnPanel.sizeDelta.y + BlinkBtnFragment);
        }
        */
        
        //read BLE data... 有新資料時將自動呼叫委派函數，receiveData為收到的新資料
        //讀取頻率將與Update相同，如需更高頻率需自行建立執行緒或使用其他方式
        btSoc.getReceiveText((receiveData) =>
        {
            //收到新資料時自動執行該處的code，可自行加入所需功能

            //BTManager.btSoc.writeCharacteristic("Length: "+receiveData.Length + "\n");
            if (isStart)
            {

                if (receiveData.Length == DiceNum)
                {
                    isnotReceiveAlph = false;
                }
                else return;

                // 接收 3x3 或 4x4 的英文字母
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

                CorrectVoc = new bool[VocList.Count];
                TextNoCurVocNum.text = VocList.Count.ToString();

                foreach(var Voc in VocList)
                {
                    Vocabulary searchVoc = new Vocabulary { English = Voc };
                    int index = Array.BinarySearch(GetVoc.Voc,searchVoc);
                    FindVocList.Add(new Vocabulary { English = GetVoc.Voc[index].English, Chinese = GetVoc.Voc[index].Chinese });
                }
                FindVocList.Sort();

                LoadingPanel.SetActive(false);
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
                Player = receiveData[0];
                string Voc = receiveData.Substring(1);

                if (Voc == ReceiveAlph.Substring(1)) return;
                //if (Voc == "ont") return;

                // StepError
                if (Voc == "AAAA" || Voc == "BBBB")
                {
                    StartCoroutine(ShowHintAndRecVocText(Player,Player + " Player Press Error", ""));
                    ErrorAudio.Play();
                    return;
                }

                // 搜尋 Voc 是否在 VocList
                int index = VocList.IndexOf(Voc);
                if (index == -1) // Not Found word 
                {
                    //TextHint.text = "Not a word";
                    StartCoroutine(ShowHintAndRecVocText(Player, Player+" Player wrong", Voc));
                    ErrorAudio.Play();
                }
                else
                {
                    if (CorrectVoc[index]) 
                    {
                        //TextHint.text = "Word repeat";
                        StartCoroutine(ShowHintAndRecVocText(Player, "Word repeat", Voc));
                        WarningAudio.Play();
                    }
                    else
                    {
                        CorrectAudio.Play();
                        CorrectVoc[index] = true;
                        CounterScore(Player, Voc);
                        StartCoroutine(ShowHintAndRecVocText(Player, Player + " Player Found a word !", Voc));
                        TextNoCurVocNum.text = (Int32.Parse(TextNoCurVocNum.text) - 1).ToString();
                        index = FindVocList.BinarySearch(new Vocabulary { English = Voc });
                        if (Player == 'A')
                        {
                            addCorrectVocA(FindVocList[index].English, FindVocList[index].Chinese);
                            AconBtnPanel.sizeDelta = new Vector2(0, AconBtnPanel.sizeDelta.y + AlinkBtnFragment);
                        }
                        else
                        {
                            addCorrectVocB(FindVocList[index].English, FindVocList[index].Chinese);
                            BconBtnPanel.sizeDelta = new Vector2(0, BconBtnPanel.sizeDelta.y + BlinkBtnFragment);
                        }
                    }
                }
            }
        });
    }

    void Update()
    {
        RequestDataDelay += Time.deltaTime;

        if (RequestDataDelay > 2.0f && isStart)
        {
            if (isnotReceiveAlph)
            {
                btSoc.writeCharacteristic("RAL\n");
            }
            RequestDataDelay = 0f;
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

    void CounterScore(char Player,string Voc)
    {

        switch (Voc.Length)
        {
            case 2:
            case 3:
            case 4:
                if(Player == 'A')
                {
                    AScore += 1;
                }
                else
                {
                    BScore += 1;
                }
                break;
            case 5:
                if (Player == 'A')
                {
                    AScore += 2;
                }
                else
                {
                    BScore += 2;
                }
                break;
            case 6:
                if(Player == 'A')
                {
                    AScore += 3;
                }
                else
                {
                    BScore += 3;
                }
                break;
            case 7:
                if (Player == 'A')
                {
                    AScore += 4;
                }
                else
                {
                    BScore += 4;
                }
                break;
            case 8:
                if (Player == 'A')
                {
                    AScore += 11;
                }
                else
                {
                    BScore += 11;
                }
                break;
        }

        ATextScore.text = AScore.ToString();
        BTextScore.text = BScore.ToString();
    }

    /*
    // HintBtn
    public void GetChiHint()
    {
        for (int i = 0; i < VocList.Count; i++)
        {
            if (CorrectVoc[i] == false)
            {
                VocChiChoiceList.Add(VocChiList[i]);
            }
        }

        TextChiHint.text = VocChiChoiceList[UnityEngine.Random.Range(0, VocChiChoiceList.Count)];
        VocChiChoiceList.Clear();

        isPressHintBtn = true;
    }
    */

    void addCorrectVocA(string VocEng, string VocChi)
    {
        GameObject newPeripheral = Instantiate(ShowVoc);
        newPeripheral.transform.SetParent(AconBtnArch);
        newPeripheral.transform.localScale = new Vector3(1, 1, 1);
        newPeripheral.transform.localPosition = new Vector2(0, AlinkButtonPos);
        newPeripheral.GetComponent<ShowVoc>().name = VocEng;
        newPeripheral.GetComponentInChildren<Text>().text = VocEng + "\n" + VocChi;

        AlinkButtonPos -= AlinkBtnFragment;
    }

    void addCorrectVocB(string VocEng, string VocChi)
    {
        GameObject newPeripheral = Instantiate(ShowVoc);
        newPeripheral.transform.SetParent(BconBtnArch);
        newPeripheral.transform.localScale = new Vector3(1, 1, 1);
        newPeripheral.transform.localPosition = new Vector2(0, BlinkButtonPos);
        newPeripheral.GetComponent<ShowVoc>().name = VocEng;
        newPeripheral.GetComponentInChildren<Text>().text = VocEng + "\n" + VocChi;

        BlinkButtonPos -= BlinkBtnFragment;
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

    IEnumerator ShowHintAndRecVocText(char Player,string HintStr,string RecVoc)
    {
        ShowHintFunNum++;
        HintDialog.SetActive(true);
        TextHint.text = HintStr;
        if (Player == 'A')
        {
            if (RecVoc.Length > 0)
            {
                ADialog.SetActive(true);
                TextARecVoc.text = RecVoc;
            }
            //AHintOpen = true;
            yield return new WaitForSeconds(3f);
            //AHintOpen = false;
            ADialog.SetActive(false);
            TextARecVoc.text = "";
        }
        else
        {
            if (RecVoc.Length > 0)
            {
                BDialog.SetActive(true);
                TextBRecVoc.text = RecVoc;
            }
            //BHintOpen = true;
            yield return new WaitForSeconds(3f);
            //BHintOpen = false;
            BDialog.SetActive(false);
            TextBRecVoc.text = "";
        }

        if (--ShowHintFunNum > 0)
        {
            HintDialog.SetActive(true);
        }
        else
        {
            HintDialog.SetActive(false);
            TextHint.text = "";
        }
    }

    public void ShowNoAnsVoc()
    {
        if (!isGenNoAnsVoc)
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

            isGenNoAnsVoc = true;
        }
    }
}