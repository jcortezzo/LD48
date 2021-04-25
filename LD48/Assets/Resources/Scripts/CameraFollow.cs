using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float lerp;
    [SerializeField] private Vector2 offsets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 targetPos = target.transform.position;
        Vector3 lerpPosition = Vector2.Lerp(this.transform.position, targetPos, lerp) + offsets;
        this.transform.position = new Vector3(lerpPosition.x, lerpPosition.y, this.transform.position.z);
    }
}
