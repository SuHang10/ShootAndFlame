using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    private Text text;
    private Character character;
    private Slider slider;
    void Start()
    {
        text = GetComponentInChildren<Text>();
        character = GetComponentInParent<Character>();
        slider = GetComponentInParent<Slider>();
    }

    void Update()
    {
        float curHP = character.curHP;
        float maxHP = character.maxHP;
        text.text =  curHP.ToString() + "/" + maxHP.ToString();
        slider.value = curHP / maxHP;
    }
}
