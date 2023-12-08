using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private ParticleSystem moveDust;
    [SerializeField] private ParticleSystem poofDust;
    [SerializeField] private float tiltAngle = 20f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private Transform characterSpriteTransform;
    [SerializeField] private Transform hatSpriteTransform;
    [SerializeField] private float cowboyHatModifier = 2f;
    [SerializeField] private float yLandVelocityCheck = -10f;
    #endregion

    #region Privates
    private Vector2 _velocityBeforePhysicUpdate;
    private CinemachineImpulseSource _impulseSource;
    private Rigidbody2D _rigidBody;
    #endregion
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void OnEnable()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        PlayerController.OnJump += PlayPoofDustVFX;
    }
    private void PlayPoofDustVFX()
    {
        poofDust.Play();
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    private void UnSubscribeEvents()
    {
        PlayerController.OnJump -= PlayPoofDustVFX;
    }

    private void Update()
    {
        DetectMoveDust();
        ApplyTilt();
    }
    private void FixedUpdate()
    {
        _velocityBeforePhysicUpdate = _rigidBody.velocity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_velocityBeforePhysicUpdate.y < yLandVelocityCheck)
        {
            PlayPoofDustVFX();
            _impulseSource.GenerateImpulse();
        }
    }
    private void DetectMoveDust()
    {
        if (PlayerController.Instance.CheckGrounded())
        {
            if (!moveDust.isPlaying)
            {
                moveDust.Play();
            }
        }
        else
        {
            if (moveDust.isPlaying)
            {
                moveDust.Stop();
            }
        }
    }
    private void ApplyTilt()
    {
        float targetAngle;
        if(PlayerController.Instance.MoveInput.x < 0f)
        {
            targetAngle = tiltAngle;
        }
        else if(PlayerController.Instance.MoveInput.x > 0f)
        {
            targetAngle = -tiltAngle;
        }
        else
        {
            targetAngle = 0f;
        }

        Quaternion currentCharacterRotation = characterSpriteTransform.rotation;
        Quaternion targetCharacterRotation = Quaternion.Euler(currentCharacterRotation.eulerAngles.x,
            currentCharacterRotation.eulerAngles.y, 
            targetAngle);

        characterSpriteTransform.rotation = Quaternion.Slerp(currentCharacterRotation,targetCharacterRotation, tiltSpeed * Time.deltaTime);

        Quaternion currentHatRotation = hatSpriteTransform.rotation;
        Quaternion targetHatRotation = Quaternion.Euler(currentHatRotation.eulerAngles.x,
            currentHatRotation.eulerAngles.y,
            -targetAngle / cowboyHatModifier);

        hatSpriteTransform.rotation = Quaternion.Slerp(currentHatRotation, targetHatRotation, tiltSpeed * cowboyHatModifier * Time.deltaTime);
    }
}
