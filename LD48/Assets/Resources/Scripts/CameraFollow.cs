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

    private float yPos;
    // Start is called before the first frame update
    void Start()
    {
        yPos = target.transform.position.y + offsets.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 targetPos = target.transform.position;
        //Vector2 currentPos = this.transform.position;
        float x = 0;
        float y = 0;

        x = targetPos.x; //(targetPos.x - currentPos.x > xFollowRange.y) ? targetPos.x : currentPos.x;
        y = targetPos.y; // (targetPos.y - currentPos.y > yFollowRange.y) ? targetPos.y : currentPos.y;

        targetPos = new Vector2(x, y);// + offsets;
        //Vector3 lerpPosition = Vector2.Lerp(this.transform.position, targetPos, lerp) + offsets;
        //this.transform.position = new Vector3(lerpPosition.x, lerpPosition.y, this.transform.position.z);
        this.transform.position = new Vector3(targetPos.x + offsets.x, yPos, this.transform.position.z);
    }
}
