using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradesUI : MonoBehaviour
{
    public static UpgradesUI Instance { get; private set; }

    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private List<Transform> _transforms;

    [SerializeField] private TextMeshProUGUI[] _prices;
    [SerializeField] private TextMeshProUGUI[] _upgradeTexts;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(Instance);
        Instance = this;
        _panel.blocksRaycasts = false;
    }

    public void OpenUpgrades()
    {
        Upgrades.Instance.CalculateUpgrades();
        UpdateUpgrades();
        _panel.blocksRaycasts = true;
        _panel.alpha = 0f;
        _panel.DOFade(1, 0.3f).SetLink(_panel.gameObject);

        float delay = 0.15f;
        foreach (Transform t in _transforms)
        {
            Vector2 startScale = t.localScale;
            t.localScale = Vector2.zero;
            t.DOScale(startScale, Random.Range(0.15f, 0.3f)).SetLink(transform.gameObject).SetEase(Ease.OutBack).SetDelay(delay);
            delay += 0.1f;
        }
    }
    public void CloseUpgrades()
    {
        _panel.blocksRaycasts = false;
        _panel.DOFade(0, 0.3f);
    }
    public void OnUpgradeButtonClicked(int id)
    {
        Upgrades.Instance.Upgrade(id);
        UpdateUpgrades();
    }
    public void UpdateUpgrades()
    {
        _prices[0].text = FormatNumHelper.FormatNum(Upgrades.Instance.NextChestPrice);
        _upgradeTexts[0].text = FormatNumHelper.FormatNum(Chest.Instance.ScorePerClick) + "\nPer click";

        _prices[1].text = FormatNumHelper.FormatNum(Upgrades.Instance.NextAutoClickPrice);
        _upgradeTexts[1].text = FormatNumHelper.FormatNum(Chest.Instance.AutoScore) + "\nPer second";

        if (Upgrades.Instance.CurrentUpgrades[2] == Upgrades.Instance.Prices.Length)
            _prices[2].text = "MAX";
        else
            _prices[2].text = FormatNumHelper.FormatNum((float)Upgrades.Instance.Prices[Upgrades.Instance.CurrentUpgrades[2]]);
        _upgradeTexts[2].text = "+" + (Upgrades.Instance.CurrentUpgrades[2] * 2).ToString() + "% Win\nSlot game";
    }
    public void OpenError()
    { 
        
    }
    public void CloseError()
    { 
        
    }
}
