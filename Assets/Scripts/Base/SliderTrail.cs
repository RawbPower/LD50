using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTrail : MonoBehaviour
{
    public Slider slider;
    public Slider trailSlider;
    public float delayTime;
    public float trailRate;
    public float trailSpeed;

    public SliderTrail parentSlider;

    private bool trailActive;
    private float trailCounter;
    private float delayCounter;
    private bool shouldTrail;

    // Start is called before the first frame update
    void Start()
    {
        trailCounter = 0.0f;
        delayCounter = 0.0f;
        shouldTrail = false;
        trailActive = parentSlider == null;
        trailSlider.value = slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        bool justActivated = false;
        if (!trailActive && (parentSlider == null || parentSlider.trailSlider.value == 0.0f))
        {
            trailActive = true;
            justActivated = true;
        }

        if (!shouldTrail && trailActive && trailSlider.value != slider.value)
        {
            shouldTrail = true;
            if (justActivated)
            {
                delayCounter = 0.0f;
            }
            else
            {
                delayCounter = delayTime;
            }
            trailCounter = trailRate;
        }

        if (slider.value > trailSlider.value)
        {
            trailSlider.value = slider.value;
        }

        if (trailSlider.value == slider.value)
        {
            shouldTrail = false;
            delayCounter = 0.0f;
            trailCounter =  0.0f;
        }

        if (shouldTrail && delayCounter > 0)
        {
            delayCounter -= Time.deltaTime;
        }
        else if (shouldTrail)
        {
            if (trailCounter <= 0.0f)
            {
                trailSlider.value = trailSlider.value - trailSpeed;
                trailCounter = trailRate;
            }
            else
            {
                trailCounter -= Time.deltaTime;
            }
        }
    }
}
