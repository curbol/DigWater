using System;
using UnityEngine;

[Serializable]
public class PhysicsProperties
{
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
    private float linearDrag;
    public float LinearDrag
    {
        get
        {
            return linearDrag;
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
    private float gravityScale;
    public float GravityScale
    {
        get
        {
            return gravityScale;
        }
    }

    [SerializeField]
    private float horizontalMaxVelocity;
    public float HorizontalMaxVelocity
    {
        get
        {
            return horizontalMaxVelocity;
        }
    }

    [SerializeField]
    private float verticalMaxVelocity;
    public float VerticalMaxVelocity
    {
        get
        {
            return verticalMaxVelocity;
        }
    }
}