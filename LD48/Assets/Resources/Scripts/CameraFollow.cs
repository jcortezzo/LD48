using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float lerp;
    [SerializeField] private Vector2 offsets;
    [SerializeField] private Vector2 xFollowRange;
    [SerializeField] private Vector2 yFollowRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 targetPos = target.transform.position;
        Vector2 currentPos = this.transform.position;
        float x = 0;
        float y = 0;

        x = targetPos.x; //(targetPos.x - currentPos.x > xFollowRange.y) ? targetPos.x : currentPos.x;
        y = targetPos.y; // (targetPos.y - currentPos.y > yFollowRange.y) ? targetPos.y : currentPos.y;

        targetPos = new Vector2(x, y);
        Vector3 lerpPosition = Vector2.Lerp(this.transform.position, targetPos, lerp) + offsets;
        this.transform.position = new Vector3(lerpPosition.x, lerpPosition.y, this.transform.position.z);
    }
}
