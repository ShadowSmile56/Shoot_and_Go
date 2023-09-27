using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public int maxAmmo = 30; // Максимальное количество патронов
    public int currentAmmo; // Текущее количество патронов
    public float reloadTime = 2.0f; // Время перезарядки в секундах
    private bool isReloading = false; // Флаг перезарядки
    private float lastReloadTime; // Время последней перезарядки
    public Text ammoText;
    
    public Transform firePoint; // Позиция, откуда будут выпускаться снаряды
    public GameObject projectilePrefab; // Префаб снаряда
    public float fireRate = 1f; // Частота стрельбы (выстрелы в секунду)
    public float shootingRange = 10f; // Дальность стрельбы
    public LayerMask targetLayer; // Слой, на котором находятся противники
    public float projectileSpeed = 10f; // Скорость снаряда
    public bool flip;

    public float angle;
    private float timeToFire = 0f;
    private Transform player;
    private void Start()
    {
        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        currentAmmo = maxAmmo;
        // Найдите и сохраните трансформ игрока
        player = GameObject.FindWithTag("Player").transform;
        PlayerData savedPlayerData = SaveLoadManager.LoadPlayerData();
        if (savedPlayerData != null)
        {
            this.currentAmmo = savedPlayerData.currentAmmo;
        }
    }

    private void Update()
    {
        if (isReloading)
        {
            if (Time.time >= lastReloadTime + reloadTime)
            {
                isReloading = false;
                currentAmmo = maxAmmo; // Завершаем перезарядку и восстанавливаем патроны
            }
        }
        else
        {
            if (currentAmmo<=0 )
            {
                // Запускаем перезарядку
                isReloading = true;
                lastReloadTime = Time.time;
                // Добавьте здесь анимацию перезарядки, звуки и т. д.
            }
        }
        ammoText.text = "Ammo: " + currentAmmo.ToString()+"/30";
        // Проверяем, можно ли стрелять (прошло достаточно времени с предыдущего выстрела)
        if (Time.time >= timeToFire)
        {
            // Обнаружение ближайшего противника
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(firePoint.position, shootingRange, targetLayer);
            Transform nearestTarget = GetNearestTarget(hitColliders);

            if (nearestTarget != null)
            {
                // Вычисляем направление к цели и направление взгляда игрока
                // Vector2 targetDirection = ((Vector2)nearestTarget.position - (Vector2)firePoint.position).normalized;
                // Vector2 playerDirection = ((Vector2)player.position - (Vector2)firePoint.position).normalized;

                // Вычисляем угол между направлениями
                // float angleBetweenDirections = Vector2.Angle(targetDirection, playerDirection);

                // Проверяем, что угол меньше определенного значения (например, 30 градусов)
                if (currentAmmo > 0 && !isReloading)
                {
                    AimAtTarget(nearestTarget.position);
                    ShootAtTarget(nearestTarget.position);
                }
            }

            // Устанавливаем время следующего выстрела
            timeToFire = Time.time + 1f / fireRate;
        }
        
    }

    Transform GetNearestTarget(Collider2D[] targets)
    {
        Transform nearestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D target in targets)
        {
            float distanceToTarget = Vector2.Distance(firePoint.position, target.transform.position);
            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                nearestTarget = target.transform;
            }
        }

        return nearestTarget;
    }

    void AimAtTarget(Vector2 targetPosition)
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Vector2 aimDirection = (targetPosition - (Vector2)firePoint.position).normalized;

        angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        if (!player.facingRight)
        {
            // Если игрок смотрит влево, установите угол вращения на 180 градусов
            angle += 180f;
        }

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    public void ShootAtTarget(Vector2 targetPosition)
    {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (!player.facingRight && ((angle >= 270 && angle <= 360) || (angle <= 90 && angle >= 0))) 
        { 
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Vector2 shootDirection = (targetPosition - (Vector2)firePoint.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * projectileSpeed;
            Destroy(bullet, 2f);
            currentAmmo--;
        }
        else if (player.facingRight && ((angle >= -90 && angle <= 90) ))
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Vector2 shootDirection = (targetPosition - (Vector2)firePoint.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * projectileSpeed;
            Destroy(bullet, 2f);
            currentAmmo--;
        }
    }
    public void Shoot()
    {
        if (currentAmmo > 0 && !isReloading)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

            // Выполняйте код выстрела здесь
            // Например, код, который создает снаряд и стреляет им
            // Обязательно уменьшьте `currentAmmo` после выстрела
            // И добавьте звуки, анимацию, и так далее

            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>();
            Destroy(bullet, 2f);
            currentAmmo--;

        }
    }
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}