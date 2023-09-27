using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootButon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Gun Gun;
    private bool isShooting = false;
    public float timeBetweenShots = 0.5f;
    private float lastShotTime; // Время последнего выстрела

    // Start is called before the first frame update
    public void OnPointerDown(PointerEventData eventData)
    {
        // Нажатие на кнопку
        isShooting = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // Отпускание кнопки
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            if (Time.time - lastShotTime >= timeBetweenShots)
            {
                // Если кнопка удерживается, вызываем метод стрельбы из контроллера стрельбы
                Gun.Shoot();
                lastShotTime = Time.time; // Обновляем время последнего выстрела
            }
        }
    }
}
