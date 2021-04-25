using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Direction direction = Direction.RIGHT;


    void Start()
    {
        player.SetDirection(direction);    
    }

    // Update is called once per frame
    void Update()
    {
        //player.SetDirection(direction);
    }
}
