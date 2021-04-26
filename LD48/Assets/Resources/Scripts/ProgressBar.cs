using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressBarInner;
    [SerializeField] private Image playerHead;
    [SerializeField] private Transform progressEnd;

    [SerializeField] private Player player;
    [SerializeField] private float maxDistance;
    private float maxProgress;
    private float currentProgress;

    private float playerHeadStart;
    private float playerHeadMaxRange;

    // Start is called before the first frame update
    void Start()
    {
        GlobalManager.Instance.LevelProgress = this;

        currentProgress = player.transform.position.x;
        maxProgress = GlobalManager.Instance.gameDirection == Direction.RIGHT ? currentProgress + maxDistance : currentProgress - maxDistance;

        playerHeadStart = playerHead.transform.position.x;
        playerHeadMaxRange = progressEnd.transform.position.x - playerHead.transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgressBar();
    }

    void UpdateProgressBar()
    {
        progressBarInner.fillAmount = ProgressPercentage();
        float x = playerHeadStart + playerHeadMaxRange * ProgressPercentage();
        //GlobalManager.Instance.gameDirection == Direction.RIGHT ? playerHeadStart + playerHeadMaxRange * ProgressPercentage() : 
        //                                                                    playerHeadStart - playerHeadMaxRange * ProgressPercentage();
        playerHead.transform.position = new Vector3(x, 
                                                    playerHead.transform.position.y, 
                                                    playerHead.transform.position.z);
    }

    public float ProgressPercentage()
    {
        float percentage = player.transform.position.x / maxProgress;
        return percentage >= 1 ? 1: percentage;
    }
}
