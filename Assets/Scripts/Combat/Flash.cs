using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material defaultMat;
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float flashTime = .1f;

    private SpriteRenderer[] _spriteRendererArray;
    private ColorChanger _colorChanger;
    private void Awake()
    {
        _spriteRendererArray = GetComponentsInChildren<SpriteRenderer>();
        _colorChanger = GetComponent<ColorChanger>();
    }
    public void StartFlash()
    {
        StartCoroutine(FlashRoutine());
    }
    private IEnumerator FlashRoutine()
    {
        foreach (SpriteRenderer sr in _spriteRendererArray)
        {
            sr.material = whiteFlashMat;

            if (_colorChanger)
            {
                _colorChanger.SetColor(Color.white);
            }
        }
        yield return new WaitForSeconds(flashTime);

        SetDefaultmaterial();
    }
    private void SetDefaultmaterial()
    {
        foreach (SpriteRenderer sr in _spriteRendererArray)
        {
            sr.material = defaultMat;

            if (_colorChanger)
                _colorChanger.SetColor(_colorChanger.DefaultColor);
        }
    }
}
