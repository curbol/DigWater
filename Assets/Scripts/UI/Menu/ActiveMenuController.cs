using UnityEngine;

public class ActiveMenuController : MonoBehaviour
{
    [SerializeField]
    private CanvasRenderer[] menuScreenPrefabs;

    private CanvasRenderer[] menuScreens;

    public void SetCurrentMenuScreen(int index)
    {
        CanvasRenderer menuScreen = null;

        if (index <= menuScreens.Length - 1)
            menuScreen = menuScreens[index];

        SetCurrentMenuScreen(menuScreen);
    }

    private void SetCurrentMenuScreen(CanvasRenderer menuScreen)
    {
        HideAllMenus();
        SetMenuScreenActive(menuScreen, true);
    }

    private void HideAllMenus()
    {
        foreach (CanvasRenderer menuScreen in menuScreens)
            SetMenuScreenActive(menuScreen, false);
    }

    private static void SetMenuScreenActive(CanvasRenderer menuScreen, bool isActive)
    {
        if (menuScreen != null)
            menuScreen.gameObject.SetActive(isActive);
    }

    private void Awake()
    {
        menuScreens = new CanvasRenderer[menuScreenPrefabs.Length];
        for (int i = 0; i < menuScreens.Length; i++)
            menuScreens[i] = transform.InstantiateChild(menuScreenPrefabs[i]);

        SetCurrentMenuScreen(0);
    }
}