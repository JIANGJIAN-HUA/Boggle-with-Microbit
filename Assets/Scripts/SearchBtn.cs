using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SearchBtn : MonoBehaviour
{
    /*
     string str = "GeeksForGeeks";

         // Finding the index of character
         // which is present in string and
         // this will show the value 5
         int index1 = str.IndexOf('F');
    */

    //public Text InputText;
    public GameObject SearchPanel;
    public GameObject ResultBtn;
    public InputField InputSearchField;
    private int linkButtonPos, linkBtnFragment;     //按鈕出現位置y,每個按鈕的間距
    public RectTransform conBtnPanel;				//scroll view panel
    public Transform conBtnArch;					//按鈕錨點與父節點

    public InputField SearchInputField;

    void Start()
    {
        linkButtonPos = 100;
        linkBtnFragment = 100;

        if(PlayerPrefs.GetInt("OpenSearchPanel") == 1)
        {
            SHBtn();
        }

        if(PlayerPrefs.GetString("GetInput").Length > 0)
        {
            SearchInputField.text = PlayerPrefs.GetString("GetInput");
            SearchVoc();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.SetString("GetInput","");
            PlayerPrefs.SetInt("OpenSearchPanel", 0);
            SceneManager.LoadScene((ushort)Scenes.MainMenu);
        }
    }
    public void ExitBtn()
    {
        SearchPanel.SetActive(false);
        //InputSearchField.clear();
        PlayerPrefs.SetInt("OpenSearchPanel", 0);
    }

    public void SHBtn()
    {
        SearchPanel.SetActive(true);
        //SearchInputField.text = PlayerPrefs.GetString("GetInput");
        PlayerPrefs.SetInt("OpenSearchPanel",1);
    }

    /*
    public void ClearResultBtn()
    {
        SceneManager.LoadScene((ushort)Scenes.ViewVoc);
    }
    */

    void addResultBtn(string Result,int index)
    {
        GameObject newPeripheral = Instantiate(ResultBtn);
        newPeripheral.transform.SetParent(conBtnArch);
        newPeripheral.transform.localScale = new Vector3(1, 1, 1);
        newPeripheral.transform.localPosition = new Vector2(0, linkButtonPos);
        newPeripheral.GetComponent<ResultBtn>().name = Result;
        newPeripheral.GetComponent<ResultBtn>().Result = Result;
        newPeripheral.GetComponent<ResultBtn>().index = index;
        newPeripheral.GetComponentInChildren<Text>().text = Result;

        linkButtonPos -= linkBtnFragment;
    }

    /*
    public void SHVocChiBtn()
    {
        for (int i = 0; i < GetVoc.VocNum; i++)
        {
            if (GetVoc.Voc[i].Chinese.IndexOf(SearchInputField.GetComponent<InputField>().text) >=0)
            {
                addResultBtn(GetVoc.Voc[i].Chinese,i);
                conBtnPanel.sizeDelta = new Vector2(0, conBtnPanel.sizeDelta.y + linkBtnFragment);
            }
        }
    }
    */

    public void TextChange()
    {
        if(SearchInputField.GetComponent<InputField>().text.Length > 0)
        {
            PlayerPrefs.SetString("GetInput", SearchInputField.GetComponent<InputField>().text);
            SceneManager.LoadSceneAsync((ushort)Scenes.ViewVoc);
        }
    }

    public void SearchVoc()
    {
        if (IsEnglish(SearchInputField.GetComponent<InputField>().text))
        {
            for (int i = 0; i < Constants.VocNum; i++)
            {
                if (GetVoc.Voc[i].English.IndexOf(SearchInputField.GetComponent<InputField>().text) >= 0)
                {
                    addResultBtn(GetVoc.Voc[i].English, i);
                    conBtnPanel.sizeDelta = new Vector2(0, conBtnPanel.sizeDelta.y + linkBtnFragment);
                }
            }
        }
        else
        {
            for (int i = 0; i < Constants.VocNum; i++)
            {
                if (GetVoc.Voc[i].Chinese.IndexOf(SearchInputField.GetComponent<InputField>().text) >= 0)
                {
                    addResultBtn(GetVoc.Voc[i].Chinese, i);
                    conBtnPanel.sizeDelta = new Vector2(0, conBtnPanel.sizeDelta.y + linkBtnFragment);
                }
            }
        }
    }

    bool IsEnglish(string str)
    {
        Regex eng = new Regex("^[a-zA-Z]+$");
        return eng.IsMatch(str);
    }

}
