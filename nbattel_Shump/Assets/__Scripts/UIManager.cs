using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Sprite[] lives;
    public Image livesImageDisplay;
    public GameObject titleScreen;
    public GameObject[] levels;


    public void UpdateLives(int currentLives)
    {
        livesImageDisplay.sprite = lives[currentLives];        
    }

    public void HideTitleScreen()
    {
        titleScreen.SetActive(false);
    }

    public void ShowTitleScreen()
    {
        titleScreen.SetActive(true);
    }

    public void ShowLevelOne()
    {
        levels[0].SetActive(true);
    }

    public void HideLevelOne()
    {
        levels[0].SetActive(false);
    }

    public void ShowLevelTwo()
    {
        levels[1].SetActive(true);
    }

    public void HideLevelTwo()
    {
        levels[1].SetActive(false);
    }
}
