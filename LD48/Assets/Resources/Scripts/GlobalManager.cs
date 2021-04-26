using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    [SerializeField] public Player player { get; private set; }
    public Camera cam { get; private set; }
    [SerializeField] private Direction direction = Direction.RIGHT;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        cam = Camera.main;
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        player.SetDirection(direction);

    }

    // Update is called once per frame
    void Update()
    {
        //player.SetDirection(direction);
    }
}
