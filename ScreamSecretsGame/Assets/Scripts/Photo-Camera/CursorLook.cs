using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLook : MonoBehaviour
{
    public PlayerFPSController fpsController; // Ссылка на ваш контроллер FPS
    public bool isLocked = false;

    void Update()
    {
        if (isLocked)
        {
            // Разблокировать курсор и показать его
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (fpsController != null)
            {
                fpsController.enabled = false; // Отключаем управление камерой
            }
        }
        else
        {
            // Заблокировать курсор и скрыть его
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (fpsController != null)
            {
                fpsController.enabled = true; // Включаем управление камерой
            }
        }
    }

    public void ToggleCameraLock()
    {
        isLocked = !isLocked;
    }


}
