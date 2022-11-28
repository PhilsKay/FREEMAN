using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private string currentState = "IdleState";
    private Transform target;

    public float chaseRange = 5;
    public float attackRange = 2;
    public float speed = 3;

    public int currentHealth;
    public int maxHealth = 100;
    public Slider currentHealthBar;

    public Animator enemyAnim;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealthBar = Slider.FindObjectOfType<Slider>();
        currentHealthBar.value = currentHealth = maxHealth;
        enemyAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (PlayerCombat.gameOver)
        // {
        //     enemyAnim.enabled = false;
        //     this.enabled = false;
        // }

        float distance = Vector3.Distance(transform.position, target.position);

        if(currentState == "IdleState")
        {
            if (distance < chaseRange)
                currentState = "ChaseState";
        }
        else if(currentState == "ChaseState")
        {
            //play the run animation
            enemyAnim.SetTrigger("chase");
            enemyAnim.SetBool("isAttacking", false);

            if(distance < attackRange)
                currentState = "AttackState";

            //move towards the player
            if(target.position.x > transform.position.x)
            {
                //move right
                transform.Translate(transform.right * speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                //move left
                transform.Translate(-transform.right * speed * Time.deltaTime);
                transform.rotation = Quaternion.identity;
            }

        }
        else if(currentState == "AttackState")
        {
            enemyAnim.SetBool("isAttacking", true);

            if (distance > attackRange)
                currentState = "ChaseState";
        }
    //Update health bar UI
        currentHealthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentState = "ChaseState";
        //Play impact animation
        enemyAnim.SetTrigger("Impact");

        if(currentHealth <= 0)
        {
            Die();
        }
    }
    

    private void Die()
    {
        //play a die animation
        enemyAnim.SetTrigger("isDead");

        //disable the script and the collider
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject, 3);
        this.enabled = false;
    }
}