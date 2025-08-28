using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string StatName;
    public int Level;
    public int MaxLevel = 5;
    public int BaseCost;

    public Upgrade(string statName, int baseCost)
    {
        StatName = statName;
        BaseCost = baseCost;

        if (PlayerPrefs.HasKey(StatName))
            Level = PlayerPrefs.GetInt(StatName);
        else
            Level = 1;
    }

    public int GetUpgradeCost()
    {
        return BaseCost * Level;
    }

    public bool CanUpgrade(int coins)
    {
        return Level < MaxLevel && coins >= GetUpgradeCost();
    }

    public void UpgradeLevel()
    {
        if (Level < MaxLevel)
        {
            Level++;
            PlayerPrefs.SetInt(StatName, Level); // kaydı güncelle
        }
    }

    public string GetDisplayText()
    {
        return $"{StatName}: {Level}/{MaxLevel}\nCost: {GetUpgradeCost()}";
    }
    
    public void ReloadLevelFromPrefs()
{
    Level = PlayerPrefs.GetInt($"UP_{StatName}_LEVEL", 0);
}

}