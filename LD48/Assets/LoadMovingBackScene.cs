using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMovingBackScene : MonoBehaviour
{
    public float maxTime;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= maxTime)
        {
            GlobalManager.Instance.gameDirection = Direction.LEFT;
            GlobalManager.Instance.LoadMovingBackHomeScene();
        }
    }
}
