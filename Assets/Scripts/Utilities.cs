using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void TestDebugLog(string text)
    {
        Debug.Log("STATIC DEBUG LOG: " + text);
    }
}
