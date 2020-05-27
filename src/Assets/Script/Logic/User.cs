using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User 
{
    private int hp, clip, level, exp, damage;
    private float pos_x, pos_y, pos_z;
    private string name;

    public void Initialize(Dictionary<string, string> dict)
    {
        /*foreach(var each in dict)
        {
            Debug.Log(each.Key + "=" + each.Value);
        }*/

        if (dict.ContainsKey("account"))
            name = dict["account"];
        else
            Debug.Log("未获得account信息");
        if (dict.ContainsKey("hp"))
            hp = int.Parse(dict["hp"]);
        else
            Debug.Log("未获得hp信息");
        if (dict.ContainsKey("clip"))
            clip = int.Parse(dict["clip"]);
        else
            Debug.Log("未获得clip信息");
        if (dict.ContainsKey("level"))
            level = int.Parse(dict["level"]);
        else
            Debug.Log("未获得level信息");
        if (dict.ContainsKey("exp"))
            exp = int.Parse(dict["exp"]);
        else
            Debug.Log("未获得exp信息");
        if (dict.ContainsKey("damage"))
            damage = int.Parse(dict["damage"]);
        else
            Debug.Log("未获得damage信息");
        try
        {
            if (dict.ContainsKey("pos_x"))
                pos_x = float.Parse(dict["pos_x"]);
            else
                Debug.Log("未获得pos_x信息");
            if (dict.ContainsKey("pos_y"))
                pos_y = float.Parse(dict["pos_y"]);
            else
                Debug.Log("未获得pos_y信息");
            if (dict.ContainsKey("pos_z"))
                pos_z = float.Parse(dict["pos_z"]);
            else
                Debug.Log("未获得pos_z信息");
        }
        catch(Exception e)
        {
            pos_x = 0;
            pos_y = 0;
            pos_z = 0;
        }
    }
    public string GetName() { return name; }
    public void SetName(string s) { name = s; }
    public int GetHP(){ return hp; }
    public void SetHp(int i){ hp = i; }
    public int GetClip(){ return clip; }
    public void SetClip(int i){ clip = i; }
    public int GetLevel(){ return level; }
    public void SetLevel(int i){ level = i; }
    public int GetEXP() { return exp; }
    public void SetEXP(int i) { exp = i; }
    public void AddEXP() { exp += 5; }
    public int GetDamage() { return damage; }
    public void SetDamage(int i) { damage = i; }
    public float GetPos_X() { return pos_x; }
    public void SetPos_X(float i) { pos_x = i; }
    public float GetPos_Y() { return pos_y; }
    public void SetPos_Y(float i) { pos_y = i; }
    public float GetPos_Z() { return pos_z; }
    public void SetPos_Z(float i) { pos_z = i; }
}
