using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button btnNewGame;
    public Button btnQuit;
    public Button btnOptions;
    public Button btnLoadGame;
    void Start()
    {
        btnNewGame.onClick.AddListener(btnNewGame_Click);
        btnQuit.onClick.AddListener(btnQuit_Click);
        btnOptions.onClick.AddListener(btnOptions_Click);
        btnLoadGame.onClick.AddListener(btnLoadGame_Click);
        //btnLoadGame.
    }

    void btnNewGame_Click()
    {
        SceneManager.LoadScene(1);
    }
    void btnQuit_Click()
    {
        Application.Quit();
    }
    void btnOptions_Click()
    {
        Application.Quit();
    }
    void btnLoadGame_Click()
    {

    }
}
