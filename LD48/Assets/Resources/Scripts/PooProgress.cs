using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PooProgress : MonoBehaviour
{
    [SerializeField] private RectTransform poo;
    [SerializeField] private RectTransform end;
    private Image pooImage;
    private float max;
    private float maxRange;

    void Start()
    {
        pooImage = poo.gameObject.GetComponent<Image>();
        maxRange = poo.position.y - end.position.y;
        max = poo.position.y;
        Debug.Log("maxx " + max);
    }

    // Update is called once per frame
    void Update()
    {
        poo.transform.position -= (0.5f * Vector3.up);
        //Debug.Log(PooPercentage());
    }

    public float PooPercentage()
    {
        //Debug.Log(poo.position + " " + end.position);
        return (max - poo.position.y) / maxRange; 
    }
}
