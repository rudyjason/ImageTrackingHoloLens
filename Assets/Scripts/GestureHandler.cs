using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GestureHandler : MonoBehaviour {

    private GazeManager gazeManager;
    private GameObject focusedObject;
    private GestureRecognizer gestureRecognizer;
    public FloatingMenu menu;

    // Use this for initialization
    void Start ()
    {
        gazeManager = GetComponentInChildren<GazeManager>();
        gestureRecognizer = new GestureRecognizer();

        initGestures();
    }
	
	// Update is called once per frame
	void Update () {
        if (gazeManager.IsGazingAtObject)
        {
            focusedObject = gazeManager.HitObject;
            Debug.Log("Gazing at: " + focusedObject.name);
        }
        else
        {
            focusedObject = null;
        }
    }

    private void HandleTap(GameObject tappedObject)
    {
        if (tappedObject == null)
        {
            Debug.Log("Tapped on NOTHING");
            if (!menu.IsOpen())
            {
                menu.OpenMenu();
            }
            else
                menu.CloseMenu();
            return;
        }
        else
        {
            Debug.Log("Tapped on: " + tappedObject.name);
        }
    }

    private void initGestures()
    {
        //Tap Event
        gestureRecognizer.TappedEvent += (source, tapCount, ray) =>
        {
            Debug.Log("Tapped event");
            HandleTap(focusedObject);
        };

        //Hold Event
        gestureRecognizer.HoldCompletedEvent += (source, ray) =>
        {
            Debug.Log("Hold Completed Event");
        };

        //Start capturing gestures
        gestureRecognizer.StartCapturingGestures();
    }
}
