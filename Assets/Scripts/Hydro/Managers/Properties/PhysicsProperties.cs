using System;
using UnityEngine;

[Serializable]
public class PhysicsProperties
{
    [SerializeField]
    private float mass = 1;
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
    private float gravityScale = 1;
    public float GravityScale
    {
        get
        {
            return gravityScale;
        }
    }

    [SerializeField]
    private float horizontalMaxVelocity = 15;
    public float MaxVelocityX
    {
        get
        {
            return horizontalMaxVelocity;
        }
    }

    [SerializeField]
    private float verticalMaxVelocity = 15;
    public float MaxVelocityY
    {
        get
        {
            return verticalMaxVelocity;
        }
    }
}