using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject levelSelectScreen;
    public void StartGame()
    {
        //GO TO FIRST SCENE
        SceneManager.LoadScene("Level1");
    }

    public void LevelSelect()
    {
        levelSelectScreen.SetActive(true);

    }


    public void GoToLevelButton(int lvlNumber)
    {
        SceneManager.LoadScene($"Level{lvlNumber}");
    }

    public void LevelSelectBack()
    {
        levelSelectScreen.SetActive(false);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
