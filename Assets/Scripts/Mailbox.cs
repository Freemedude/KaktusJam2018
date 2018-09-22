using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mailbox : MonoBehaviour {

    private GameObject spawnpoint, mainCamera, player;
    public DialogueManager dialogueManager;
    private Component dialogueTriggerScript;
    public bool firstCollision = true;
    public bool inDialogue = false;
	// Use this for initialization
	void Start () {
        spawnpoint = GameObject.Find("SpawnPoint");
        mainCamera = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
        if (GetComponent("DialogueTrigger") != null) {
            dialogueTriggerScript = GetComponent("DialogueTrigger");
        } else {

        }
    }
	
	// Update is called once per frame
	void Update () {
        if(inDialogue) {
            if(!dialogueManager.dialogueIsOpen) {
                inDialogue = false;
                Unpause();
            }
        }
	}

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "Player" && firstCollision) {
            dialogueTriggerScript.SendMessage("TriggerDialogue");
            movePlayerSpawnPoint();
            Pause();

            firstCollision = false;
        }
    }

    void movePlayerSpawnPoint() {
        spawnpoint.transform.position = this.transform.position + Vector3.up * 1.5f;
    }

    void Pause() {
        inDialogue = true;
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        mainCamera.SendMessage("Pause");
        player.SendMessage("Pause");
        for (var i = 0; i < gos.Length; i++) {
            gos[i].SendMessage("Pause");
        }
    }

    void Unpause() {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        mainCamera.SendMessage("Unpause");
        player.SendMessage("Unpause");
        for (var i = 0; i < gos.Length; i++) {
            gos[i].SendMessage("Unpause");
        }
    }
}
