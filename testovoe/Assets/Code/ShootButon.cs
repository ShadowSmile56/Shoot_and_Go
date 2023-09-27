using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootButon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Gun Gun;
    private bool isShooting = false;
    public float timeBetweenShots = 0.5f;
    private float lastShotTime; // ����� ���������� ��������

    // Start is called before the first frame update
    public void OnPointerDown(PointerEventData eventData)
    {
        // ������� �� ������
        isShooting = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // ���������� ������
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            if (Time.time - lastShotTime >= timeBetweenShots)
            {
                // ���� ������ ������������, �������� ����� �������� �� ����������� ��������
                Gun.Shoot();
                lastShotTime = Time.time; // ��������� ����� ���������� ��������
            }
        }
    }
}
