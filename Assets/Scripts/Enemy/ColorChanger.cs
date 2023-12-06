using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color DefaultColor { get; private set; }

    [SerializeField] private Color[] colors;
    [SerializeField] private SpriteRenderer fillSpriteRenderer;
    public void SetDefaultColor(Color color)
    {
        DefaultColor = color;
        SetColor(color);
    }
    public void SetColor(Color color)
    {
        fillSpriteRenderer.color = color;
    }
    public void SetRandomColor()
    {
        int randomNum = Random.Range(0, colors.Length);
        DefaultColor = colors[randomNum];
        fillSpriteRenderer.color = DefaultColor;
    }
}
