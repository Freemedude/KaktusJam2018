﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [Header("Player movement settings")]
    public float flapCooldown;
    private float flapCooldownCounter = 0;

    public float movementSpeed;
    public float flapStrength;
    private Rigidbody2D rb;
    private bool paused = false;

    public GameObject gameOverScreen;
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
    private AudioSource[] sounds;
    public Camera mainCamera;

    public StartDialogue startDialogue;
    public Image fade;
    public Image star;

    /*** Dialogue management ***/
    public DialogueManager dialogueManager;

    /*** State management ***/
    public bool isFlying;
    public bool isHolding;
    public bool isMoving;
    public enum Facing {
        Left, Right
    }
    public Facing facing;

    private bool isGrounded = true;
    public float distance = 2f;
    public LayerMask layerMask;

    // Use this for initialization
    void Start() {
        Time.timeScale = 1;
        // Get components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        sounds = GetComponents<AudioSource>();
        facing = Facing.Left;

        // Default to idle spriterenderer
        UpdateState();

        // get the health controller and stamina controller in the scene
        HealthController = FindObjectOfType<HealthController>();
        StaminaBarController = FindObjectOfType<StaminaBarController>();
        currentHearts = maxHearts;
        HealthController.DrawHearts(currentHearts);
        currentStamina = maxStamina;
        fade.CrossFadeAlpha(0, .0f, true);
        star.CrossFadeAlpha(0, .0f, true);

        Pause_All();
        
        startDialogue.StartStory();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (paused) {
            if(!dialogueManager.dialogueActive && startDialogue.hasFinished)
            {
                Unpause_All();
            }
            rb.isKinematic = true;
            rb.velocity = new Vector3 (0,0,0);
            return;
        }
        
        rb.isKinematic = false;

        flapCooldownCounter -= Time.deltaTime;

        var hori = Input.GetAxis("Horizontal");
        var jump = Input.GetKeyDown("joystick 1 button 0");
        string[] controllers = Input.GetJoystickNames();
        if (controllers.Length == 0 || controllers[0] == "")
            jump = Input.GetKeyDown(KeyCode.Space);
        else
            jump = Input.GetKeyDown("joystick 1 button 0");

        CheckGround();

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
            spriteRenderer.flipX = false;
        }
        else if (hori > 0) {
            facing = Facing.Right;
            spriteRenderer.flipX = true;
        }

        // If we're jumping
        if (flapped && currentStamina >= staminaDecreaseValue) {
            sounds[1].Play();
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
        if (isHolding)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isFlying", false);
            animator.SetBool("isIdleMail", true);
            animator.SetBool("isWalkingMail", false);
            animator.SetBool("isFlyingMail", false);
        }
        else
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isFlying", false);
            animator.SetBool("isIdleMail", false);
            animator.SetBool("isWalkingMail", false);
            animator.SetBool("isFlyingMail", false);
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
            animator.SetBool("isWalking", false);
            animator.SetBool("isFlying", false);
            animator.SetBool("isIdleMail", false);
            animator.SetBool("isWalkingMail", true);
            animator.SetBool("isFlyingMail", false);
        }
        else
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isFlying", false);
            animator.SetBool("isIdleMail", false);
            animator.SetBool("isWalkingMail", false);
            animator.SetBool("isFlyingMail", false);
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
            animator.SetBool("isFlying", false);
            animator.SetBool("isIdleMail", false);
            animator.SetBool("isWalkingMail", false);
            animator.SetBool("isFlyingMail", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isFlying", true);
            animator.SetBool("isIdleMail", false);
            animator.SetBool("isWalkingMail", false);
            animator.SetBool("isFlyingMail", false);
        }
    }

    
    // Pauses all pausable gameobjects
    void Pause_All() 
    {
        Pause();
        var gos = GameObject.FindGameObjectsWithTag("Enemy");
        mainCamera.SendMessage("Pause");
        for (var i = 0; i < gos.Length; i++) {
            gos[i].SendMessage("Pause");
        }
    }


    // Unpauses all pausable gameobjects
    void Unpause_All() 
    {
        Unpause();
        var gos = GameObject.FindGameObjectsWithTag("Enemy");
        mainCamera.SendMessage("Unpause");
        for (var i = 0; i < gos.Length; i++) {
            gos[i].SendMessage("Unpause");
        }
    }


    void Pause() {
        paused = true;
    }

    void Unpause() {
        paused = false;
    }

    /// <summary>
	/// If the player hits the ground, say we're not flying.
	/// If the player gets hit by an enemy, decrease health.
    /// </summary>
	private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy")
            DecreaseHealth();
    }


    /// <summary>
    /// If the player leaves ground, say we are flying
    /// </summary>
    void CheckGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * distance, Color.green);
        isGrounded = Physics2D.Raycast(transform.position, Vector3.down, distance, layerMask);
        isFlying = !isGrounded;
    }

    /// <summary>
    /// Decreases the health and check if the player is game over.
    /// </summary>
    public void DecreaseHealth() {
        currentHearts--;

        HealthController.UpdateHearts(currentHearts);

        if (currentHearts <= 0)
            GameOver();
        else {
            //isHolding = false;
            this.transform.position = spawnPoint.transform.position;
        }
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
    gameOverScreen.SetActive(true);
        mainCamera.SendMessage("Pause");
        StartCoroutine(Wait(3));
    }

    IEnumerator Wait(float x) {
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("FinalGame");
    }

    IEnumerator WaitForEnd(float x) {
        fade.CrossFadeAlpha(1, 4.0f, true);
        star.CrossFadeAlpha(1, 4.0f, true);
        Pause();
        yield return new WaitForSeconds(x);
        SceneManager.LoadScene("StartScreen");
    }

    //On trigger enter test
    private void OnTriggerEnter2D(Collider2D col) 
    {
    

        if(col.gameObject.tag == "WinZone")
        {
            Time.timeScale = .25f;
            StartCoroutine(WaitForEnd(2));
        }

        if (col.gameObject.tag == "Mail")
        {
            sounds[0].Play(); //Yeah!
            isHolding = true;
            Destroy(col.gameObject);
        }
    }

    private void MailDelivered() {
        isHolding = false;
    }

}