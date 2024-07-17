using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotProgress : MonoBehaviour
{
    public static SlotProgress Instance { get; private set; }

    public float CurrentProgress { get; private set; }
    public float ScoreToOpen { get; private set; }

    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _scoreToOpenText;

    private void Awake()
    {
        if(Instance != this && Instance != null)
            Destroy(this);
        Instance = this;

        CurrentProgress = PlayerPrefs.GetFloat("CurrentProgress", 0);
        ScoreToOpen = PlayerPrefs.GetFloat("ScoreToOpen", 200);
    }
    private void Start()
    {
        UpdateProgressBar();
    }
    public void ChangeProgress(float count)
    {
        if (CurrentProgress <= (ScoreToOpen + count))
        {
            CurrentProgress += count;
            UpdateProgressBar();
            Save();
        }
    }

    public void CalculateNewProgress()
    {
        CurrentProgress = 0;
        ScoreToOpen = (Chest.Instance.ScorePerClick * 400) + (Chest.Instance.AutoScore * 400);
        Save();
    }
    public void UpdateProgressBar()
    {
        _scoreToOpenText.text = FormatNumHelper.FormatNum(ScoreToOpen);
        if (CurrentProgress != 0)
            _progressBar.fillAmount = CurrentProgress / ScoreToOpen;
        else 
            _progressBar.fillAmount = 0;

        if (CurrentProgress >= ScoreToOpen)
            ClickerInterface.Instance.OpenSlotButton();
        else
            ClickerInterface.Instance.CloseSlotButton();
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("CurrentProgress", CurrentProgress);
        PlayerPrefs.SetFloat("ScoreToOpen", ScoreToOpen);
    }
}
