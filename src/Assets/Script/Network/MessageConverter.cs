using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WebManager
{
    public static class MessageConverter
    {
        public static Dictionary<string,string> LogInMsg(string username, string password)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["calling"] = "login";
            dict["username"] = username;
            dict["password"] = password;
            return dict;
        }

        public static Dictionary<string, string> LogInACKMsg(string username)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["calling"] = "login_ack";
            dict["username"] = username;
            return dict;
        }

        public static Dictionary<string, string> LogOutMsg(string username)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["calling"] = "logout";
            dict["username"] = username;
            return dict;
        }

        public static Dictionary<string, string> RegisterMsg(string username, string password)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["calling"] = "register";
            dict["username"] = username;
            dict["password"] = password;
            return dict;
        }

        public static Dictionary<string, string> OnEnemyUpdateMsg(Enemy enemy)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["calling"] = "enemy_update";
            dict["name"] = enemy.name;
            dict["hp"] = enemy.curHP.ToString();
            dict["pos_x"] = enemy.transform.position.x.ToString();
            dict["pos_y"] = enemy.transform.position.y.ToString();
            dict["pos_z"] = enemy.transform.position.z.ToString();
            return dict;
        }

        public static Dictionary<string, string> OnEnemyDeadMsg(Enemy enemy)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["calling"] = "enemy_dead";
            dict["name"] = enemy.name;
            return dict;
        }

        public static Dictionary<string, string> OnUserUpdateMsg(User user)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["calling"] = "user_update";
            dict["name"] = user.GetName();
            dict["hp"] = user.GetHP().ToString();
            dict["clip"] = user.GetClip().ToString();
            dict["level"] = user.GetLevel().ToString();
            dict["exp"] = user.GetEXP().ToString();
            dict["pos_x"] = user.GetPos_X().ToString();
            //dict["pos_y"] = ((int)user.GetPos_Y()).ToString();
            dict["pos_z"] = user.GetPos_Z().ToString();
            return dict;
        }

        public static Dictionary<string, string> OnUserDeadMsg(User user)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["calling"] = "user_dead";
            dict["name"] = user.GetName();
            dict["level"] = user.GetLevel().ToString();
            dict["exp"] = user.GetEXP().ToString();
            dict["pos_x"] = ((int)user.GetPos_X()).ToString();
            dict["pos_z"] = ((int)user.GetPos_Z()).ToString();
            return dict;
        }


        public static Dictionary<string, string> OnHealthMsg(string chapter, string hp)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["calling"] = "onhealth";
            dict["chapter"] = chapter;
            dict["hp"] = hp;
            return dict;
        }

        public static Dictionary<string, string> OnClipMsg(string chapter, string clip)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["calling"] = "onclip";
            dict["chapter"] = chapter;
            dict["clip"] = clip;
            return dict;
        }
    }
}
