using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpotLight : MonoBehaviour
{
    [SerializeField] private GameObject spotLightHead;
    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] float discoRotationSpeed = 120f;
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
    public IEnumerator SpotLightDiscoParty(float discoParty)
    {
        float _defaultRotSpeed = rotationSpeed;
        rotationSpeed = discoRotationSpeed;
        yield return new WaitForSeconds(discoParty);
        rotationSpeed = _defaultRotSpeed;
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
