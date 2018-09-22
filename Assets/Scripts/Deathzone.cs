using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathzone : MonoBehaviour {
    // Use this for initialization
    public bool collided = false;
    public GameObject spawnPoint;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            collided = true;
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            player.DecreaseHealth();
            col.gameObject.transform.position = spawnPoint.transform.position;
        }
    }
}
