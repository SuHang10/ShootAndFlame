using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public Slider sliderHP;
    private Soldier[] soldiers;
    void Start()
    {
        soldiers = FindObjectsOfType<Soldier>();
    }

    void Update()
    {
        /*soldiers = FindObjectsOfType<SoldierAI>();
        foreach(var sol in soldiers)
        {
            Slider slider = Instantiate(sliderHP);
            slider.transform.position = sol.transform.position + Vector3.up * 3;
        }*/
    }
}
