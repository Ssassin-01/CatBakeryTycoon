using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BakeryTouchInput : MonoBehaviour, IPointerDownHandler
{
    [Header("References")]
    [SerializeField] private EconomyManager economyManager;
    [SerializeField] private RectTransform popupParent;
    [SerializeField] private TouchIncomePopup touchIncomePopupPrefab;

    [Header("Ignored UI Roots")]
    [SerializeField] private RectTransform[] ignoredUiRoots;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null || IsInsideIgnoredUi(eventData))
        {
            return;
        }

        if (economyManager == null)
        {
            economyManager = FindFirstObjectByType<EconomyManager>();
        }

        if (economyManager == null)
        {
            Debug.LogWarning("[BakeryTouchInput] EconomyManager reference is missing.");
            return;
        }

        double earnedAmount = economyManager.AddTapIncome();
        SpawnPopup(eventData, earnedAmount);
    }

    private bool IsInsideIgnoredUi(PointerEventData eventData)
    {
        Camera uiCamera = eventData.pressEventCamera;
        if (ignoredUiRoots != null)
        {
            for (int i = 0; i < ignoredUiRoots.Length; i++)
            {
                RectTransform root = ignoredUiRoots[i];
                if (root == null)
                {
                    continue;
                }

                if (RectTransformUtility.RectangleContainsScreenPoint(root, eventData.position, uiCamera))
                {
                    return true;
                }
            }
        }

        return IsBlockedByOtherRaycastTarget(eventData);
    }

    private bool IsBlockedByOtherRaycastTarget(PointerEventData eventData)
    {
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            return false;
        }

        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(eventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            GameObject hitObject = results[i].gameObject;
            if (hitObject == null)
            {
                continue;
            }

            if (hitObject == gameObject || hitObject.transform.IsChildOf(transform))
            {
                return false;
            }

            TouchIncomePopup popup = hitObject.GetComponentInParent<TouchIncomePopup>();
            if (popup != null)
            {
                return true;
            }
        }

        return false;
    }

    private void SpawnPopup(PointerEventData eventData, double earnedAmount)
    {
        if (touchIncomePopupPrefab == null || popupParent == null)
        {
            return;
        }

        TouchIncomePopup popup = Instantiate(touchIncomePopupPrefab, popupParent);
        RectTransform popupRect = popup.GetComponent<RectTransform>();

        if (popupRect != null)
        {
            Camera uiCamera = eventData.pressEventCamera;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    popupParent,
                    eventData.position,
                    uiCamera,
                    out Vector2 localPosition))
            {
                popupRect.anchoredPosition = localPosition;
            }
        }

        popup.Show(earnedAmount);
    }
}
