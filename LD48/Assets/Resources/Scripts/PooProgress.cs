using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PooProgress : MonoBehaviour
{
    public static PooProgress Instance;
    [SerializeField] private RectTransform start;
    [SerializeField] private RectTransform poo;
    [SerializeField] private RectTransform end;
    [SerializeField] private float pooSpeed;

    [Range(0.0f, 1f)]
    public float startingProgress;

    private Image pooImage, colonImage;

    private float max;
    private float maxRange;

    private bool beginProgress;

    [SerializeField] private float POO_TIME;
    private float pooTimer;

    //private float startY;

    void Start()
    {
        Instance = this;
        GlobalManager.Instance.PooProgress = this;
        beginProgress = true;
        colonImage = this.GetComponent<Image>();
        pooImage = poo.gameObject.GetComponent<Image>();

        // based on time
        pooTimer = POO_TIME;

        // based on distance
        //maxRange = poo.position.y - end.position.y;
        //max = poo.position.y;

        PushPooDeeper(-maxRange * startingProgress);
        Debug.Log("maxx " + max);
    }

    public void DisablePoo()
    {
        beginProgress = false;
        pooImage.enabled = false;
        colonImage.enabled = false;
    }
    public void BeginPoo()
    {
        beginProgress = true;
        pooImage.enabled = true;
        colonImage.enabled = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (!beginProgress) return;

        // distance
        //poo.transform.position -= (pooSpeed * Vector3.up * Time.deltaTime);

        //pooTimer = Mathf.Max(pooTimer - Time.deltaTime, 0);
        pooTimer -= Time.deltaTime;
        float dist = Math.Abs(end.position.y - start.position.y);
        poo.position = new Vector3(poo.position.x, start.position.y - (PooPercentageComplete() * dist));
        
        //Debug.Log(PooPercentage());
    }

    /// <summary>
    /// returns value [0,1] for how far the poop is from coming out
    /// </summary>
    /// <returns></returns>
    public float PooPercentageComplete()
    {
        //Debug.Log(poo.position + " " + end.position);

        // distance
        //return (max - poo.position.y) / maxRange;

        // time
        //float modified = pooTimer < 0 ? POO_TIME + -pooTimer : pooTimer > 0 ? pooTimer : 0.0001f;  // allow >100%
        return 1 - pooTimer / POO_TIME;
    }
    
    public void PushPooDeeper(float value = 0.15f)
    {
        // distance
        //if(PooPercentage() >= 0)
        //{
        //    poo.transform.position += Vector3.up * value;
        //}

        // time
        pooTimer = Mathf.Min(pooTimer + POO_TIME * value, POO_TIME);
    }
}
