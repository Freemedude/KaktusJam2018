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
<<<<<<< HEAD
    private bool paused = false;
=======

    public HealthController HealthController;
    public StaminaBarController StaminaBarController;
    private int maxHearts = 3;
    private int currentHearts;
    private bool isJumping;
    private float maxStamina = 100;
    [SerializeField]
    private float staminaDecreaseValue = 1.0f;
    [SerializeField]
    private float staminaIncreaseValue = 1.5f;
    private float currentStamina;

>>>>>>> bba3237d00d3ea4c52e71ca026d5d69f38e11a6e

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

	    currentHearts = maxHearts;
        HealthController.DrawHearts(currentHearts);
	    currentStamina = maxStamina;
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
		if(vert > 0 && currentStamina >= staminaDecreaseValue)
		{
			movementVector = new Vector2(hori * movementSpeed, vert * jumpHeight);
			ChangeState(PlayerState.Jumping);
		    isJumping = true;
		}

		// Of we're crouching
		else if(vert < 0)
		{
			ChangeState(PlayerState.Crouch);
		    isJumping = false;
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

<<<<<<< HEAD
    void Pause() {
        paused = true;
    }

    void UnPause() {
        paused = false;
=======
    /// <summary>
    /// If the player gets hit by an enemy, update the health.
    /// </summary>
    /// <param name="col"></param>
    void OnColliderEnter(Collider col)
    {
        if (col.tag == "Enemy")
            DecreaseHealth();
    }

    /// <summary>
    /// Decreases the health and check if the player is game over.
    /// </summary>
    private void DecreaseHealth()
    {
        currentHearts--;

        HealthController.UpdateHearts(currentHearts);

        if (currentHearts <= 0)
            GameOver();
    }

    /// <summary>
    /// Updates the stamina.
    /// </summary>
    private void UpdateStamina(bool jumping)
    {
        if (jumping)
            currentStamina -= staminaDecreaseValue;
        else if (currentStamina < maxStamina)
            currentStamina += staminaIncreaseValue;

        float percentage = currentStamina / maxStamina;
        StaminaBarController.ChangeStamina(percentage);
    }

    /// <summary>
    /// When the player is game over, he spawns at the checkpoint.
    /// </summary>
    void GameOver()
    {
        // todo spawn at checkpoint
        currentHearts = maxHearts;
        currentStamina = maxStamina;

        HealthController.RestartHearts();
        StaminaBarController.ChangeStamina(currentStamina);
>>>>>>> bba3237d00d3ea4c52e71ca026d5d69f38e11a6e
    }
}
