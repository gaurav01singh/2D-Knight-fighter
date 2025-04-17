using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    [SerializeField] private Animator animator;
    public float delay = 0.5f;
    public Transform player;
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool gettingHit = false;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    public float SpaceBetween = 1.0f;
    public bool SelfCollide = false;
    public ParticleSystem HitEffect;
    public float detectionRadius = 0.5f;
    public LayerMask enemyLayer;
    public GameObject PointA;
    public GameObject PointB;
    private Transform currentPoint;
    public int damage = 20;
    [SerializeField] private float attackCooldown =1;
    [SerializeField] private float range=1;
    [SerializeField] private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField]private float coliderDistance = 0.5f;
    [SerializeField] private float speed = 2f;
    



    [System.Obsolete]
    private void OnValidate()
    {
        animator = GetComponent<Animator>();
     player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        
    }
    private void Start()
    {
        currentPoint = PointA.transform;
    }

    private void Update()
    {
        //if (!isDead && !gettingHit && player != null)
        //{
        //    MoveTowardsPlayer();
        //}

        Vector2 direction = currentPoint.position - transform.position;
        if(currentPoint == PointB.transform)
        {
            if(!PlayerInRange())
            {
                rb.linearVelocity = new Vector2(speed, 0);
                animator.SetBool("IsWalking", true);
            }
            
        }
        else
        {
            if (!PlayerInRange())
            {
                rb.linearVelocity = new Vector2(-speed, 0);
                animator.SetBool("IsWalking", true);
            }
        }
           
        if (Vector2.Distance(transform.position, currentPoint.position) < 2f  )
        {
            
            currentPoint = currentPoint.position == PointA.transform.position ? PointB.transform : PointA.transform;
            transform.rotation = Quaternion.Euler(0, transform.rotation.y == 0 ? 180 : 0, 0);
        }
        


        cooldownTimer += Time.deltaTime;
        if(PlayerInRange() && cooldownTimer >= attackCooldown)
        {
            animator.SetBool("Attack", true);
            cooldownTimer = 0;
            animator.SetBool("IsWalking", false);


        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }
    private bool PlayerInRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
    col.bounds.center + transform.right * range * transform.localScale.x * coliderDistance,
    new Vector2(col.bounds.size.x * range, col.bounds.size.y),
    0, Vector2.left, 0, playerLayer
);
        if (hit.collider == null)
        {
            animator.SetBool("IsWalking", true);
        }
        return hit.collider != null;
    }
    private void DamagePlayer(int damage)
    {
        PlayerMovement playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
        if (playerMovement != null && PlayerInRange())
        {
            playerMovement.TakeDamage(damage);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            col.bounds.center + transform.right * range * transform.localScale.x*coliderDistance,
            new Vector3(col.bounds.size.x * range, col.bounds.size.y, col.bounds.size.z)
        );
    }
    public void TakeDamage(int damage)
    {
        if (animator != null)
        {
            gettingHit = true;
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("GotHit");
            Invoke("ResetHitState", 0.6f);
        }
        HitEffect.Play();
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }
    void ResetHitState()
    {
        gettingHit = false;
    }
    void Die()
    {
        isDead = true;
        if (animator != null)
        {
            animator.ResetTrigger("GotHit");
            animator.SetTrigger("Die");
        }
        if (rb != null) Destroy(rb); 
        

        if (col != null) Destroy(col); 

        else
        {
            Debug.LogWarning("Spawner is not assigned to the enemy.");
        }

        // Destroy the enemy after a delay to allow animation to play
        Destroy(gameObject, delay);
    }

    //void MoveTowardsPlayer()
    //{
    //    if (player != null)
    //    {
    //        if (Vector2.Distance(transform.position, player.position) > SpaceBetween && !SelfCollide)
    //        {
    //            animator.SetBool("IsWalking", true);
                
    //            transform.position = Vector2.MoveTowards(transform.position, player.position, 2 * Time.deltaTime);
    //        }
    //        else
    //        {
    //            animator.SetBool("IsWalking", false);
    //        }
    //    }
    //    if (player.position.x < transform.position.x)
    //    {
    //        transform.localScale = new Vector3(-0.15f, 0.15f, 0.15f);
    //    }
    //    else
    //    {
    //        transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
    //    }
    //}
    //void MoveonSpecificRoute()
    //{
    //    // Move on a specific route
    //    transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 0), 2 * Time.deltaTime);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            SelfCollide = true;
            animator.SetBool("IsWalking", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            // Check if no other enemies are in front
            Collider2D[] enemiesNearby = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
            if (enemiesNearby.Length == 0)
            {
                SelfCollide = false;
            }
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Debug.Log(animator.GetBool("Attack"));
    //        animator.SetBool("Attack", true);
    //        collision.GetComponent<PlayerMovement>().TakeDamage(damage);
    //    }
        
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        animator.SetBool("Attack", false);
    //    }
    //}
}