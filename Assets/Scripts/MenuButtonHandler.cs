using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonHandler : MonoBehaviour
{

    void Start()
    {

    }

    public virtual void ButtonClicked(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                Debug.Log("BUTTON 1 PRESSED");
                //do stuff
                break;
            case 1:
                Debug.Log("BUTTON 2 PRESSED");
                //do stuff
                break;
            case 2:
                Debug.Log("BUTTON 3 PRESSED");
                //do stuff
                break;
        }
    }

    public virtual void ButtonVertClicked(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                Debug.Log("DETAIL BUTTON 1 PRESSED");
                //do stuff
                break;
            case 1:
                Debug.Log("DETAIL BUTTON 2 PRESSED");
                //do stuff
                break;
            case 2:
                Debug.Log("DETAIL BUTTON 3 PRESSED");
                //do stuff
                break;
        }
    }
}
