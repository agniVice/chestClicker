using System;
using UnityEngine;

public class UserBalance : MonoBehaviour
{
    public static UserBalance Instance { get; private set; }

    public Action OnChanged;
    public float Balance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        Balance = PlayerPrefs.GetFloat("Balance", 0);
    }
    public void Change(float count)
    {
        Balance += count;
        OnChanged?.Invoke();
        Save();
    }
    private void Save() => PlayerPrefs.SetFloat("Balance", Balance);
}
