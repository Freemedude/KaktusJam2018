using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mailbox : MonoBehaviour {
    private bool mailDelivered;
    public GameObject mainCamera; // Probably need to reference for pausing?
    private GameObject spawnpoint;
	// Use this for initialization
	void Start () {
        spawnpoint = GameObject.Find("SpawnPoint");
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "Player") {
            //pause()
            //showtextbox()
            setPlayerSpawn();
        }
    }

    public void setPlayerSpawn() {
        spawnpoint.transform.position = this.transform.position;
    }
    
}
