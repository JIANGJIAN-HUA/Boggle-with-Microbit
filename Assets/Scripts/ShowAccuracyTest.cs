using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAccuracyTest : MonoBehaviour
{
    public Text ShowText;
    public Text VocText;
    private string Voc;
    
    void Update()
    {
        Voc= VocText.text.ToUpper();
        if (PlayerPrefs.GetInt(Voc+ "Total") == 0)
        {
            ShowText.text = "?";
        }
        else
        {
            ShowText.text = ((double)PlayerPrefs.GetInt(Voc + "Correct") / PlayerPrefs.GetInt(Voc + "Total")).ToString();
        }
        
    }
}
