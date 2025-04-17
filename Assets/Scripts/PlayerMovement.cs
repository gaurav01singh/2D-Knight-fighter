using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public int PlayerHP;
    public Slider healthBar;
    public Text scoreText;
    private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    public float speed = 5f;
    public float jumpForce = 10f;
    private Animator anim;
    private float horizontalMove;
    private bool isGrounded;
    public LayerMask isGround;
    [SerializeField] float ground_radius = 0.2f;
    public Transform groundCheck;
    private bool jump;
    public Transform attackPos;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int damage = 30;
    public float delay = 0.5f;
    public ParticleSystem HitEffect;
    private int MaxHealth = 100;
    private int score = 0;
    private UIManagner UIManagner;
    public static Action OnPlayerHit;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        scoreText = FindFirstObjectByType<Text>();
        healthBar = FindFirstObjectByType<Slider>();
        UIManagner = FindFirstObjectByType<UIManagner>();
    }

    private void Start()
    {
        PlayerHP = MaxHealth;
        healthBar.maxValue = MaxHealth;
        healthBar.value = MaxHealth;
        score = PlayerPrefs.GetInt("Score", 0);
        UpdateScoreUI();// Fix for CS0029  
    }

    private void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");

        // Stop movement when attacking  
        if (!anim.GetBool("Attacking"))
        {
            rb.linearVelocity = new Vector2(horizontalMove * speed, rb.linearVelocity.y);
            anim.SetBool("IsRunning", horizontalMove != 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetBool("IsRunning", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            anim.SetTrigger("Jump");
            jump = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jump = false;
            anim.ResetTrigger("Jump");
            anim.SetBool("Down", true);
        }

        if (horizontalMove > 0.01f)
        {
            transform.localScale = new Vector3(2f, 2f, 1f);
        }
        else if (horizontalMove < -0.01f)
        {
            transform.localScale = new Vector3(-2f, 2f, 1f);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, ground_radius, isGround);

        if (isGrounded)
        {
            anim.SetBool("Down", false);
        }

        if (Input.GetButtonDown("Fire1") && isGrounded)
        {
            anim.SetBool("Attacking", true);
            
            rb.linearVelocity = Vector2.zero; // Ensure player stops moving immediately  
        }
        else if (Input.GetButtonUp("Fire1") || !isGrounded)
        {
            anim.SetBool("Attacking", false);
        }
    }

    void FixedUpdate()
    {
        if (jump && isGrounded)
        {
            jump = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (!isGrounded)
        {
            anim.SetBool("Down", true);
        }
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        PlayerHP -= damage;
        healthBar.value = PlayerHP;

        if (HitEffect != null)
            HitEffect.Play();

        if (PlayerHP <= 0)
            Die();

        OnPlayerHit?.Invoke();
    }

    private void Die()
    {
        if (rb != null) Destroy(rb);
        if (col != null) Destroy(col);
        UIManagner.ShowGameOverPanel();
        anim.SetTrigger("PDie");
        Destroy(gameObject, delay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coins"))
        {
            score++;
            UpdateScoreUI();
            Destroy(collision.gameObject);
        }
    }
    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score", score);
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }
}
