using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;

public class Fade : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float fadeTime = 1.5f;

    private Image _image;
    private CinemachineVirtualCamera _virtualCam;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
    }
    public void FadeInAndOut()
    {
        StartCoroutine(FadeIn());
    }
    private IEnumerator FadeIn()
    {
        yield return StartCoroutine(FadeRoutine(1f));
        Respawn();
        StartCoroutine(FadeRoutine(0f));
    }
    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float elapsedTime = 0f;
        float startValue = _image.color.a;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetAlpha, elapsedTime/fadeTime);
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, newAlpha);
            yield return null;
        }

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, targetAlpha);
    }
    private void Respawn()
    {
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = respawnPoint.position;
        _virtualCam.Follow = player.transform;
    }
}
