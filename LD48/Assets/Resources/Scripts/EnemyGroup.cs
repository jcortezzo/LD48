using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;

    void Start()
    {
        foreach(Transform t in transform)
        {
            GameObject go = Instantiate(enemies[Random.Range(0, enemies.Length)], t.position, Quaternion.identity);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
