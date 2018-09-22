using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

    public float scrollSpeed= .005f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + Vector3.up * scrollSpeed;
	}

    void Pause() {
        scrollSpeed = 0f;
    }

    void Unpause() {
        scrollSpeed = 0.005f;
    }
}
