using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;
    public Direction gameDirection;

    public float MAX_DISTANCE;
    public bool levelBegin;

    public ProgressBar LevelProgress { get; set; }
    public PooProgress PooProgress { get; set; }

    [SerializeField] public Player player { get; set; }
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
            if (PooProgress.PooPercentage() > 1.3f)
            {
                if(player != null) player.ShitPants();
                //LoadLosingScene();
            }
        }

        if(LevelProgress != null && gameDirection == Direction.RIGHT)
        {
            if (LevelProgress.ProgressPercentage() >= 1.0f) LoadStoreScene();
        }

        if (LevelProgress != null && gameDirection == Direction.LEFT)
        {
            if (LevelProgress.ProgressPercentage() >= 1.0f) LoadWinScene();
        }
    }

    public void LoadLosingScene()
    {
        SceneManager.LoadScene("LostScene");
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }

    public void LoadStoreScene()
    {
        SceneManager.LoadScene("StoreScene");
    }

    public void LoadMovingBackHomeScene()
    {
        SceneManager.LoadScene("NguyenPrettyScene_Backward");
    }
}
