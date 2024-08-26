using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

public abstract class InteractibleObject : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject uiElement;
    [SerializeField] private Vector3 uiOffset;
    
    private bool _isUIVisible;
    
    public abstract void Interact();
    public virtual void ShowUI()
    {
        if (uiElement != null && !_isUIVisible)
        {
            uiElement.SetActive(true);
            _isUIVisible = true;
        }
    }

    public virtual void HideUI()
    {
        if (uiElement != null && _isUIVisible)
        {
            uiElement.SetActive(false);
            _isUIVisible = false;
        }
    }

    protected virtual void UpdateUIPosition()
    {
        if (uiElement != null)
        {
            Vector3 worldPosition = transform.position + uiOffset;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            uiElement.transform.position = screenPosition;
        }
    }

    protected virtual void Update()
    {
        
    }
}
