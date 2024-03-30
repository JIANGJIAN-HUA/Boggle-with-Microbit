using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAppear : MonoBehaviour
{
    public GameObject MainMenuBtn;
    public GameObject LargeBtn;

    public void MenuAppearBtn()
    {
        MainMenuBtn.SetActive(true);
        LargeBtn.SetActive(false);
    }
}
