using System;
using UnityEngine;

public abstract class RecyclableObject : MonoBehaviour
{
    public Action OnDeath { get; set; }
}