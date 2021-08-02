using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ToxicPuddle : MonoBehaviour
{
    public float timeToScaleDown;
    public float scaleDownTime;

    private float scaleDownSpeed;
    private float defaultScale;
    private new Light2D light;

    private void Start()
    {
        defaultScale = transform.localScale.x;
        light = GetComponent<Light2D>();
        light.pointLightOuterRadius = defaultScale;
        Destroy(gameObject, timeToScaleDown + scaleDownTime);
    }


    void Update()
    {
        timeToScaleDown -= Time.deltaTime;
        if (timeToScaleDown <= 0)
        {
            transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) / scaleDownTime * defaultScale;
            light.pointLightOuterRadius -= Time.deltaTime / scaleDownTime * defaultScale;
        }
    }
    
    
}
