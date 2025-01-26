using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusWidget : MonoBehaviour 
{
    
    [Range(0f, 1f), SerializeField] private float deflationRate; // value per second, normalized

    [SerializeField] private int maxInflationCount = 6;

    [SerializeField] private Slider inflationBar;
    
    [SerializeField] private Color positiveColor = Color.green;
    [SerializeField] private Color negativeColor = Color.red;


    [SerializeField] private Image fillImage;
    [SerializeField] private AnimationCurve imageColorFillCurve;
    private bool _inflating;
    private bool _tick = true;

    private void Update()
    {
        if(!_tick) return;
        
        Deflate();

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Inflate();
        //}
        LerpColor();
    }

    private void LerpColor()
    {
        var sliderValue = inflationBar.value;
        sliderValue = imageColorFillCurve.Evaluate(sliderValue);
        fillImage.color = Color.Lerp(negativeColor, positiveColor, sliderValue);
        // var t = Mathf.InverseLerp()
    }
    
    private void Deflate()
    {
        if(_inflating) return;

        var deflateValue = deflationRate * Time.deltaTime;

        if(inflationBar.value > 0)
            inflationBar.value -= deflateValue;
    }

    public void Inflate()
    {
        _inflating = true;
        
        inflationBar.value += 1f / maxInflationCount;

        _inflating = false;
    }
    
    
}
