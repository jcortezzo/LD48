using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueEntity : MonoBehaviour
{
    private TextMeshPro text;

    void Start()
    {
        text = transform.GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplaySentence(string sentence)
    {
        text.text = sentence;
    }

    public void ClearText()
    {
        text.text = "";
    }
}
