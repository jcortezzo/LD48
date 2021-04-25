using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private Player player;
    [SerializeField] private DialogueEntity dialogueEntity1;
    [SerializeField] private DialogueEntity dialogueEntity2;
    [SerializeField] private Dialogue[] dialogues;
    public bool IsInDialogue { get; private set; }

    private Queue<string> sentences;
    private bool firstPerson;

    void Start()
    {
        Instance = this;
        sentences = new Queue<string>();
        firstPerson = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue()
    {
        Dialogue dialogue = dialogues[Random.Range(0, dialogues.Length)];
        sentences.Clear();
        foreach(string s in dialogue.text)
        {
            sentences.Enqueue(s);
        }
        DisplayNextSentence();
        IsInDialogue = true;
    }

    private void DisplayNextSentence()
    {
        if (!ContainNextDialogue()) return;
        string sentence = sentences.Dequeue();
        dialogueEntity1.ClearText();
        dialogueEntity2.ClearText();
        if (firstPerson)
        {
            dialogueEntity1.DisplaySentence(sentence);
        } else
        {
            dialogueEntity2.DisplaySentence(sentence);
        }
        if(!ContainNextDialogue())
        {
            dialogueEntity1.gameObject.GetComponent<Collider2D>().enabled = false;
            player.SetDirection(Direction.RIGHT);
            IsInDialogue = false;
        }
    }

    private bool ContainNextDialogue()
    {
        return sentences.Count > 0;
    }

    public void SetDialogueEntities(Player d1, DialogueEntity d2)
    {
        player = d1;
        dialogueEntity1 = d2;
        dialogueEntity2 = d1.gameObject.GetComponent<DialogueEntity>();
        d1.SetDirection(Direction.IDLE);
        StartDialogue();
    }

}

[System.Serializable]
public class Dialogue
{
    public string[] text;
}
