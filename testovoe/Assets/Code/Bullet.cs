using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 10f; // Скорость снаряда
    public float lifetime = 2f; // Время жизни снаряда
    public int damage = 1; // Урон, который наносит снаряд
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // Задаем начальную скорость снаряда
        if (player.facingRight)
            rb.velocity = transform.right * speed;
        else rb.velocity = transform.right*-1 * speed;
        // Уничтожаем снаряд через указанное время
        Destroy(gameObject, lifetime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
