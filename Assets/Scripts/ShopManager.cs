using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public TMP_Text coinText;
    private int coins;

    void Start()
    {
        LoadCoins();
        UpdateCoinUI();
    }

    void LoadCoins()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
    }

    void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = $"Coins: {coins}";
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveCoins();
        UpdateCoinUI();
    }

    public void BuyBall(string ballKey, int price)
    {
        if (coins >= price)
        {
            coins -= price;
            SaveCoins();
            UpdateCoinUI();

            PlayerPrefs.SetInt(ballKey + "_Bought", 1);
            PlayerPrefs.Save();

            Debug.Log(ballKey + "_Bought");
        }
        else
        {
            Debug.Log("not enough");
        }
    }

    public void BuyBasketBall() { BuyBall("BasketBall", 0); }
    public void BuyFootball() { BuyBall("Football", 1000); }
    public void BuyBaseBall() { BuyBall("BaseBall", 1700); }
    public void BuyBowling() { BuyBall("Bowling", 2000); }
    public void BuyBabyBall() { BuyBall("BabyBall", 2700); }
    public void BuyVolleyball() { BuyBall("Volleyball", 3000); }

    public bool IsBallBought(string ballKey)
    {
        return PlayerPrefs.GetInt(ballKey + "_Bought", 0) == 1;
    }

    public void SelectBall(string ballKey)
    {
        if (IsBallBought(ballKey))
        {
            PlayerPrefs.SetString("SelectedBall", ballKey);
            PlayerPrefs.Save();
            Debug.Log(ballKey + " selected!");

            BallButton[] buttons = FindObjectsOfType<BallButton>();
            foreach (BallButton b in buttons)
            {
                b.UpdateButtonUI();
            }
        }
        else
        {
            Debug.Log("still not bought!");
        }
    }
}
