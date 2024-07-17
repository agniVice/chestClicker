using DG.Tweening;
using UnityEngine;

public class MineSwepperUI : MonoBehaviour
{
    public static MineSwepperUI Instance { get; private set; }

    [SerializeField] private CanvasGroup _panel;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(this);
        else
            Instance = this;
    }
    public void OpenWindow()
    {
        if (!ItemManager.Instance.Unlocked)
            return;
        _panel.alpha = 0f;
        _panel.blocksRaycasts = true;
        _panel.DOFade(1f, 0.4f).SetLink(_panel.gameObject);
        ItemManager.Instance.RegenerateField();
    }
    public void CloseWindow(float delay = 0f)
    {
        ItemManager.Instance.Lock();
        _panel.blocksRaycasts = false;
        _panel.DOFade(0f, 0.4f).SetLink(_panel.gameObject).SetDelay(delay);
    }
}
