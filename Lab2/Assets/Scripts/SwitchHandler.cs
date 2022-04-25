using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class SwitchHandler : MonoBehaviour
{
    public bool state;
    private bool state2;
    private Color32[] color;
    public Image imageBtn;


    public void Start()
    {
        color = new Color32[2];
        color[0] = new Color32(60, 250, 236, 255);
        color[1] = new Color32(255, 0, 0, 255);
    }
    public void Update()
    {
        if (state != state2)
        {
            state2 = state;
            if (state == true)
            {
                imageBtn.DOColor(color[0], 2f);
            }
            else imageBtn.DOColor(color[1], 2f);
        }
    }

    public void OnClick()
    {
        state = !state2;
    }

}