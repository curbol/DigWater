using System.Linq;
using UnityEngine;

public class ActiveMenuController : MonoBehaviour
{
    [SerializeField]
    private NamedMenuScreen[] menuScreens;

    public void SetCurrentMenuScreen(int index)
    {
        NamedMenuScreen menuScreen = null;

        if (index <= menuScreens.Length - 1)
            menuScreen = menuScreens[index];

        SetCurrentMenuScreen(menuScreen);
    }

    public void SetCurrentMenuScreen(string title)
    {
        SetCurrentMenuScreen(menuScreens.FirstOrDefault(a => a.Title == title));
    }

    private void SetCurrentMenuScreen(NamedMenuScreen namedMenuScreen)
    {
        HideAllMenus();
        SetNamedMenuScreenActive(namedMenuScreen, true);
    }

    private void HideAllMenus()
    {
        foreach (NamedMenuScreen menuScreen in menuScreens)
            SetNamedMenuScreenActive(menuScreen, false);
    }

    private static void SetNamedMenuScreenActive(NamedMenuScreen namedMenuScreen, bool isActive)
    {
        if (namedMenuScreen != null && namedMenuScreen.MenuScreen != null)
            namedMenuScreen.MenuScreen.gameObject.SetActive(isActive);
    }

    private void Awake()
    {
        SetCurrentMenuScreen(0);
    }
}