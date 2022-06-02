using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour
{
    public Button btnResume;
    public Button btnQuit;
    public Button btnOptions;
    public Button btnLoadGame;
    public Button btnReturnToMainMenu;
    public Canvas canvas;
    bool openMenu = true;
    void Start()
    {
        btnResume.onClick.AddListener(btnResume_Click);
        btnQuit.onClick.AddListener(btnQuit_Click);
        btnOptions.onClick.AddListener(btnOptions_Click);
        btnLoadGame.onClick.AddListener(btnLoadGame_Click);
        btnReturnToMainMenu.onClick.AddListener(btnReturnToMainMenu_Click);
        canvas.enabled = false;
    }
    internal void setOpenMenu(bool val)
    {
        openMenu = val;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)&&openMenu==true)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            canvas.enabled = true;
        }
    }

    void btnResume_Click()
    {
        canvas.enabled=false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
    void btnQuit_Click()
    {
        Application.Quit();
    }
    void btnOptions_Click()
    {
        //Application.Quit();
    }
    void btnLoadGame_Click()
    {

    }
    void btnReturnToMainMenu_Click()
    {
        SceneManager.UnloadSceneAsync(2);
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }
}
