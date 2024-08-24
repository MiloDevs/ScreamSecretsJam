using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCameraControl : MonoBehaviour
{
    public RawImage rwPref; 
    public Transform sliderContent; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            MakePhoto(); 
            //Sound & effects
        }

    }

    public void MakePhoto()
    {
        Instantiate(rwPref);
    }
}
