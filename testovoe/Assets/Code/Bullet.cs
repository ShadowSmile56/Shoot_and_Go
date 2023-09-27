using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 10f; // �������� �������
    public float lifetime = 2f; // ����� ����� �������
    public int damage = 1; // ����, ������� ������� ������
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // ������ ��������� �������� �������
        if (player.facingRight)
            rb.velocity = transform.right * speed;
        else rb.velocity = transform.right*-1 * speed;
        // ���������� ������ ����� ��������� �����
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
