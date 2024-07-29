using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingScreenController : MonoBehaviour
{
    public Slider slider;
    public TMP_Text loadingText;
    public GameObject loadingScreenParent; // Parent GameObject holding all elements
    private float loadTime = 4f;
    private float readyTime = 2f;
    private float fadeDuration = 2f; // Duration for the fade-out
    private float blinkInterval = 0.5f; // Interval for blinking effect
    private Coroutine blinkCoroutine;

    private void Start()
    {
        gameObject.SetActive(true);
        StartCoroutine(LoadRoutine());
    }

    private IEnumerator LoadRoutine()
    {
        float elapsedTime = 0f;

        // Fill the slider over the specified load time
        while (elapsedTime < loadTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / loadTime);
            slider.value = progress;
            loadingText.text = $"{(int)(progress * 100)}%";
            yield return null;
        }

        // Display "Ready!!!" and start blinking for 2 seconds
        loadingText.text = "Ready!!!";
        blinkCoroutine = StartCoroutine(BlinkText());
        yield return new WaitForSeconds(readyTime);

        // Stop blinking and make sure text is visible
        StopCoroutine(blinkCoroutine);
        loadingText.enabled = true;

        // Fade out the loading screen
        elapsedTime = 0f;
        CanvasGroup canvasGroup = loadingScreenParent.AddComponent<CanvasGroup>();

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }

        // Ensure the alpha is completely 0
        canvasGroup.alpha = 0;

        // Disable the loading screen parent
        loadingScreenParent.SetActive(false);
    }

    private IEnumerator BlinkText()
    {
        while (true)
        {
            loadingText.enabled = !loadingText.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
