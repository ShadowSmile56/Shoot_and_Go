using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button exitButton; // ������ �� ������ ������

    private void Start()
    {
        // ��������� ����� ������ ��� ������� �� ������
        exitButton.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        // ���� ��� ��������� ����� �� ���������� �� ��������� ����������, ����� ��� PC, Android, iOS � ������
#if UNITY_EDITOR
        // � ������ ��������� Unity ������ ��������� ����������
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // �� ����������� ��������� ��������������� ����� ���������� ����������
            Application.Quit();
#endif
    }
}