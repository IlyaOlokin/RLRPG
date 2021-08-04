using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpSlider : MonoBehaviour
{
    [SerializeField] private Slider sliderHp;
    [SerializeField] private Slider sliderWhiteHp;
    private Transform parent;

    private bool dmgTakenRecently;

    private void Start()
    {
        parent = transform.parent;
        transform.SetParent(null);
    }

    public void SetMaxValue(float sliderMaxValue)
    {
        sliderHp.maxValue = sliderMaxValue;
        sliderWhiteHp.maxValue = sliderMaxValue;
        sliderHp.value = sliderHp.maxValue;
        sliderWhiteHp.value = sliderWhiteHp.maxValue;
    }

    private void Update()
    {
        KeepRotationUp();
        UpdateWhiteSlider();
    }

    private void KeepRotationUp()
    {
        //transform.eulerAngles = Vector3.zero;
        transform.position = parent.position + new Vector3(0, 0.6f, 0);
    }

    public void TakeDmg(float dmg)
    {
        sliderHp.value -= dmg;
        StartCoroutine(WhiteSliderDelay());
    }

    public void TakeHeal(float healAmount)
    {
        sliderHp.value += healAmount;
    }

    private void UpdateWhiteSlider()
    {
        if (sliderWhiteHp.value <= sliderHp.value)
        {
            sliderWhiteHp.value = sliderHp.value;
            dmgTakenRecently = false;
        }
        else if (dmgTakenRecently)
        {
            sliderWhiteHp.normalizedValue -= 0.15f * Time.deltaTime;
        }
    }

    private IEnumerator WhiteSliderDelay()
    {
        yield return new WaitForSeconds(0.7f);
        dmgTakenRecently = true;
    }
}