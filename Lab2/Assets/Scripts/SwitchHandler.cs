using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class SwitchHandler : MonoBehaviour
{
    public bool switchState;
    private bool switchState2;
    // public GameObject switchBtn;
    // private float curX;
    // public Image img;
    private Color32[] cols;
    public Image imageBtn;
    // public RectTransform a;


    public void Start()
    {
        // curX=Mathf.Abs(switchBtn.transform.localPosition.x);
        // switchState2=switchState=(switchBtn.transform.localPosition.x<0)?false:true;;
        cols = new Color32[2];
        cols[0] = new Color32(60, 250, 236, 255);
        cols[1] = new Color32(255, 0, 0, 255);
    }
    public void Update()
    {
        if (switchState != switchState2)
        {
            switchState2 = switchState;
            if (switchState == true)
            {
                imageBtn.DOColor(cols[0], 2f);
            }
            else imageBtn.DOColor(cols[1], 2f);
        }
    }

    public void OnSwitchButtonClicked()
    {
        switchState = !switchState2;

    }


}