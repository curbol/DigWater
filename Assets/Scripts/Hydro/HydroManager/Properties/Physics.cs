using System;
using UnityEngine;

[Serializable]
public class Physics
{
    [SerializeField]
    private float gravityScale;
    public float GravityScale
    {
        get
        {
            return gravityScale;
        }
    }

    [SerializeField]
    private float angularDrag;
    public float AngularDrag
    {
        get
        {
            return angularDrag;
        }
    }

    [SerializeField]
    private float mass;
    public float Mass
    {
        get
        {
            return mass;
        }
    }

    [SerializeField]
    private float maximumVelocity;
    public float MaximumVelocity
    {
        get
        {
            return maximumVelocity;
        }
    }
}