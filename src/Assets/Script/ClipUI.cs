using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipUI : MonoBehaviour
{
    private Text text;
    private PlayerWeapon weapon;
    void Start()
    {
        text = GetComponent<Text>();
        weapon = GetComponentInParent<PlayerWeapon>();
    }

    void Update()
    {
        string clipNum = weapon.GetClipNum().ToString();
        string ammunitionNum = weapon.ammunition.ToString();
        text.text = "弹夹容量：" + clipNum + "/" + ammunitionNum;
    }
}
