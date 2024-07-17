using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuInterface : MonoBehaviour
{
    public static MenuInterface Instance { get; private set; }
    [Header("Menu")]
    [SerializeField] private List<Transform> _menuTransforms;
    [SerializeField] private CanvasGroup _menuGroup;

    [Header("Settings")]
    [SerializeField] private List<Transform> _settingsTransforms;
    [SerializeField] private CanvasGroup _settingsGroup;

    [Space]
    [SerializeField] private Sprite _settingsEnabled;
    [SerializeField] private Sprite _settingsDisabled;

    [Space]
    [SerializeField] private Image _imageMusic;
    [SerializeField] private Image _imageSound;
    [SerializeField] private TextMeshProUGUI _textMusic;
    [SerializeField] private TextMeshProUGUI _textSound;

    public const string EnabledText = "ENABLED";
    public const string DisabledText = "DISABLED";

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        OpenMenu();
    }
    public void OnButtonPlayClicked() => Interface.Instance.OpenGame();
    public void OnButtonSettingsClicked() => OpenSettings(); 
    public void OnButtonExitClicked() => Application.Quit();
    public void OnButtonReturnClicked() => OpenMenu();
    public void OnButtonSoundClicked()
    {
        Audio.Instance.ToggleSound();
        UpdateSettings();
    }
    public void OnButtonMusicClicked()
    {
        Audio.Instance.ToggleMusic();
        UpdateSettings();
    }
    private void OpenMenu()
    {
        _settingsGroup.gameObject.SetActive(false);
        _menuGroup.gameObject.SetActive(true);
        _menuGroup.alpha = 0f;
        _menuGroup.DOFade(1, 0.3f).SetLink(_menuGroup.gameObject);

        foreach (var item in _menuTransforms)
        {
            item.localScale = Vector3.zero;
            item.DOScale(1, Random.Range(0.15f, 0.4f)).SetLink(item.gameObject).SetEase(Ease.OutBack);
        }
    }
    private void OpenSettings()
    {
        UpdateSettings();

        _menuGroup.gameObject.SetActive(false);
        _settingsGroup.gameObject.SetActive(true);
        _settingsGroup.alpha = 0f;
        _settingsGroup.DOFade(1, 0.3f).SetLink(_settingsGroup.gameObject);

        foreach (var item in _settingsTransforms)
        {
            item.localScale = Vector3.zero;
            item.DOScale(1, Random.Range(0.15f, 0.4f)).SetLink(item.gameObject).SetEase(Ease.OutBack);
        }
    }
    private void UpdateSettings()
    {
        _imageMusic.sprite = _settingsDisabled;
        _imageSound.sprite = _settingsDisabled;

        _textMusic.text = DisabledText;
        _textSound.text = DisabledText;

        if (Audio.Instance.Music)
        {
            _imageMusic.sprite = _settingsEnabled;
            _textMusic.text = EnabledText;
        }

        if (Audio.Instance.Sound)
        {
            _imageSound.sprite = _settingsEnabled;
            _textSound.text = EnabledText;
        }
    }
}
