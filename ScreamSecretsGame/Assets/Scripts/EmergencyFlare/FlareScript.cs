using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareScript : MonoBehaviour
{
    private Light targetLight; // Reference to the Light component
    private float minIntensity = 0f; // Minimum light intensity
    private float maxIntensity = 1f; // Maximum light intensity
    private float changeInterval = 0.15f; // Time interval between intensity changes
    private FlareInstatiateScript flareScript;

    [SerializeField] private float fadeTime;
    [SerializeField] private float flareDuration;


    private float timer;

    void Start()
    {
        flareScript = GetComponentInParent<FlareInstatiateScript>();
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>(); // Automatically get the Light component if not assigned
        }
        StartCoroutine(FadeIn());
        StartCoroutine(FadeOut());
        StartCoroutine(DestroyObj());
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeInterval)
        {
            ChangeIntensity(minIntensity, maxIntensity);
            timer = 0f;
        }
    }

    void ChangeIntensity(float minIntensity, float maxIntensity)
    {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        targetLight.intensity = randomIntensity;
    }


    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(fadeTime);
        minIntensity = 1.45f;
        maxIntensity = 1.69f;
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(flareDuration);
        minIntensity = 0f;
        minIntensity = 1f;
        
    }
    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(flareDuration + fadeTime);
        Destroy(gameObject);
        flareScript.flareOn = false;
    }

}
