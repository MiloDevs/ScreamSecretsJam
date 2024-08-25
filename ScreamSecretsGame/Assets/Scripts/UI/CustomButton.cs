using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CustomButton : Selectable
{
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    
    [SerializeField] private Image buttonImage;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float fillDuration = 0.5f;

    private Coroutine fillCoroutine;
    
    // on click events
    public UnityEvent onClick = new UnityEvent();

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetButton();
    }

    private void ResetButton()
    {
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }
        
        if (buttonImage != null)
        {
            buttonImage.fillAmount = 0;
        }
        
        transform.localScale = Vector3.one;
    }

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        ResetButton();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(FillImage(1f));
        if (hoverSound) audioSource.PlayOneShot(hoverSound);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(FillImage(0f));
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (clickSound) audioSource.PlayOneShot(clickSound);
        // execute all events within click list
        onClick.Invoke();
    }

    private IEnumerator FillImage(float targetFillAmount)
    {
        float elapsedTime = 0f;
        float startFillAmount = buttonImage.fillAmount;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fillDuration);
            buttonImage.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, t);
            yield return null;
        }

        buttonImage.fillAmount = targetFillAmount;
    }

    private IEnumerator ScaleButton(float targetScale)
    {
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;
        
        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fillDuration);
            transform.localScale = Vector3.Lerp(startScale, Vector3.one * targetScale, t);
            yield return null;
        }
        transform.localScale = Vector3.one * targetScale;
    }
}