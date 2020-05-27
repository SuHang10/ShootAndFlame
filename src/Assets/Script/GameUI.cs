using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WebManager;
using Logic;

public class GameUI : MonoBehaviour
{
    public Button button;
    private bool shootOption_One = true;
    private GameObject ShootOption_One_Text;
    private GameObject ShootOption_Three_Text;
    void Start()
    {
        button.onClick.AddListener(MyClick);
        ShootOption_One_Text = GameObject.Find("ShootOption_One");
        ShootOption_Three_Text = GameObject.Find("ShootOption_Three");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            shootOption_One = !shootOption_One;
        ShootOption_One_Text.SetActive(shootOption_One);
        ShootOption_Three_Text.SetActive(!shootOption_One);
    }

    private void MyClick()
    {
        string username = LogicMain.chapter.GetName();
        var logoutMsg = MessageConverter.LogOutMsg(username);
        Client.SendMsg(logoutMsg);
        SceneManager.LoadScene("Login");
    }
}
