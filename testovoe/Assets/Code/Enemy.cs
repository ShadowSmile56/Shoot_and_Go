using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float speed;
    public float detectionRadius = 5f; // ������ ����������� ������
    public float attackRadius = 1f; // ������ ����� ������
    public float attackCooldown = 1f; // ����� ����� �������
    public int attackDamage = 1; // ����, ��������� ������

    private float currentHealth;
    private Animator anim;
    private bool facingRight = true;
    private Transform player;
    private float lastAttackTime;
    [SerializeField] HealthBar healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }

        // ��������� ���������� �� ������
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (distanceToPlayer <= attackRadius)
            {
                // ����� � ������� �����
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    // �������� �����, ���� ������ ���������� ������� � ��������� �����
                    AttackPlayer();
                }
                else
                {
                    // ������� ��������� �����, ������������� �������� ������
                    StopMoving();
                }
            }
            else
            {
                // ����� � ������� �����������, ����� � ����
                MoveTowardsPlayer();
            }
        }
        else
        {
            // ����� �� � ������� �����������, ���������������
            StopMoving();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 moveDirection = (player.position - transform.position).normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // ���������� �����������, � ������� ������� ����
        if (moveDirection.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveDirection.x < 0 && facingRight)
        {
            Flip();
        }
        anim.SetBool("IsRunning", true);
        anim.SetBool("EnemyAttack", false);
    }

    void StopMoving()
    {
        // ������������� ��������
        // ����� ����� �������� �������� ���������, ���� ����

        // ������������� �������� �����
        anim.SetBool("IsRunning", false);
    }

    void AttackPlayer()
    {
        anim.SetBool("IsRunning", false);
        // ������� ���� ������
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(attackDamage);
        }

        // ���������� ����� ��������� �����
        lastAttackTime = Time.time;
        
        anim.SetBool("EnemyAttack", true);

        
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void Die()
    {
        // ��������� ������ �����
        // ��������, ��������� ��������, ��������������� ����� � �. �.
        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
}

