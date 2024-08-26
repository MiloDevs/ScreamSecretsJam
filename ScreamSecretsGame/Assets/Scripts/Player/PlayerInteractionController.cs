using System;
using Interactions;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float sphereCastRadius = 0.2f;
    [SerializeField] private float uiShowDistance = 5f;

    [Header("Inspection Settings")]
    [SerializeField] private Vector3 inspectionPosition = new Vector3(0, -0.5f, 2f);
    [SerializeField] private float inspectionRotationSpeed = 5f;
    [SerializeField] private float inspectionZoomSpeed = 0.5f;
    [SerializeField] private float minZoomDistance = 1f;
    [SerializeField] private float maxZoomDistance = 4f;

    private Camera playerCamera;
    private IInteractible currentInteractable;
    private InspectableObject currentInspectedObject;
    private float currentInspectionDistance;
    private PlayerFPSController fpsController;

    private void Start()
    {
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        currentInspectionDistance = inspectionPosition.z;
        fpsController = GetComponent<PlayerFPSController>();
    }

    private void Update()
    {
        if (currentInspectedObject == null)
        {
            HandleInteraction();
        }
        else
        {
            HandleInspection();
        }
    }

    private void HandleInteraction()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereCastRadius, uiShowDistance, interactableLayer);

        IInteractible closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            IInteractible interactible = hit.collider.GetComponent<IInteractible>();
            if (interactible != null)
            {
                float distance = Vector3.Distance(transform.position, hit.point);
                if (distance < closestDistance)
                {
                    closestInteractable = interactible;
                    closestDistance = distance;
                }
            }
        }

        if (closestInteractable != null)
        {
            if (closestInteractable != currentInteractable)
            {
                if (currentInteractable != null)
                {
                    currentInteractable.HideUI();
                }
                currentInteractable = closestInteractable;
                currentInteractable.ShowUI();
            }

            if (Input.GetKeyDown(KeyCode.E) && closestDistance <= interactionDistance)
            {
                currentInteractable.Interact();
                if (currentInteractable is InspectableObject inspectableObject)
                {
                    StartInspection(inspectableObject);
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.HideUI();
                currentInteractable = null;
            }
        }
    }

    private void StartInspection(InspectableObject inspectableObject)
    {
        currentInspectedObject = inspectableObject;
        currentInspectedObject.StartInspection(playerCamera.transform, inspectionPosition);
        DisablePlayerMovement();
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void HandleInspection()
    {
        if (currentInspectedObject == null) return;

        // Rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        currentInspectedObject.Rotate(mouseY * inspectionRotationSpeed, mouseX * inspectionRotationSpeed);

        // Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentInspectionDistance = Mathf.Clamp(currentInspectionDistance - scroll * inspectionZoomSpeed, minZoomDistance, maxZoomDistance);
        Vector3 newPosition = inspectionPosition;
        newPosition.z = currentInspectionDistance;
        currentInspectedObject.UpdatePosition(newPosition);

        // Exit inspection
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))  // E or right-click to exit
        {
            StopInspection();
        }
    }

    private void StopInspection()
    {
        if (currentInspectedObject != null)
        {
            currentInspectedObject.StopInspection();
            currentInspectedObject = null;
            EnablePlayerMovement();
            Cursor.lockState = CursorLockMode.Locked;
            currentInspectionDistance = inspectionPosition.z;  // Reset zoom
        }
    }

    private void DisablePlayerMovement()
    {
        fpsController.DisablePlayerMovement();
    }

    private void EnablePlayerMovement()
    {
        fpsController.EnablePlayerMovement();
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 direction = playerCamera.transform.forward * interactionDistance;
            Gizmos.DrawRay(playerCamera.transform.position, direction);
            Gizmos.DrawWireSphere(playerCamera.transform.position + direction, sphereCastRadius);
        }
    }
}