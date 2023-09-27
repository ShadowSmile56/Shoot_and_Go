using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCleaner : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        print("���������� �������");
        // � ���� ������ �� ������ ������� ������� �� �����, ��������� ���������
        CleanupObjects();
    }

    private void CleanupObjects()
    {
        Destroy(gameObject);
    }
}