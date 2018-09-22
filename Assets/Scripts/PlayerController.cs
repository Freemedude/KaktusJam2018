using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	[Header("Player graphic settings")]
	public bool startFacingRight;
	public SpriteRenderer playerSpriteRenderer;

	public Sprite playerSpriteIdleLeft;
	public Sprite playerSpriteIdleRight;
	public Sprite playerSpriteRunLeft;
	public Sprite playerSpriteRunRight;
	public Sprite playerSpriteFlyLeft;
	public Sprite playerSpriteFlyRight;


	[Header("Player movement settings")]
	public float flapCooldown;
	public float flapCooldownCounter = 0;


	public float movementSpeed;
	public float flapStrength;
	private Rigidbody rb;
    private bool paused = false;

    public GameObject spawnPoint;
    public HealthController HealthController;
    public StaminaBarController StaminaBarController;
    private int maxHearts = 3;
    private int currentHearts;
    private float maxStamina = 100;
    [SerializeField]
    private float staminaDecreaseValue = 1.0f;
    [SerializeField]
    private float staminaIncreaseValue = 1.5f;
    private float currentStamina;


	/*** State management ***/

    public bool isFlying;
	public bool isHolding;
	public bool isMoving;
	public enum Facing {
		Left, Right
	}
	public Facing facing;


	// Use this for initialization
	void Start ()
	{
		// Get components
		rb = GetComponent<Rigidbody>();

		// Default to idle spriterenderer
		UpdateState();

		facing = startFacingRight ? Facing.Right : Facing.Left;

	    currentHearts = maxHearts;
        HealthController.DrawHearts(currentHearts);
	    currentStamina = maxStamina;
    }
	
	// Update is called once per frame
	void Update ()
	{
		if(paused)
		{
			return;
		}

		flapCooldownCounter -= Time.deltaTime;
		
		var hori = Input.GetAxis("Horizontal");
		var jump = Input.GetKeyDown("joystick 1 button 0");

		isMoving = (hori != 0f || isFlying);

		HandleMovement(hori, jump);
	}


	void HandleMovement(float hori, bool jump)
	{
		var flapped = false;
		if(jump && flapCooldownCounter < 0)
		{
			flapped = jump;
			flapCooldownCounter = flapCooldown;
		}

		if(hori < 0)
		{
			facing = Facing.Left;
		}
		else if(hori > 0)
		{
			facing = Facing.Right;
		}
		
		// If we're jumping
		if(flapped && currentStamina >= staminaDecreaseValue)
		{
			rb.AddForce(Vector3.up * flapStrength);
		}

		// We're not jumping or crouching
		transform.Translate(new Vector2(movementSpeed * Time.deltaTime * hori, 0));

		UpdateState();
		UpdateStamina(flapped);
    }

	public void UpdateState()
	{
		// If we do not move, set sprite to idle with correct facing
		if(!isMoving)
		{
			switch(facing)
			{
				case Facing.Left:
					SetSprite(playerSpriteIdleLeft);
					break;
				case Facing.Right:
					SetSprite(playerSpriteIdleRight);
					break;
			}	
		}
		// We are moving
		else
		{
			// We are not flying
			if(!isFlying)
			{
				// We are not holding anything, so just set sprite to movement in correct direction
				if(!isHolding)
				{
					switch(facing)
					{
						case Facing.Left:
							SetSprite(playerSpriteRunLeft);
							break;
						case Facing.Right:
							SetSprite(playerSpriteRunRight);
							break;
					}
				}
				// We are holding something, so set holding with correct direction
				// todo: Implement this with actual sprites
				else {
					switch(facing)
					{
						case Facing.Left:
							SetSprite(playerSpriteRunLeft);
							break;
						case Facing.Right:
							SetSprite(playerSpriteRunRight);
							break;
					}
				}
				
			}
			// We are flying
			else {
				// We are not holding anything, so just set sprite to movement in correct direction
				if(!isHolding)
				{
					switch(facing)
					{
						case Facing.Left:
							SetSprite(playerSpriteFlyLeft);
							break;
						case Facing.Right:
							SetSprite(playerSpriteFlyRight);
							break;
					}
				}
				// We are holding something and flying, set direction
				// TODO: DO this
				else {
					switch(facing)
					{
						case Facing.Left:
							SetSprite(playerSpriteFlyLeft);
							break;
						case Facing.Right:
							SetSprite(playerSpriteFlyRight);
							break;
					}
				}
			}

		}
	}

	void SetSprite(Sprite sprite)
	{
		playerSpriteRenderer.sprite = sprite;
	}

    void Pause() {
        paused = true;
    }

    void Unpause() {
        paused = false;
    }

    /// <summary>
    /// If the player gets hit by an enemy, update the health.
	/// If the player hits the ground, say we're not flying.
    /// </summary>
    /// <param name="col"></param>
    void OnColliderEnter(Collider col)
    {
        if (col.tag == "Enemy")
            DecreaseHealth();
			
		UpdateState();
    }

    /// <summary>
	/// If the player hits the ground, say we're not flying.
    /// </summary>
	private void OnCollisionEnter(Collision col)
	{
			
		if(col.gameObject.tag == "Ground")
		{
			isFlying = false;
		}
	}


    /// <summary>
    /// If the player leaves ground, say we are flying
    /// </summary>
	private void OnCollisionExit(Collision other)
	{
		if(other.gameObject.tag == "Ground")
		{
			isFlying = true;
		}
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
        this.transform.position = spawnPoint.transform.position;
        currentHearts = maxHearts;
        currentStamina = maxStamina;

        HealthController.RestartHearts();
        StaminaBarController.ChangeStamina(currentStamina);
    }
}
