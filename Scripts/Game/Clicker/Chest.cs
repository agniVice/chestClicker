using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public static Chest Instance {  get; private set; }

    public Action ChestClicked;
    public float ScorePerClick { get; private set; }
    public float AutoScore { get; private set; }

    public float InitialProductivityClick;
    public float InitialProductivityAutoClick;

    [SerializeField] private Transform _chest;

    [SerializeField] private GameObject _scorePrefab;
    [SerializeField] private Transform _scoreParent;

    private Vector2 _chestScale;

    private bool _autoClickEnable;
    private float _autoClickTimer = 1f;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(this);
        Instance = this;
        _chestScale = _chest.localScale;
    }
    private void Start()
    {
        UpdateProductivity();
    }
    private void FixedUpdate()
    {
        if (!_autoClickEnable)
            return;
        if (_autoClickTimer > 0)
            _autoClickTimer -= Time.fixedDeltaTime;
        else
            AutoClick();

    }
    public void OnChestClicked()
    {
        UserBalance.Instance.Change(ScorePerClick);
        SlotProgress.Instance.ChangeProgress(ScorePerClick);
        _chest.DOScale(_chestScale * 0.85f, 0.15f).SetLink(_chest.gameObject).SetEase(Ease.OutBack).OnKill(() => {
            _chest.DOScale(_chestScale, 0.15f).SetLink(_chest.gameObject).SetEase(Ease.OutBack);});
        ChestClicked?.Invoke();
        SpawnScore(ScorePerClick);
    }
    private void SpawnScore(float count)
    {
        Vector2 pos = new Vector2(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-150, 150));
        var score = Instantiate(_scorePrefab,_scoreParent);
        score.transform.localPosition = pos;
        score.GetComponent<TextMeshProUGUI>().text = "+" + FormatNumHelper.FormatNum(count);
        score.transform.DOLocalMoveY(pos.y + 500, 1f).SetLink(score.gameObject);
        score.GetComponent<CanvasGroup>().DOFade(0, 1f).SetLink(score.gameObject);
        Destroy(score, 1.5f);
    }
    private void AutoClick()
    {
        _autoClickTimer = 1f;
        UserBalance.Instance.Change(AutoScore);
        SlotProgress.Instance.ChangeProgress(AutoScore);
        ChestClicked?.Invoke();
        SpawnScore(AutoScore);
    }
    public void UpdateProductivity()
    {
        ScorePerClick = InitialProductivityClick * Upgrades.Instance.CurrentUpgrades[0];
        AutoScore = InitialProductivityAutoClick * Upgrades.Instance.CurrentUpgrades[1];
    }
    public void StopAutoClick() => _autoClickEnable = false;
    public void StartAutoClick() => _autoClickEnable = true;
}
