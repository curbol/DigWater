using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LocationObjective2D
{
    [SerializeField]
    private Location2D location;
    public Location2D Location
    {
        get
        {
            return location;
        }
    }

    [SerializeField]
    private Toggle toggle;
    public Toggle Toggle
    {
        get
        {
            return toggle;
        }
    }
}