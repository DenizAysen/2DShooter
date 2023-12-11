using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpotLight : MonoBehaviour
{
    [SerializeField] private GameObject spotLightHead;
    public float rotationSpeed = 20f;
    [SerializeField] private float maxRotation = 45f;

    private float _currentRotation;
    private void Start()
    {
        RandomStartingRotation();
    }
    private void Update()
    {
        RotateHead();
    }
    private void RotateHead()
    {
        _currentRotation += Time.deltaTime * rotationSpeed;
        float z = Mathf.PingPong(_currentRotation, maxRotation);
        spotLightHead.transform.localRotation = Quaternion.Euler(0f, 0f, z);
    }
    private void RandomStartingRotation()
    {
        float randomStartingZ = Random.Range(-maxRotation, maxRotation);
        spotLightHead.transform.localRotation = Quaternion.Euler(0f, 0f, randomStartingZ);
        _currentRotation = randomStartingZ + maxRotation;
    }
}
