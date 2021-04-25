using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private Vector2 startPos;
    [SerializeField] private Camera cam;
    [SerializeField] private float parallexModifier;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = Camera.main;//GlobalManager.instance.cam;
    }

    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - parallexModifier);
        float dist = cam.transform.position.x * parallexModifier;

        transform.position = new Vector3(startPos.x + dist, transform.position.y, transform.position.z);

        if (temp > startPos.x + length)
        {
            startPos = new Vector2(startPos.x + length, startPos.y);
        }
        else if (temp < startPos.x - length)
        {
            startPos = new Vector2(startPos.x - length, startPos.y);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
