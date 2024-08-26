using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCameraControl : MonoBehaviour
{
    public RawImage rwPref; 
    public Transform sliderContent;

    //For flash effect
    public Light flashLight => gameObject.GetComponentInChildren<Light>();
    public float flashDuration = 0.1f;
    OpenGallery openGalleryScript => gameObject.GetComponentInParent<OpenGallery>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !openGalleryScript.galleryOpened)
        { 
            MakePhoto();
            TriggerFlash();
        }

    }

    public void MakePhoto()
    {
        Instantiate(rwPref);
    }

    void TriggerFlash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        flashLight.enabled = true;
        yield return new WaitForSeconds(flashDuration);
        flashLight.enabled = false;
    }

    
}
