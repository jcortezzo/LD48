using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private MovableEntity player;
    [SerializeField] private MovableEntity enemy;
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
            dialogueEntity2.DisplaySentence(sentence);
        } else
        {
            dialogueEntity1.DisplaySentence(sentence);
        }
        if(!ContainNextDialogue())
        {
            dialogueEntity2.gameObject.GetComponent<Collider2D>().enabled = false;
            dialogueEntity2.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            player.SetDirection(Direction.RIGHT);
            IsInDialogue = false;
        }
    }

    private bool ContainNextDialogue()
    {
        return sentences.Count > 0;
    }

    public void SetDialogueEntities(MovableEntity d1, MovableEntity d2)
    {
        player = d1;
        enemy = d2;
        dialogueEntity1 = d1.gameObject.GetComponent<DialogueEntity>();
        dialogueEntity2 = d2.gameObject.GetComponent<DialogueEntity>();
        d1.SetDirection(Direction.IDLE);
        d2.SetDirection(Direction.IDLE);
        StartDialogue();
    }

}

[System.Serializable]
public class Dialogue
{
    public string[] text;
}
