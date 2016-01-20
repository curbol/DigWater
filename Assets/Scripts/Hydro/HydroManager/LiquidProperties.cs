using System;
using UnityEngine;

[Serializable]
public class LiquidProperties
{
    [SerializeField]
    private Color color;
    public Color Color
    {
        get
        {
            return color;
        }
    }
}