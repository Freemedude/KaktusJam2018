﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mailbox : MonoBehaviour {

    private GameObject spawnpoint, mainCamera;
    private Component dialogueTriggerScript;
    public bool firstCollision = true;
	// Use this for initialization
	void Start () {
        spawnpoint = GameObject.Find("SpawnPoint");
        mainCamera = GameObject.Find("Main Camera");
        if (GetComponent("DialogueTrigger") != null) {
            dialogueTriggerScript = GetComponent("DialogueTrigger");
        } else {

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "Player" && firstCollision) {
            dialogueTriggerScript.SendMessage("TriggerDialogue");
            movePlayerSpawnPoint();
            pause();

            firstCollision = false;
        }
    }

    void movePlayerSpawnPoint() {
        spawnpoint.transform.position = this.transform.position;
    }

    void pause() {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        mainCamera.SendMessage("Pause");
        for (var i = 0; i < gos.Length; i++) {
            gos[i].SendMessage("Pause");
        }
    }
}