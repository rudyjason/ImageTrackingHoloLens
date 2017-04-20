using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setLookat : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
        transform.LookAt(target.transform);
	}
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(target.transform);
    }
}
