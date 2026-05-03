using UnityEngine;
using UnityEngine.UI;

public class BottomMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private RecipeUpgradePanel recipeUpgradePanel;

    [Header("Buttons")]
    [SerializeField] private Button recipeButton;
    [SerializeField] private Button catButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button settingsButton;

    private void Awake()
    {
        if (recipeButton != null)
        {
            recipeButton.onClick.AddListener(OpenRecipeUpgradePanel);
        }

        AddListener(catButton, "알바냥");
        AddListener(menuButton, "메뉴");
        AddListener(shopButton, "가게");
        AddListener(settingsButton, "설정");
    }

    private void OpenRecipeUpgradePanel()
    {
        if (recipeUpgradePanel == null)
        {
            Debug.LogWarning("[BottomMenuController] RecipeUpgradePanel reference is missing.");
            return;
        }

        recipeUpgradePanel.Open();
    }

    private static void AddListener(Button button, string label)
    {
        if (button == null)
        {
            return;
        }

        button.onClick.AddListener(() =>
        {
            Debug.Log($"[BottomMenuController] {label} button clicked. This menu will be implemented later.");
        });
    }
}
