using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Extension
{
    /*
    public static void clear(this InputField inputfield)
    {
        inputfield.Select();
        inputfield.text = "";
    }
    */
}
public class ResultBtn : MonoBehaviour
{
    public GameObject SearchPanel;
    public string Result;
    public int index;
    public Text EnglishText; // 顯示英文
    public Text ChineseText; // 顯示中文
    public InputField InputSearchField;

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(BtnClicked);
    }
    void BtnClicked()
    {
        ViewVoc.index = index;
        PlayerPrefs.SetInt("Page", ViewVoc.index);

        EnglishText.text = GetVoc.Voc[ViewVoc.index].English;
        ChineseText.text = GetVoc.Voc[ViewVoc.index].Chinese;

        //InputSearchField.clear();
        SearchPanel.SetActive(false);
    }


}
