using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PooProgress : MonoBehaviour
{
    public static PooProgress Instance;
    [SerializeField] private RectTransform poo;
    [SerializeField] private RectTransform end;
    [SerializeField] private float pooSpeed;
    private Image pooImage;
    private float max;
    private float maxRange;


    void Start()
    {
        Instance = this;
        pooImage = poo.gameObject.GetComponent<Image>();
        maxRange = poo.position.y - end.position.y;
        max = poo.position.y;
        Debug.Log("maxx " + max);
    }

    // Update is called once per frame
    void Update()
    {
        poo.transform.position -= (pooSpeed * Vector3.up);
        if (PooPercentage() > 1.3f) SceneManager.LoadScene("LostScene");
        //Debug.Log(PooPercentage());
    }

    public float PooPercentage()
    {
        //Debug.Log(poo.position + " " + end.position);
        return (max - poo.position.y) / maxRange; 
    }
    
    public void PushPooDeeper(float value = 10)
    {
        poo.transform.position += Vector3.up * value;
    }
}
