using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

    public float scrollSpeed= .005f;
    public GameObject spawnPoint;
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

    void GameOver() {
        Vector3 offset = new Vector3(-spawnPoint.transform.position.x, 0, -10);
        transform.position = spawnPoint.transform.position + offset;
    }
}
