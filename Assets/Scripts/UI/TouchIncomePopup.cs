using System.Collections;
using TMPro;
using UnityEngine;

public class TouchIncomePopup : MonoBehaviour
{
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 0.75f;
    [SerializeField] private float moveDistance = 60f;

    private RectTransform rectTransform;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (amountText == null)
        {
            amountText = GetComponentInChildren<TMP_Text>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void Show(double amount)
    {
        if (amountText != null)
        {
            amountText.text = $"+{FormatMoney(amount)}원";
            amountText.raycastTarget = true;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = false;
        }

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(AnimateAndDestroy());
    }

    private IEnumerator AnimateAndDestroy()
    {
        Vector2 startPosition = rectTransform != null ? rectTransform.anchoredPosition : Vector2.zero;
        Vector2 endPosition = startPosition + Vector2.up * moveDistance;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = duration <= 0f ? 1f : Mathf.Clamp01(elapsed / duration);

            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, progress);
            }

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f - progress;
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    private static string FormatMoney(double value)
    {
        if (value < 1000d)
        {
            return value.ToString("0");
        }

        return value.ToString("N0");
    }
}
