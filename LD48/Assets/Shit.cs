using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        JukeBox.Instance.StopAllMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeShitSound()
    {
        JukeBox.Instance.PlaySFX("diaria");
    }

    public void RestartGame()
    {
        // TODO: restart code here
    }
}
