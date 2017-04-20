using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    private Text hudText;

    private bool countdown;
    private float counter;
    public float showTime = 3.0f;
    private AfterHUDCallback cb;
    public Camera TagalongCamera;
    public GameObject gazeCursor;

    // Use this for initialization
    void Start () {
        hudText = GetComponent<Text>();

        countdown = false;
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(countdown)
        {
            counter += Time.deltaTime;
            if(counter > showTime)
            {
                countdown = false;
                counter = 0;
                hudText.enabled = false;
                hudText.text = "";
                if(cb != null)
                {
                    cb();
                }
            }
        }
	}

    public void ShowText(string text, bool timed)
    {
        hudText.enabled = true;
        hudText.text = text;
        countdown = timed;
    }

    public void SetShowTime(float seconds)
    {
        showTime = seconds;
    }

    public void SetDelegate(AfterHUDCallback del)
    {
        cb = del;
    }

    public void RunTimer(float seconds)
    {
        showTime = seconds;
        countdown = true;
        hudText.enabled = true;
    }

    public void DisableText()
    {
        hudText.enabled = false;
    }

    public void SetTagalongDistance(float distance)
    {
        TagalongCamera.orthographicSize = distance;
    }

    public delegate void AfterHUDCallback();
}
