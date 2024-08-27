using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPhoto : MonoBehaviour
{
    public Camera photoCamera; // Убедитесь, что это ваша нужная камера
    public RawImage rw; // RawImage, куда будет выводиться текстура
    public RenderTexture renderTexture; // RenderTexture для захвата изображения
    private Texture2D photoTexture;
    private GameObject Photo;

    // Start is called before the first frame update
    void Start()
    {
        photoCamera = GameObject.FindGameObjectWithTag("PhotoCamera").GetComponent<Camera>();
        // Устанавливаем RenderTexture на камеру
        photoCamera.targetTexture = renderTexture;
        StartCoroutine(ScreenS()); // Запускаем корутину с методом скриншота
        Photo = rw.gameObject; // Указываем, что Photo это RawImage
    }

    public void DeletePhoto()
    {
        Destroy(Photo); // Удаляем фото
    }

    IEnumerator ScreenS()
    {
        // Ждем завершения всех отрисовок
        yield return new WaitForEndOfFrame();

        // Создаем временную текстуру, чтобы захватить RenderTexture
        RenderTexture.active = renderTexture;
        Texture2D screen = new Texture2D(renderTexture.width, renderTexture.height);
        screen.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screen.Apply();
        RenderTexture.active = null;

        // Устанавливаем текстуру в RawImage
        rw.texture = screen;
    }
}
