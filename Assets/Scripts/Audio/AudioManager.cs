using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundSO gunShoot;
    [SerializeField] private SoundSO jumpSO;

    [SerializeField] private GameObject SoundObject;
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        Gun.OnShoot += OnShoot;
        PlayerController.OnJump += OnJump;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void UnSubscribeEvents()
    {
        Gun.OnShoot -= OnShoot;
        PlayerController.OnJump -= OnJump;
    }
    private void PlaySound(SoundSO soundSO)
    {
        //GameObject soundObject = new GameObject("Temp Audio Source");
        //AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        AudioSource audioSource = SoundObject.GetComponent<AudioSource>();
        audioSource.clip = soundSO.Clip;
        audioSource.Play();
    }
    private void OnShoot()
    {
        PlaySound(gunShoot);
    }
    private void OnJump()
    {
        PlaySound(jumpSO);
    }

}
