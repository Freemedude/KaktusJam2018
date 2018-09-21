using System.Collections;
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

    public StaminaBarController StaminaBar;
    private int health = 3;
    private bool isJumping;
    private float maxStamina = 100;
    [SerializeField]
    private float staminaDecreaseValue = 20;
    [SerializeField]
    private float staminaIncreaseValue = 25;
    private float currentStamina;


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

	    currentStamina = maxStamina;
    }
	
	// Update is called once per frame
	void Update () {
		var hori = Input.GetAxis("Horizontal");
		var vert = Input.GetAxis("Vertical");
	
		HandleMovement(hori, vert);
	}

	void HandleMovement(float hori, float vert)
	{
		Vector2 movementVector = Vector2.zero;
		
		// If we're jumping
		if(vert > 0 && currentStamina >= staminaDecreaseValue)
		{
			movementVector = new Vector2(hori * movementSpeed, vert * jumpHeight);
			ChangeState(PlayerState.Jumping);
		    isJumping = true;
		}

		// Of we're crouching
		else if(vert < 0)
		{
		    isJumping = false;
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

		    isJumping = false;
		}
		rb.AddForce(movementVector);
	    UpdateStamina(isJumping);
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

    private void UpdateHealth()
    {
        //todo
    }

    /// <summary>
    /// Updates the stamina
    /// </summary>
    private void UpdateStamina(bool jumping)
    {
        if (jumping)
            currentStamina -= staminaDecreaseValue;
        else
            currentStamina += staminaIncreaseValue;

        float percentage = currentStamina / maxStamina;
        StaminaBar.ChangeStamina(percentage);
    }
}
