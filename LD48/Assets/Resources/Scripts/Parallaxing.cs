using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    private float length, startPosX, startPosY;
    public Camera cam;
    public float lerpT;
    public float parallaxSpeed;

    private void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = cam.transform.position.x * (1 - parallaxSpeed);
        float distX = (cam.transform.position.x * parallaxSpeed);
        float distY = (cam.transform.position.y * parallaxSpeed);
        //Vector3 newPos = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);
        Vector3 newPos = new Vector3(startPosX + distX, transform.position.y, transform.position.z);
        //Vector3 lerpPos = Vector3.Lerp(transform.position, newPos, lerpT);
        transform.position = newPos;

        if (temp > startPosX + length) startPosX += length;
        else if (temp < startPosX - length) startPosX -= length;
    }

}