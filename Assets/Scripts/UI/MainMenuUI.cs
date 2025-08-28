using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; 

public class MainMenuUI : MonoBehaviour
{
    public TMP_Text coinText;
    public Button playButton;

    public Button speedButton, damageButton, healthButton;
    public TMP_Text speedButtonText, damageButtonText, healthButtonText;

    private Upgrade speed, damage, health;
    private int coins;

    void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 200);

        speed = new Upgrade("Speed", 50);
        damage = new Upgrade("Damage", 75);
        health = new Upgrade("Health", 100);


        playButton.onClick.AddListener(OnPlayButtonPressed);
        speedButton.onClick.AddListener(() => UpgradeStat(speed));
        damageButton.onClick.AddListener(() => UpgradeStat(damage));
        healthButton.onClick.AddListener(() => UpgradeStat(health));

        UpdateUI();
    }

    void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("Level1");
    }

    void UpgradeStat(Upgrade upgrade)
    {
        if (upgrade.CanUpgrade(coins))
        {
            coins -= upgrade.GetUpgradeCost();
            upgrade.UpgradeLevel();

            PlayerPrefs.SetInt("Coins", coins);
            PlayerPrefs.SetInt(upgrade.StatName, upgrade.Level);

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        coinText.text = "Coins: " + coins;

        speedButtonText.text = speed.GetDisplayText();
        damageButtonText.text = damage.GetDisplayText();
        healthButtonText.text = health.GetDisplayText();
    }
    
    public void RefreshMenu()
    {
        coins = PlayerPrefs.GetInt("Coins", 200);

        speed.ReloadLevelFromPrefs();
        damage.ReloadLevelFromPrefs();
        health.ReloadLevelFromPrefs();

        UpdateUI();
    }

}