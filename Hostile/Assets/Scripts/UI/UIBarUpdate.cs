using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIBarUpdate : MonoBehaviour
{
    private Image bar;
    public Gradient color;

    private void OnEnable()
    {
        bar = this.GetComponent<Image>();
        if (bar != null)
        {
            bar.color = color.Evaluate(1);
        }
    }

    public void Barupdate(float percent)//float max, float current)
    {
        if (bar != null)
        {
            //float percent = current / max;
            bar.fillAmount = percent;
//            bar.color = color.Evaluate(percent);
        }
    }
}
