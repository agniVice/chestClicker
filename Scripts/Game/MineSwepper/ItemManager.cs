using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    public const int ItemCount = 16;
    public const int Bombs = 4;
    public const int Flags = 4;
    public const int Emptys = 8;
    public float Multiplier = 0.05f;

    public float TimeToUnlock;
    public bool Unlocked;

    private bool _isItemsSpawned;

    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _prefabItem;
    [SerializeField] private List<Item> _items = new List<Item>();
    [SerializeField] private List<Item> _freeItem = new List<Item>();

    [SerializeField] private float _timeToUnlock;

    private void Awake()
    {
        if(Instance != this && Instance != null)
            Destroy(this);
        else
            Instance = this;
    }
    private void Start()
    {
        Lock();
    }
    private void FixedUpdate()
    {
        if (Unlocked)
            return;
        if (TimeToUnlock > 0)
            TimeToUnlock -= Time.fixedDeltaTime;
        else
            UnLock();
    }
    public void RegenerateField()
    {
        DestroyField();
        SpawnItems();
    }
    public void OnMiniGameOver()
    {
        Invoke("OpenAllBeforeDestroy", 0.3f);
    }
    private void OpenAllBeforeDestroy()
    {
        foreach (Item item in _items)
            item.FinishOpen();
        foreach (Item item in _freeItem)
            item.FinishOpen();
    }
    private void DestroyField()
    {
        foreach (Item item in _items)
            Destroy(item.gameObject);
        foreach (Item item in _freeItem)
            Destroy(item.gameObject);

        _freeItem.Clear();
        _items.Clear();
    }
    private void SpawnItems()
    {
        for (int i = 0; i < ItemCount; i++)
        {
            Item item = Instantiate(_prefabItem, _parent).GetComponent<Item>();
            item.Type = Item.TypeItem.Empty;
            _items.Add(item);
            _freeItem.Add(item);
        }
        for (int i = 0; i < Bombs; i++)
        {
            Item item = _freeItem[Mathf.RoundToInt(UnityEngine.Random.Range(0, _freeItem.Count - 1))];
            item.Type = Item.TypeItem.Bomb;
            _freeItem.Remove(item);
        }
        for (int i = 0; i < Flags; i++)
        {
            Item item = _freeItem[Mathf.RoundToInt(UnityEngine.Random.Range(0, _freeItem.Count - 1))];
            item.Type = Item.TypeItem.Flag;
            _freeItem.Remove(item);
        }
        _freeItem.Clear();
    }
    public void ItemOpened(Item item)
    {
        switch (item.Type)
        {
            case Item.TypeItem.Bomb:
                Audio.Instance.PlaySound(Audio.Instance.Incorrect, 1, 0.6f);
                Invoke("OpenAllBeforeDestroy", 0.3f);
                MineSwepperUI.Instance.CloseWindow(1f);
                break;
            case Item.TypeItem.Flag:
                Audio.Instance.PlaySound(Audio.Instance.Correct, 1, 0.6f);
                UserBalance.Instance.Change(UserBalance.Instance.Balance * Multiplier);
                break;
        }


        _freeItem.Add(item);
        _items.Remove(item);

        if (_items.Count == 0)
        {
            Invoke("OpenAllBeforeDestroy", 0.3f);
            MineSwepperUI.Instance.CloseWindow(1f);
        }
    }
    public void Lock()
    {
        ClickerInterface.Instance.LockBonus();
        TimeToUnlock = _timeToUnlock;
        Unlocked = false;
    }
    public void UnLock()
    {
        ClickerInterface.Instance.UnlockBonus();
        Unlocked = true;
    }
}
