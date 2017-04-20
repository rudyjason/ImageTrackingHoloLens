using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButtonHandler : MenuButtonHandler {

    public GameObject logic;
    private FloatingMenu menu;

	// Use this for initialization
	void Start () {
        menu = GetComponent<FloatingMenu>();
	}

    public override void ButtonClicked(int buttonIndex)
    {
        switch (buttonIndex) {
            case 0:
                RestartPicking();
                menu.CloseMenu();
                break;
            case 1:
                EnableTestTracking();
                menu.CloseMenu();
                break;
            case 2:
                DisableTestTracking();
                menu.CloseMenu();
                break;
            case 3:
                if (!menu.IsOpenDetail())
                    menu.OpenDetailMenu();
                else
                    menu.CloseDetailMenu();
                break;
        }

    }

    public override void ButtonVertClicked(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                ChangeLocationType(true);
                break;
            case 1:
                ChangeLocationType(false);
                break;
            default:
                menu.CloseMenu();
                break;
        }
    }

    private void RestartPicking()
    {
        logic.GetComponent<DestinationIndication>().RestartPicking();
    }

    private void EnableTestTracking()
    {
        logic.GetComponent<DestinationIndication>().TestTracking(true);
    }

    private void DisableTestTracking()
    {
        logic.GetComponent<DestinationIndication>().TestTracking(false);
    }

    private void ChangeLocationType(bool stuck)
    {
        logic.GetComponent<DestinationIndication>().ChangeLocationType(stuck);
    }
}
