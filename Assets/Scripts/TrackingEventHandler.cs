using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class TrackingEventHandler : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    public bool extendedTracking = false;
    public string labelText;
    private float persistenceTimer;
    private bool visible;

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        Debug.Log("NEW TRACKING STATUS: " + newStatus.ToString());
        if(newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.DETECTED || (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED))
        {
            persistenceTimer = 0;
            visible = true;
            transform.FindChild("target").gameObject.SetActive(true);
            transform.FindChild("target").transform.FindChild("Canvas").gameObject.SetActive(true);
            gameObject.GetComponentInChildren<MeshRenderer>().gameObject.SetActive(true);
            Debug.Log("ENABLING GAMEOBJECT");
        } else
        {
            visible = false;
        }
    }    

    void Start()
    {
        visible = false;
        persistenceTimer = 0;
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        if(labelText.Equals(""))
        {
            string name = gameObject.GetComponent<ImageTargetBehaviour>().TrackableName;
            transform.FindChild("target").transform.FindChild("Canvas").GetComponentInChildren<Text>().text = name;
        } else
        {
            transform.FindChild("target").transform.FindChild("Canvas").GetComponentInChildren<Text>().text = labelText;
        }

    }

    void Update()
    {
        if(!extendedTracking && !visible)
        {
            persistenceTimer += Time.deltaTime;
            if (persistenceTimer > 1)
            {
                persistenceTimer = 0;
                transform.FindChild("target").gameObject.SetActive(false);
                transform.FindChild("target").transform.FindChild("Canvas").gameObject.SetActive(false);
                //gameObject.GetComponentInChildren<MeshRenderer>().gameObject.SetActive(false);
                Debug.Log("DISABLING GAMEOBJECT");
            }
        }

    }
}
