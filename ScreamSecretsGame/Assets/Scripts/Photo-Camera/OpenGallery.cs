using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGallery : MonoBehaviour
{
    public GameObject PanelPhoto;
    bool galleryOpened = false;
    public RectTransform ContentForms;
    public CursorLook cursor;

    void Start()
    {
        PanelPhoto.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P) && !galleryOpened)
        {
            PanelPhoto.SetActive(true);
            galleryOpened = true;
            cursor.ToggleCameraLock();
            FindPhoto();
        }
        else if (Input.GetKeyUp(KeyCode.P) && galleryOpened)
        {
            PanelPhoto.SetActive(false);
            galleryOpened = false;
            cursor.ToggleCameraLock();
        }
    }

    public void FindPhoto()
    {
        GameObject[] Photos = GameObject.FindGameObjectsWithTag("Photo");
        foreach (GameObject photo in Photos)
        {
            photo.transform.SetParent(ContentForms.transform, false);
            photo.transform.SetAsLastSibling();
        }
    }


}
