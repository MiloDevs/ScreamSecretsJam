using UnityEngine;
using System.Collections;

public class DoorInteractable : InteractibleObject
{
    [SerializeField] private float rotationSpeed = 90f; // Degrees per second
    [SerializeField] private float maxRotationAngle = 90f;
    [SerializeField] private Item typeOfKey;
    private bool isOpen = false;
    private bool isRotating = false;
    private bool isUnlocked = false;
    public override void Interact()
    {
        if (isOpen)
        {
            AudioManager.FindObjectOfType<AudioManager>().Play("Door_Close");
        }
        else
        { 
            AudioManager.FindObjectOfType<AudioManager>().Play("Door_Open");
        }

        if (GameObject.FindObjectOfType<InventorySystem>().playerHasThisItem(typeOfKey) || isUnlocked)
        {
            GameObject.FindObjectOfType<InventorySystem>().RemoveItem(typeOfKey);
            isUnlocked = true;
            if (!isRotating)
            {
                isOpen = !isOpen;
                StartCoroutine(RotateDoor());
            }
        }
    }

    private IEnumerator RotateDoor()
    {
        isRotating = true;
        float startRotation = transform.eulerAngles.y;
        float endRotation = isOpen ? startRotation + maxRotationAngle : startRotation - maxRotationAngle;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * (rotationSpeed / maxRotationAngle);
            float yRotation = Mathf.Lerp(startRotation, endRotation, t);
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }

        // Ensure the door ends at exactly the desired rotation
        transform.rotation = Quaternion.Euler(0f, endRotation, 0f);
        isRotating = false;
    }
}