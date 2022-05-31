using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginMenu : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public Button btnLogin;
    public Button btnQuit;
    public TMP_Text invalidDataLabel;
    // Start is called before the first frame update
    void Start()
    {
        btnLogin.onClick.AddListener(btnLogin_Click);
        btnQuit.onClick.AddListener(btnQuit_Click);
    }

    void btnQuit_Click()
    {
        Application.Quit();
    }

    void btnLogin_Click()
    {
        if(username.text=="admin" && password.text=="admin")
        {
            SceneManager.LoadScene(1,LoadSceneMode.Single);
        }else if(username.text=="")
        {
            invalidDataLabel.text = "enter username";
        }else
            invalidDataLabel.text = "invalid username or password";
    }

}
