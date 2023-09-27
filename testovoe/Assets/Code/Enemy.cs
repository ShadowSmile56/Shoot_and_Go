using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float speed;
    public float detectionRadius = 5f; // Радиус обнаружения игрока
    public float attackRadius = 1f; // Радиус атаки игрока
    public float attackCooldown = 1f; // Время между атаками
    public int attackDamage = 1; // Урон, наносимый игроку

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

        // Проверяем расстояние до игрока
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (distanceToPlayer <= attackRadius)
            {
                // Игрок в радиусе атаки
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    // Проводим атаку, если прошло достаточно времени с последней атаки
                    AttackPlayer();
                }
                else
                {
                    // Ожидаем следующей атаки, останавливаем анимацию ходьбы
                    StopMoving();
                }
            }
            else
            {
                // Игрок в радиусе обнаружения, бежим к нему
                MoveTowardsPlayer();
            }
        }
        else
        {
            // Игрок не в радиусе обнаружения, останавливаемся
            StopMoving();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 moveDirection = (player.position - transform.position).normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // Определяем направление, в котором смотрит враг
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
        // Останавливаем движение
        // Здесь можно добавить анимацию остановки, если есть

        // Устанавливаем анимацию покоя
        anim.SetBool("IsRunning", false);
    }

    void AttackPlayer()
    {
        anim.SetBool("IsRunning", false);
        // Наносим урон игроку
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(attackDamage);
        }

        // Запоминаем время последней атаки
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
        // Обработка смерти врага
        // Например, включение анимации, воспроизведение звука и т. д.
        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
}

