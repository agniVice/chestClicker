using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerInterface : MonoBehaviour
{
    public static ClickerInterface Instance { get; private set; }

    [SerializeField] private CanvasGroup _clickerGroup;
    [SerializeField] private CanvasGroup _slotButtonGroup;

    [SerializeField] private TextMeshProUGUI _balanceText;
    [SerializeField] private TextMeshProUGUI _autoText;

    [SerializeField] private Image _slotBar;
    [SerializeField] private TextMeshProUGUI _diamondToSlotText;

    [SerializeField] private CanvasGroup _lockBonus;
    [SerializeField] private TextMeshProUGUI _timeToUnlockBonus;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        Chest.Instance.ChestClicked += OnObjectClicked;
    }
    private void FixedUpdate()
    {
        if(!ItemManager.Instance.Unlocked)
            _timeToUnlockBonus.text = Mathf.Round(ItemManager.Instance.TimeToUnlock).ToString();
    }
    public void OpenClicker()
    {
        PlayerPrefs.SetInt("SlotOpened", 0);
        _clickerGroup.blocksRaycasts = true;
        _clickerGroup.alpha = 0f;
        _clickerGroup.DOFade(1, 0.3f).SetLink(_clickerGroup.gameObject);
        UpdateBalance();
        UpdateAuto();
        Chest.Instance.StartAutoClick();
    }
    public void CloseClicker()
    {
        _clickerGroup.blocksRaycasts = false;
        _clickerGroup.DOFade(0, 0.3f).SetLink(_clickerGroup.gameObject);
        Chest.Instance.StopAutoClick();
    }
    public void UpdateBalance()
    {
        _balanceText.text = FormatNumHelper.FormatNum(UserBalance.Instance.Balance);
    }
    public void UpdateAuto()
    {
        _autoText.text = "+" + FormatNumHelper.FormatNum(Chest.Instance.AutoScore) + "/s";
    }
    public void LockBonus()
    {
        _lockBonus.DOFade(1, 0.2f).SetLink(_lockBonus.gameObject);
    }
    public void UnlockBonus()
    {
        _lockBonus.DOFade(0, 0.2f).SetLink(_lockBonus.gameObject);
    }
    public void OpenSlotButton()
    {
        _slotButtonGroup.blocksRaycasts = true;
        _slotButtonGroup.alpha = 0f;
        _slotButtonGroup.DOFade(1, 0.3f).SetLink(_slotButtonGroup.gameObject);
    }
    public void CloseSlotButton()
    {
        _slotButtonGroup.blocksRaycasts = false;
        _slotButtonGroup.DOFade(0, 0.3f).SetLink(_slotButtonGroup.gameObject);
    }
    public void OnObjectClicked()
    {
        UpdateBalance();
    }
    public void OnSlotButtonClicked()
    {
        Interface.Instance.OpenSlot();
        SlotProgress.Instance.CalculateNewProgress();
    }
    public void OnMenuButtonClicked() => Interface.Instance.OpenMenu();
    public void OnUpgradesButtonClicked() => UpgradesUI.Instance.OpenUpgrades();
    public void OnRatingButtonClicked() => RatingUI.Instance.OpenRating();
    public void OnMissonsButtonClicked() => MissionsUI.Instance.OpenMissions();
}
