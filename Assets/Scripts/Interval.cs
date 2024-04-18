using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interval : MonoBehaviour
{
    public static Interval Instance;
    private void Awake()
    {
        Instance = this;
    }
    public TextMeshProUGUI IntervalText;
    public Slider IntervalSlider;

    public void setIntervalText()
    {
        IntervalText.text = IntervalSlider.value+ "s";
    }
}
