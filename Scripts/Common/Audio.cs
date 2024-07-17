using System;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    public static Audio Instance;

    public bool Sound { get; private set; }
    public bool Music { get; private set; }

    [SerializeField] private AudioMixer _mixer;

    [Header("Slot")]
    public AudioClip DefaultWin;
    public AudioClip BigWin;
    public AudioClip Scroll;
    public AudioClip LastScroll;
    public AudioClip SpinSound;
    public AudioClip Bonus;
    public AudioClip Upgrade;
    public AudioClip Correct;
    public AudioClip Incorrect;

    [Header("Shop")]
    public AudioClip UpgradeSound;

    [SerializeField] private GameObject _prefabSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        Sound = Convert.ToBoolean(PlayerPrefs.GetInt("SoundEnabled", 1));
        Music = Convert.ToBoolean(PlayerPrefs.GetInt("MusicEnabled", 0));

        UpdateSoundAndMusic();
    }
    private void UpdateSoundAndMusic()
    {
        if (Sound)
            _mixer.SetFloat("Sound", 0f);
        else
            _mixer.SetFloat("Sound", -80f);
        if (Music)
            _mixer.SetFloat("Music", 0f);
        else
            _mixer.SetFloat("Music", -80f);
    }
    public void PlaySound(AudioClip clip, float pitch, float volume = 1f)
    {
        if (Sound)
            Instantiate(_prefabSound).GetComponent<Sound>().PlaySound(clip, pitch, volume);
    }
    public void Save()
    {
        PlayerPrefs.SetInt("SoundEnabled", Convert.ToInt32(Sound));
        PlayerPrefs.SetInt("MusicEnabled", Convert.ToInt32(Music));
    }
    public void ToggleSound()
    {
        Sound = !Sound;
        UpdateSoundAndMusic();
        Save();
    }
    public void ToggleMusic()
    {
        Music = !Music;
        UpdateSoundAndMusic();
        Save();
    }
}
