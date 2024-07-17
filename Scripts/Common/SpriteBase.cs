using UnityEngine;

public class SpriteBase : MonoBehaviour
{
    public static SpriteBase Instance { get; private set; }

    public Sprite[] Sprites;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(this);
        Instance = this;
    }
}
