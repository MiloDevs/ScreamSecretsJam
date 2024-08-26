using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLook : MonoBehaviour
{
    public PlayerFPSController fpsController; // ������ �� ��� ���������� FPS
    public bool isLocked = false;

    void Update()
    {
        if (isLocked)
        {
            // �������������� ������ � �������� ���
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (fpsController != null)
            {
                fpsController.enabled = false; // ��������� ���������� �������
            }
        }
        else
        {
            // ������������� ������ � ������ ���
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (fpsController != null)
            {
                fpsController.enabled = true; // �������� ���������� �������
            }
        }
    }

    public void ToggleCameraLock()
    {
        isLocked = !isLocked;
    }


}
