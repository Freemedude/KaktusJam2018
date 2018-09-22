using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Player movement settings")]
    public float flapCooldown;
    public float flapCooldownCounter = 0;

    public float movementSpeed;
    public float flapStrength;
    private Rigidbody rb;
    private bool paused = false;
    public float mailToDeliver = 3; //Change this to the number of mailboxes to deliver

    public GameObject spawnPoint;
    private HealthController HealthController;
    private StaminaBarController StaminaBarController;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
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
    void Start() {
        // Get components
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        facing = Facing.Right;

        // Default to idle spriterenderer
        UpdateState();

        // get the health controller and stamina controller in the scene
        HealthController = FindObjectOfType<HealthController>();
        StaminaBarController = FindObjectOfType<StaminaBarController>();
        currentHearts = maxHearts;
        HealthController.DrawHearts(currentHearts);
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update() {
        if (paused) {
            return;
        }

        flapCooldownCounter -= Time.deltaTime;

        var hori = Input.GetAxis("Horizontal");
        string[] controllers = Input.GetJoystickNames();
        bool jump = controllers[0] == "" ? Input.GetKeyDown(KeyCode.Space) : Input.GetKeyDown("joystick 1 button 0");

        isMoving = (hori != 0f || isFlying);

        HandleMovement(hori, jump);
    }


    void HandleMovement(float hori, bool jump) {
        var flapped = false;
        if (jump && flapCooldownCounter < 0) {
            flapped = currentStamina >= staminaDecreaseValue;
            flapCooldownCounter = flapCooldown;
        }

        if (hori < 0) {
            facing = Facing.Left;
            spriteRenderer.flipX = true;
        }
        else if (hori > 0) {
            facing = Facing.Right;
            spriteRenderer.flipX = false;
        }

        // If we're jumping
        if (flapped && currentStamina >= staminaDecreaseValue) {
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
        if (!isMoving)
            SetIdleAnimation();
        // We are moving
        else
        {
            // We are not flying
            if (!isFlying)
                SetWalkAnimation();
            // We are flying
            else
                SetFlyAnimation();
        }
    }

    /// <summary>
    /// Sets animation to idle animation
    /// </summary>
    void SetIdleAnimation()
    {
        // todo add holding animation!
        if (isHolding)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isFlying", false);
        }
        else
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isFlying", false);
        }
    }

    /// <summary>
    /// Sets animation to walking animation
    /// </summary>
    void SetWalkAnimation()
    {
        if (isHolding)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isFlying", false);
        }
        // todo add holding animation!
        else
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isFlying", false);
        }
    }

    /// <summary>
    /// Sets animation to flying animation
    /// </summary>
    void SetFlyAnimation()
    {
        if (isHolding)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isFlying", true);
        }
        // todo add holding animation!
        else
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isFlying", true);
        }
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
	void OnColliderEnter(Collider col) {
        if (col.tag == "Enemy")
            DecreaseHealth();

        UpdateState();
    }

    /// <summary>
	/// If the player hits the ground, say we're not flying.
    /// </summary>
	private void OnCollisionEnter(Collision col) {

        if (col.gameObject.tag == "Ground") {
            isFlying = false;
        }
    }


    /// <summary>
    /// If the player leaves ground, say we are flying
    /// </summary>
	private void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Ground") {
            isFlying = true;
        }
    }

    /// <summary>
    /// Decreases the health and check if the player is game over.
    /// </summary>
    public void DecreaseHealth() {
        currentHearts--;

        HealthController.UpdateHearts(currentHearts);

        if (currentHearts <= 0)
            GameOver();
    }

    /// <summary>
    /// Updates the stamina.
    /// </summary>
    private void UpdateStamina(bool jumping) {
        if (jumping)
            currentStamina -= staminaDecreaseValue;
        else if (currentStamina < maxStamina && !isFlying)
            currentStamina += staminaIncreaseValue;

        float percentage = currentStamina / maxStamina;
        StaminaBarController.ChangeStamina(percentage);
    }

    /// <summary>
    /// When the player is game over, he spawns at the checkpoint.
    /// </summary>
    void GameOver() {
        isHolding = false;
        this.transform.position = spawnPoint.transform.position;
        currentHearts = maxHearts;
        currentStamina = maxStamina;

        HealthController.RestartHearts();
        StaminaBarController.ChangeStamina(currentStamina);
    }

    //On trigger enter test
    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Mail") {
            isHolding = true;
            Destroy(col.gameObject);
        }
    }

    private void MailDelivered() {
        isHolding = false;
        if(--mailToDeliver <= 0) {
            //winState();
        }
    }
}