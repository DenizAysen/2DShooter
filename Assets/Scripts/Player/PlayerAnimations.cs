using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem moveDust;
    [SerializeField] private float tiltAngle = 20f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private Transform characterSpriteTransform;
    private void Update()
    {
        DetectMoveDust();
        ApplyTilt();
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
    }
}
