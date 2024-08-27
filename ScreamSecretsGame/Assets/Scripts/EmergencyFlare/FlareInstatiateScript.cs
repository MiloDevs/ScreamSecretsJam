using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareInstatiateScript : MonoBehaviour
{
    public GameObject flare;
    public int flareQuantity;
    [HideInInspector] public bool flareOn = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !flareOn && flareQuantity > 0)
        {
            flareOn = true;
            flareQuantity -= 1;
            GameObject instance = Instantiate(flare, Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(transform);
            instance.transform.localPosition = Vector3.zero;
        }
    }
}
