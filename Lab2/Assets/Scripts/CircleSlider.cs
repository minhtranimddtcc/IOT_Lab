using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CircleSlider : MonoBehaviour
{
    public Image image;
    public float speed;
    public float fill;
    // Start is called before the first frame update
    void Start()
    {
        speed=0.5f;
        fill=0;
    }

    // Update is called once per frame
    void Update()
    {
        fill+=Time.deltaTime*speed;
        image.fillAmount=fill;
        if (fill>1) fill=0;
    }
}
