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
    public GameObject about;
    public GameObject mainMenu;
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
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        SceneManager.LoadScene(2,LoadSceneMode.Single);
    }
    void btnQuit_Click()
    {
        Application.Quit();
    }
    void btnOptions_Click()
    {
        about.SetActive(true);
        mainMenu.SetActive(false);
    }
    void btnLoadGame_Click()
    {

    }
}
