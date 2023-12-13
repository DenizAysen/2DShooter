using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    public static Action OnDiscoBallHitEvent;

    [SerializeField] private float discoBallPartyTime = 2f;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private float discoGlobalLightIntensity = .2f;

    private float _defaultGlobalLightIntensity;
    private Coroutine _discoCoroutine;
    private ColorSpotLight[] _allSpotLigts;
    private void Awake()
    {
        _defaultGlobalLightIntensity = globalLight.intensity;
    }
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
    public void DiscoBallParty()
    {
        if (_discoCoroutine != null) 
            return;

        OnDiscoBallHitEvent?.Invoke();
    }

    private void DimTheLights()
    {
        foreach (ColorSpotLight spotLight in _allSpotLigts)
        {
            StartCoroutine(spotLight.SpotLightDiscoParty(discoBallPartyTime));
        }
        _discoCoroutine = StartCoroutine(GlobalLightResetRoutine());

    }
   private IEnumerator GlobalLightResetRoutine()
    {
        globalLight.intensity = discoGlobalLightIntensity;
        yield return new WaitForSeconds(discoBallPartyTime);
        globalLight.intensity = _defaultGlobalLightIntensity;
        _discoCoroutine = null;
    }
}
