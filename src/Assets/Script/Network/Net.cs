using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Logic;

namespace WebManager
{
    public class Net : MonoBehaviour
    {
        public InputField usernameInputField;
        public InputField passwordInputField;
        public InputField portInputField;
        public Button loginButton, registerButton;
        public GameObject msgText;
        private int count = 0;
        private Text usernameText, passwordText, portText;
        private bool portHasBind = false;

        void Start()
        {
            /*DontDestroyOnLoad(this);
            if (GameObject.Find(name).gameObject != this.gameObject)
                Destroy(this.gameObject);*/

            LogicMain.Initialize();
            LogicMain.logInEvent.AddListener(s => { ShowMsg(s); });
            Client.Initialize();
            usernameText = usernameInputField.GetComponentInChildren<Text>();
            passwordText = passwordInputField.GetComponentInChildren<Text>();
            portText = portInputField.GetComponentInChildren<Text>();
            loginButton.onClick.AddListener(LogInClick);
            registerButton.onClick.AddListener(RegisterClick);
        }

        void Update()
        {
            if(LogicMain.ackTask.ContainsKey("login_ack"))
            {
                var login_ackList = LogicMain.ackTask["login_ack"];
                if (login_ackList.Count > 0)
                {
                    foreach (var ack in login_ackList)
                    {
                        if (ack["state"] == "1")
                        {
                            Debug.Log("收到ACK，登陆成功");
                            LogicMain.chapter.Initialize(ack);
                            SceneManager.LoadScene("Game");

                            var login_ackMsg = MessageConverter.LogInACKMsg(ack["account"]);
                            Client.SendMsg(login_ackMsg);
                        }
                        else
                        {
                            Debug.Log("收到ACK,登陆失败");
                            LogicMain.logInEvent.Invoke(ack["msg"]);
                        }
                    }
                }
                LogicMain.ackTask["login_ack"].Clear();
            }
            if (LogicMain.ackTask.ContainsKey("register_ack"))
            {
                var register_ackList = LogicMain.ackTask["register_ack"];
                if (register_ackList.Count > 0)
                {
                    foreach (var ack in register_ackList)
                        LogicMain.logInEvent.Invoke(ack["msg"]);
                }
                LogicMain.ackTask["register_ack"].Clear();
            }
            if (count == 0)
                msgText.SetActive(false);
            else
                count--;
        }

        private void ShowMsg(string s="")
        {
            var t = msgText.GetComponent<Text>();
            Debug.Log(s);
            t.text = s;
            msgText.SetActive(true);
            count = 100;
        }

        private void LogInClick()
        {
            string username = usernameText.text;
            string password = passwordText.text;
            /*string port = portText.text;
            if(!portHasBind)
            {
                Exception e = Client.BindPort(port);
                if (e != null)
                {
                    ShowMsg("端口错误");
                    return;
                }
                else
                    portHasBind = true;
            }*/

            var loginMsg = MessageConverter.LogInMsg(username, password);
            Client.SendMsg(loginMsg);
        }

        private void RegisterClick()
        {
            string username = usernameText.text;
            string password = passwordText.text;
            /*string port = portText.text;
            if (!portHasBind)
            {
                Exception e = Client.BindPort(port);
                if (e != null)
                {
                    ShowMsg("端口错误");
                    return;
                }
                else
                    portHasBind = true;
            }*/
            var registerMsg = MessageConverter.RegisterMsg(username, password);
            Client.SendMsg(registerMsg);
        }
    }
}
