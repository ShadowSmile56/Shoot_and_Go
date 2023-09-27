using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public int maxAmmo = 30; // ������������ ���������� ��������
    public int currentAmmo; // ������� ���������� ��������
    public float reloadTime = 2.0f; // ����� ����������� � ��������
    private bool isReloading = false; // ���� �����������
    private float lastReloadTime; // ����� ��������� �����������
    public Text ammoText;
    
    public Transform firePoint; // �������, ������ ����� ����������� �������
    public GameObject projectilePrefab; // ������ �������
    public float fireRate = 1f; // ������� �������� (�������� � �������)
    public float shootingRange = 10f; // ��������� ��������
    public LayerMask targetLayer; // ����, �� ������� ��������� ����������
    public float projectileSpeed = 10f; // �������� �������
    public bool flip;

    public float angle;
    private float timeToFire = 0f;
    private Transform player;
    private void Start()
    {
        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        currentAmmo = maxAmmo;
        // ������� � ��������� ��������� ������
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
                currentAmmo = maxAmmo; // ��������� ����������� � ��������������� �������
            }
        }
        else
        {
            if (currentAmmo<=0 )
            {
                // ��������� �����������
                isReloading = true;
                lastReloadTime = Time.time;
                // �������� ����� �������� �����������, ����� � �. �.
            }
        }
        ammoText.text = "Ammo: " + currentAmmo.ToString()+"/30";
        // ���������, ����� �� �������� (������ ���������� ������� � ����������� ��������)
        if (Time.time >= timeToFire)
        {
            // ����������� ���������� ����������
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(firePoint.position, shootingRange, targetLayer);
            Transform nearestTarget = GetNearestTarget(hitColliders);

            if (nearestTarget != null)
            {
                // ��������� ����������� � ���� � ����������� ������� ������
                // Vector2 targetDirection = ((Vector2)nearestTarget.position - (Vector2)firePoint.position).normalized;
                // Vector2 playerDirection = ((Vector2)player.position - (Vector2)firePoint.position).normalized;

                // ��������� ���� ����� �������������
                // float angleBetweenDirections = Vector2.Angle(targetDirection, playerDirection);

                // ���������, ��� ���� ������ ������������� �������� (��������, 30 ��������)
                if (currentAmmo > 0 && !isReloading)
                {
                    AimAtTarget(nearestTarget.position);
                    ShootAtTarget(nearestTarget.position);
                }
            }

            // ������������� ����� ���������� ��������
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
            // ���� ����� ������� �����, ���������� ���� �������� �� 180 ��������
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

            // ���������� ��� �������� �����
            // ��������, ���, ������� ������� ������ � �������� ��
            // ����������� ��������� `currentAmmo` ����� ��������
            // � �������� �����, ��������, � ��� �����

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