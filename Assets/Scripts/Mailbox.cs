using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mailbox : MonoBehaviour {

    private GameObject spawnpoint;
    private Component dialogueTrigger;
    public bool firstCollision = true;
	// Use this for initialization
	void Start () {
        spawnpoint = GameObject.Find("SpawnPoint");
        if (GetComponent("DialogueTrigger") != null) {
            dialogueTrigger = GetComponent("DialogueTrigger");
        } else {

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "Player" && firstCollision) {
            //pause()
            dialogueTrigger.SendMessage("TriggerDialogue");
            movePlayerSpawnPoint();

            firstCollision = false;
        }
    }

    void movePlayerSpawnPoint() {
        spawnpoint.transform.position = this.transform.position;
    }
}
