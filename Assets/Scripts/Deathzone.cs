using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathzone : MonoBehaviour {
    // Use this for initialization
    public bool collided = false;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.name == "Player") {
            collided = true;
            col.gameObject.SendMessage("GameOver");
            this.transform.parent.SendMessage("GameOver");
        }
    }
}
