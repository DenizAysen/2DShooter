using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBallManager : MonoBehaviour
{
    [SerializeField] private float discoBallPartyTime = 2f;

    public static Action OnDiscoBallHitEvent;
    private ColorSpotLight[] _allSpotLigts;
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        OnDiscoBallHitEvent += DimTheLights;
    }
    private void Start()
    {
        _allSpotLigts = FindObjectsOfType<ColorSpotLight>();
    }

    private void OnDisable()
    {
        UnSubscribeEvents();  
    }

    private void UnSubscribeEvents()
    {
        OnDiscoBallHitEvent -= DimTheLights;
    }

    private void DimTheLights()
    {
        StartCoroutine(DimTheLightsRoutine(_allSpotLigts));
    }
    private IEnumerator DimTheLightsRoutine(ColorSpotLight[] colorSpotLights)
    {
        foreach (ColorSpotLight colorSpotLight in colorSpotLights)
        {
            Debug.Log(colorSpotLight.gameObject.name + " default rotate speed : " + colorSpotLight.rotationSpeed);
            colorSpotLight.rotationSpeed *= 3f;
            Debug.Log(colorSpotLight.gameObject.name + " new rotate speed : " + colorSpotLight.rotationSpeed);
        }

        yield return new WaitForSeconds(discoBallPartyTime);

        foreach (ColorSpotLight colorSpotLight in colorSpotLights)
        {
            colorSpotLight.rotationSpeed /= 3;
        }
    }
}
