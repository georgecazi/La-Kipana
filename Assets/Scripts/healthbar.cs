using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image FillImage;
    private Slider slider;




    private void Awake()
    {
        slider = GetComponent<Slider>();
    }


    // Update is called once per frame
    void Update()
    {
        float fillValue = playerHealth.currentHealth / playerHealth.maxHealth;
        if (slider.value <= slider.minValue)
        {
        FillImage.enabled = false;
        }
        if (slider.value >= slider.minValue && !FillImage.enabled)
        {
            FillImage.enabled = true;
        }
        if (fillValue <= slider.maxValue / 3)
        {
            FillImage.color = Color.yellow;
        }
        else if (fillValue > slider.maxValue / 3)
        {
            FillImage.color = Color.green;
        }
        slider.value = fillValue;
    }
}
