using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class IndexBtn : MonoBehaviour
{
    /*
     string str = "GeeksForGeeks";

         // Finding the index of character
         // which is present in string and
         // this will show the value 5
         int index1 = str.IndexOf('F');
    */

    public GameObject IndexPanel;
    public Text EnglishText; // 顯示英文
    public Text ChineseText; // 顯示中文

    public void InBtn()
    {
        IndexPanel.SetActive(true);
    }

    public void FindAlpFirst(string Alp)
    {
        Alp = Alp.ToLower();
        bool isFind = false;
        for(int i=0;i<Constants.VocNum;i++)
        {
            if (GetVoc.Voc[i].English.ToLower().IndexOf(Alp) == 0)
            {
                ViewVoc.index = i;
                isFind = true;
                break;
            }
        }

        if(isFind)
        {
            PlayerPrefs.SetInt("Page", ViewVoc.index);

            EnglishText.text = GetVoc.Voc[ViewVoc.index].English;
            ChineseText.text = GetVoc.Voc[ViewVoc.index].Chinese;

            IndexPanel.SetActive(false);
        }
    }

    public void ExitBtn()
    {
        IndexPanel.SetActive(false);
    }

    public void ABtn() { FindAlpFirst("A"); }
    public void BBtn() { FindAlpFirst("B"); }
    public void CBtn() { FindAlpFirst("C"); }
    public void DBtn() { FindAlpFirst("D"); }
    public void EBtn() { FindAlpFirst("E"); }
    public void FBtn() { FindAlpFirst("F"); }
    public void GBtn() { FindAlpFirst("G"); }
    public void HBtn() { FindAlpFirst("H"); }
    public void IBtn() { FindAlpFirst("I"); }
    public void JBtn() { FindAlpFirst("J"); }
    public void KBtn() { FindAlpFirst("K"); }
    public void LBtn() { FindAlpFirst("L"); }
    public void MBtn() { FindAlpFirst("M"); }
    public void NBtn() { FindAlpFirst("N"); }
    public void OBtn() { FindAlpFirst("O"); }
    public void PBtn() { FindAlpFirst("P"); }
    public void QBtn() { FindAlpFirst("Q"); }
    public void RBtn() { FindAlpFirst("R"); }
    public void SBtn() { FindAlpFirst("S"); }
    public void TBtn() { FindAlpFirst("T"); }
    public void UBtn() { FindAlpFirst("U"); }
    public void VBtn() { FindAlpFirst("V"); }
    public void WBtn() { FindAlpFirst("W"); }
    public void XBtn() { FindAlpFirst("X"); }
    public void YBtn() { FindAlpFirst("Y"); }
    public void ZBtn() { FindAlpFirst("Z"); }

}
