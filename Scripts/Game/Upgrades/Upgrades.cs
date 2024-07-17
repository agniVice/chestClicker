using System.Collections;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Instance {  get; private set; }

    //public int[] DefaultPrices;
    //public int[] PriceMultipliers;
    public int[] CurrentUpgrades;
    public int[] Prices;
    public int MaxSlotUpgrade;

    [Space]
    [Header("ChestClick")]
    public float BaseChestPrice;
    public float ChestCoefficient;
    public float NextChestPrice { get; private set; }

    [Space]
    [Header("ChestAutoClick")]
    public float BaseAutoclickPrice;
    public float AutoClickCoefficient;
    public float NextAutoClickPrice { get; private set; }

    private void Awake()
    {
        if(Instance != this && Instance != null)
            Destroy(this);
        Instance = this;
        Initialize();
        CalculateUpgrades();
    }
    private void Initialize()
    { 
        for(int i = 0; i < 3; i++) 
            CurrentUpgrades[i] = PlayerPrefs.GetInt(i + "Upgrade",1);
    }
    public void Upgrade(int id)
    {
        switch (id)
        {
            case 0:
                {
                    if (UserBalance.Instance.Balance >= NextChestPrice)
                    {
                        UserBalance.Instance.Change(-NextChestPrice);
                        ClickerInterface.Instance.UpdateBalance();
                        CurrentUpgrades[id]++;
                        Chest.Instance.UpdateProductivity();
                        Missions.Instance.TakeInfo("Click");
                        Save();
                        CalculateUpgrades();
                        Audio.Instance.PlaySound(Audio.Instance.Upgrade, 1f, 0.5f);
                    }
                    break;
                }
            case 1:
                {
                    if (UserBalance.Instance.Balance >= NextAutoClickPrice)
                    {
                        UserBalance.Instance.Change(-NextAutoClickPrice);
                        ClickerInterface.Instance.UpdateBalance();
                        CurrentUpgrades[id]++;
                        Chest.Instance.UpdateProductivity();
                        Missions.Instance.TakeInfo("Second");
                        Save();
                        CalculateUpgrades();
                        ClickerInterface.Instance.UpdateAuto();
                        Audio.Instance.PlaySound(Audio.Instance.Upgrade, 1f, 0.5f);
                    }
                    break;
                }
            case 2:
                {
                    if (CurrentUpgrades[2] >= MaxSlotUpgrade)
                        return;
                    if (UserBalance.Instance.Balance >= Prices[CurrentUpgrades[id] + 1])
                    {
                        UserBalance.Instance.Change(-Prices[CurrentUpgrades[id] + 1]);
                        CurrentUpgrades[id]++;
                        PlayerPrefs.SetInt("Upgrade3", CurrentUpgrades[id]);
                        Audio.Instance.PlaySound(Audio.Instance.Upgrade, 1f, 0.5f);
                        Save();
                    }
                    break;
                }
        }
    }
    private void Save()
    {
        for (int i = 0; i < 3; i++)
            PlayerPrefs.SetInt(i + "Upgrade", CurrentUpgrades[i]);
    }
    public void CalculateUpgrades()
    { 
        NextChestPrice = BaseChestPrice * Mathf.Pow(ChestCoefficient, CurrentUpgrades[0]);
        NextAutoClickPrice = BaseAutoclickPrice * Mathf.Pow(AutoClickCoefficient, CurrentUpgrades[1]);
    }
}
