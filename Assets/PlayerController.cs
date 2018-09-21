﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	[Header("Player graphic settings")]
	public SpriteRenderer playerSpriteRenderer;

	public Sprite playerSpriteIdle;
	public Sprite playerSpriteLeft;
	public Sprite playerSpriteRight;
	public Sprite playerSpriteJump;
	public Sprite playerSpriteCrouch;

	[Header("Player movement settings")]
	public float movementSpeed;
	public float jumpHeight;
	private Rigidbody rb;
    private bool paused = false;

	/*** State management ***/
	public enum PlayerState {
		Idle, 
		MovingLeft, 
		MovingRight, 
		Jumping,
		Crouch
    }
	[HideInInspector]
	public PlayerState currentState;


	// Use this for initialization
	void Start () {
		// Get components
		rb = GetComponent<Rigidbody>();
		// Default to idle spriterenderer
		ChangeState(PlayerState.Idle);
	}
	
	// Update is called once per frame
	void Update () {
        //if (paused = false) { Add unpause bedore you readd this
            var hori = Input.GetAxis("Horizontal");
            var vert = Input.GetAxis("Vertical");

            HandleMovement(hori, vert);
        //}
	}

	void HandleMovement(float hori, float vert)
	{
		Vector2 movementVector = Vector2.zero;
		
		// If we're jumping
		if(vert > 0)
		{
			movementVector = new Vector2(hori * movementSpeed, vert * jumpHeight);
			ChangeState(PlayerState.Jumping);
		}

		// Of we're crouching
		else if(vert < 0)
		{
			ChangeState(PlayerState.Crouch);
		}

		// We're not jumping or crouching
		else {
			movementVector = new Vector2(hori * movementSpeed, 0);

			if(hori < 0)
			{
				ChangeState(PlayerState.MovingLeft);
			}
			else if(hori > 0)
			{
				ChangeState(PlayerState.MovingRight);
			}

			// We're not moving at all
			else {
				ChangeState(PlayerState.Idle);
			}
		}
		rb.AddForce(movementVector);
	}

	public void ChangeState(PlayerState newState)
	{
		if(newState == currentState)
		{
			return;
		}
		switch(newState)
		{
			case PlayerState.Idle:
				SetSprite(playerSpriteIdle);
				break;	
			case PlayerState.MovingLeft:
				SetSprite(playerSpriteLeft);
				break;	
			case PlayerState.MovingRight:
				SetSprite(playerSpriteRight);
				break;	
			case PlayerState.Jumping:
				SetSprite(playerSpriteJump);
				break;	
			case PlayerState.Crouch:
				SetSprite(playerSpriteCrouch);
				break;	
		}

	}


	void SetSprite(Sprite sprite)
	{
		playerSpriteRenderer.sprite = sprite;
	}

    void Pause() {
        paused = true;
    }

    void UnPause() {
        paused = false;
    }
}
