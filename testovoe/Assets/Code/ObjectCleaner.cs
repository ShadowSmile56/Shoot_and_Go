using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCleaner : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        print("Приложение закрыто");
        // В этом методе вы можете удалить объекты на земле, созданные монстрами
        CleanupObjects();
    }

    private void CleanupObjects()
    {
        Destroy(gameObject);
    }
}