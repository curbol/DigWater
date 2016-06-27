using System;
using UnityEngine;

[Serializable]
public class NamedMenuScreen
{
    [SerializeField]
    private string title;
    public string Title
    {
        get
        {
            return title;
        }
    }

    [SerializeField]
    private CanvasRenderer menuScreen;
    public CanvasRenderer MenuScreen
    {
        get
        {
            return menuScreen;
        }
    }
}