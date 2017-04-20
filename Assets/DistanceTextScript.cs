using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTextScript : MonoBehaviour {

    public DestinationIndication logic;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        UnityEngine.UI.Text text = GetComponent<UnityEngine.UI.Text>();
        text.text = "Distance to object: " + logic.distance + "\nDistance to location: " + logic.distance2;
    }
}
