using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rating : MonoBehaviour
{
    public static Rating Instance { get; private set; }
    public List<string> Nicknames { get; private set; } = new List<string>();
    public List<float> Scores { get; private set; } = new List<float>();
    public int UserIndex { get; private set; }

    [SerializeField] private List<string> _defaultNicknames = new List<string>();
    [SerializeField] private List<float> _defaultScores = new List<float>();

    [SerializeField] private List<RatingItem> _items = new List<RatingItem>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        InitializeAll();
        StartCoroutine(AutoUpdate());
    }
    public void InitializeAll()
    {
        UpdateScores();
        for (int i = 0; i < _items.Count; i++)
            _items[i].Initialize(i, Scores[i], Nicknames[i], (UserIndex == i));
       
    }
    private void ClearScores()
    {
        Scores.Clear();
        foreach (float score in _defaultScores)
            Scores.Add(score);

        Nicknames.Clear();
        foreach (string nickname in _defaultNicknames)
            Nicknames.Add(nickname);
    }
    private void UpdateScores()
    {
        ClearScores();
        InsertInSortedList();
    }
    private IEnumerator AutoUpdate()
    {
        while (true)
        { 
            yield return new WaitForSeconds(5f);
            InsertInSortedList();        
        }
    }
    private void InsertInSortedList()
    {
        int index = Scores.BinarySearch(UserBalance.Instance.Balance, Comparer<float>.Create((x, y) => y.CompareTo(x)));
        if (index < 0)
            index = ~index;
        Nicknames.Insert(index, "You");
        Scores.Insert(index, UserBalance.Instance.Balance);

        UserIndex = index;

        Missions.Instance.TakeInfo("Rating");
    }
}
