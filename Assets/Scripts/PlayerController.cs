using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	[Header("Player graphic settings")]
	public SpriteRenderer playerSpriteRenderer;

	public Sprite playerSpriteIdle;
	public Sprite playerSpriteLeft;
	public Sprite playerSpriteRight;
	public Sprite playerSpriteFlyLeft;
	public Sprite playerSpriteFlyRight;

    public Sprite playerSpriteIdleMail;
    public Sprite playerSpriteLeftMail;
    public Sprite playerSpriteRightMail;
    public Sprite playerSpriteFlyLeftMail;
    public Sprite playerSpriteFlyRightMail;

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
    private bool isFlying, holdingMail;
    private float maxStamina = 100;
    [SerializeField]
    private float staminaDecreaseValue = 1.0f;
    [SerializeField]
    private float staminaIncreaseValue = 1.5f;
    private float currentStamina;

	/*** State management ***/
	public enum PlayerState {
		Idle, 
		MovingLeft, 
		MovingRight, 
		FlyingLeft, 
		FlyingRight,

        IdleMail,
        MovingLeftMail,
        MovingRightMail,
        FlyingLeftMail,
        FlyingRightMail,

    }
	[HideInInspector]
	public PlayerState currentState;


	// Use this for initialization
	void Start ()
	{
		// Get components
		rb = GetComponent<Rigidbody>();

		// Default to idle spriterenderer
		ChangeState(PlayerState.Idle);

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
		var jump = Input.GetKey("joystick 1 button 0");
	
		HandleMovement(hori, jump);

	}

	void HandleMovement(float hori, bool jump)
	{
		var flapped = false;
		if(flapCooldownCounter < 0)
		{
			flapped = jump;
			flapCooldownCounter = flapCooldown;
		}
		var movingLeft = hori < 0;
		var movingRight = hori > 0;


		// If we're jumping
		if(flapped && currentStamina >= staminaDecreaseValue)
		{
			rb.AddForce(Vector3.up * flapStrength);
		    isFlying = true;
		}

		// We're not jumping or crouching
		transform.Translate(new Vector2(movementSpeed * Time.deltaTime * hori, 0));

		if(!flapped)
		{
			if(hori < 0)
			{
                if (!holdingMail) {
                    ChangeState(PlayerState.MovingLeft);
                } else {
                    ChangeState(PlayerState.MovingLeftMail);
                }
			}
			else if(hori > 0)
			{
                if (!holdingMail) {
                    ChangeState(PlayerState.MovingRight);
                }
                else {
                    ChangeState(PlayerState.MovingRightMail);
                }
            }

				// We're not moving at all
			else {
                if (!holdingMail) {
                    ChangeState(PlayerState.Idle);
                }
                else {
                    ChangeState(PlayerState.IdleMail);
                }
            }
			isFlying = false;
		}

		
	    UpdateStamina(isFlying);
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
            case PlayerState.IdleMail:
                SetSprite(playerSpriteIdleMail);
                break;
            case PlayerState.MovingLeftMail:
                SetSprite(playerSpriteLeftMail);
                break;
            case PlayerState.MovingRightMail:
                SetSprite(playerSpriteRightMail);
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

    void Unpause() {
        paused = false;
    }
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
        this.transform.position = spawnPoint.transform.position;
        currentHearts = maxHearts;
        currentStamina = maxStamina;

        HealthController.RestartHearts();
        StaminaBarController.ChangeStamina(currentStamina);
    }

    //On trigger enter
    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.name == "Death Zone") {
            col.gameObject.transform.parent.SendMessage("GameOver");
            GameOver();
            holdingMail = false;
        } else if(col.gameObject.tag == "Mail") {
            holdingMail = true;
            Destroy(col.gameObject);
        }
    }

}
