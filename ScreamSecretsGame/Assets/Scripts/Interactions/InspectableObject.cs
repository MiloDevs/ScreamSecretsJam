﻿using UnityEngine;
using Interactions;

public class InspectableObject : MonoBehaviour, IInteractible
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;
    private bool isBeingInspected = false;
    private Transform cameraTransform;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalParent = transform.parent;
    }

    public void Interact()
    {
        // This method is called by the PlayerInteractionController
        // The actual inspection start/stop is handled by the controller
    }

    public void ShowUI()
    {
        // Implement UI show logic
    }

    public void HideUI()
    {
        // Implement UI hide logic
    }

    public void StartInspection(Transform cameraTransform, Vector3 inspectionPosition)
    {
        isBeingInspected = true;
        this.cameraTransform = cameraTransform;
        GetComponent<Collider>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        
        transform.SetParent(cameraTransform);
        transform.localPosition = inspectionPosition;
        transform.localRotation = Quaternion.identity;
    }

    public void StopInspection()
    {
        isBeingInspected = false;
        GetComponent<Collider>().enabled = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;
        
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    public void Rotate(float pitchDegrees, float yawDegrees)
    {
        if (isBeingInspected)
        {
            transform.Rotate(Vector3.right, pitchDegrees, Space.World);
            transform.Rotate(Vector3.up, yawDegrees, Space.World);
        }
    }

    public void UpdatePosition(Vector3 localPosition)
    {
        if (isBeingInspected)
        {
            transform.localPosition = localPosition;
        }
    }
}