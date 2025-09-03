using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallButton : MonoBehaviour
{
    public string ballKey;
    public int price;
    public TMP_Text buttonText;
    public Button button;
    public Image coinImage;

    private ShopManager shopManager;

    void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();
        UpdateButtonUI();

        if (button != null)
            button.onClick.AddListener(OnClick);
    }

    public void UpdateButtonUI()
    {
        if (shopManager == null) return;

        if (!shopManager.IsBallBought(ballKey))
        {
            if (buttonText != null) buttonText.text = price.ToString();
            if (button != null) button.interactable = true;
            if (coinImage != null) coinImage.enabled = true;
        }
        else
        {
            string selected = PlayerPrefs.GetString("SelectedBall", "");
            if (selected == ballKey)
            {
                if (buttonText != null) buttonText.text = "Selected";
                if (button != null) button.interactable = false;
            }
            else
            {
                if (buttonText != null) buttonText.text = "Select";
                if (button != null) button.interactable = true;
            }
            if (coinImage != null) coinImage.enabled = false;
        }
    }

    public void OnClick()
    {
        if (shopManager == null) return;

        if (!shopManager.IsBallBought(ballKey))
        {
            shopManager.BuyBall(ballKey, price);
        }
        else
        {
            shopManager.SelectBall(ballKey);
        }

        UpdateButtonUI();
    }
}
