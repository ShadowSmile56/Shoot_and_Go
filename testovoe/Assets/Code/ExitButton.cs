using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button exitButton; // Ссылка на кнопку выхода

    private void Start()
    {
        // Назначьте метод выхода при нажатии на кнопку
        exitButton.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        // Этот код позволяет выйти из приложения на различных платформах, таких как PC, Android, iOS и других
#if UNITY_EDITOR
        // В режиме редактора Unity просто завершите приложение
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // На устройствах примените соответствующий метод завершения приложения
            Application.Quit();
#endif
    }
}