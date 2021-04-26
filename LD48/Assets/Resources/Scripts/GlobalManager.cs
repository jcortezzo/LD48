using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;
    public Direction gameDirection;

    public float MAX_DISTANCE;

    public ProgressBar LevelProgress { get; set; }
    public PooProgress PooProgress { get; set; }

    [SerializeField] public Player player { get; private set; }
    public Camera cam { get; private set; }

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
        player.SetDirection(gameDirection);

    }

    // Update is called once per frame
    void Update()
    {
        if(PooProgress != null)
        {
            if (PooProgress.PooPercentage() > 1.3f) LoadLosingScene();
        }

        if(LevelProgress != null)
        {
            if (LevelProgress.ProgressPercentage() >= 1.0f) LoadStoreScene();
        }
    }

    void LoadLosingScene()
    {
        SceneManager.LoadScene("LostScene");
    }

    void LoadStoreScene()
    {
        SceneManager.LoadScene("StoreScene");
    }
}
