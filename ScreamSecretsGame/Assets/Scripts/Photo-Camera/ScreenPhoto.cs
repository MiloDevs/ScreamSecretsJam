using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPhoto : MonoBehaviour
{
    public Camera photoCamera; 
    public RawImage rw; 
    public RenderTexture renderTexture; 
    private Texture2D photoTexture;
    private GameObject Photo;

    // Start is called before the first frame update
    void Start()
    {
        photoCamera = GameObject.FindGameObjectWithTag("PhotoCamera").GetComponent<Camera>();
        photoCamera.targetTexture = renderTexture;
        StartCoroutine(ScreenS()); 
        Photo = rw.gameObject; 
    }

    public void DeletePhoto()
    {
        Destroy(Photo); 
    }

    IEnumerator ScreenS()
    {
        
        yield return new WaitForEndOfFrame();

        
        RenderTexture.active = renderTexture;
        Texture2D screen = new Texture2D(renderTexture.width, renderTexture.height);
        screen.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screen.Apply();
        RenderTexture.active = null;

        
        rw.texture = screen;
    }
}
