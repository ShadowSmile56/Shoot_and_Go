 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public ControlType controlType;
    public Joystick joystick;
    public float speed;
    public float maxHealth;

    public Gun gun;
    public enum ControlType { PC, Android };

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private Animator anim;
    public bool facingRight = true;
    public float currentHealth;
    public bool isdead = false;


    [SerializeField] HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        healthBar = GetComponentInChildren<HealthBar>();
        isdead = false;
        // Загружаем данные игрока при запуске
        PlayerData savedPlayerData = SaveLoadManager.LoadPlayerData();
        if (savedPlayerData != null)
        {
            currentHealth = savedPlayerData.playerHealth;
            transform.position = savedPlayerData.position;
        }
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (controlType == ControlType.PC)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else if (controlType == ControlType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        moveVelocity = moveInput.normalized * speed;

        if (moveInput.x == 0)
        {
            anim.SetBool("isRunning", false);

        }
        else
        {
            anim.SetBool("isRunning", true);


        }

        if (!facingRight && moveInput.x > 0)
        {
            Flip();

        }
        else if (facingRight && moveInput.x < 0)
        {
            Flip();

        }
        SaveLoadManager.SavePlayerData(new PlayerData { playerHealth = currentHealth, position = transform.position, currentAmmo = gun.GetCurrentAmmo(), isdead=isdead });
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    public void TakeDamage(int damage)
    {
        // Уменьшаем здоровье игрока на величину урона
        currentHealth -= damage;
        print(currentHealth);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        // Обработка смерти игрока
        if (currentHealth <= 0)
        {
            // Здесь можно добавить анимацию смерти и другие действия при смерти игрока

            // Загружаем текущую сцену заново (перезагрузка сцены)
            isdead = true;
            SetInitialValues();
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
    public void SetInitialValues()
    {
        currentHealth = maxHealth;

        SaveLoadManager.SavePlayerData(new PlayerData { playerHealth = maxHealth, position = new Vector2(0, 0), currentAmmo = 30, isdead=isdead });

    }
}