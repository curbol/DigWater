using System;
using UnityEngine;

[Serializable]
public class SoilTypeSettings
{
    [SerializeField]
    private int seed;
    public int Seed
    {
        get
        {
            return seed;
        }

        set
        {
            seed = value;
        }
    }
}