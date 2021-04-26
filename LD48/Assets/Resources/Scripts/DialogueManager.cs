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
    [SerializeField] private Dialogue[] birdDialogues;
    public bool IsInDialogue { get; private set; }

    private Queue<string> sentences;
    private bool firstPerson;

    void Start()
    {
        Instance = this;
        sentences = new Queue<string>();
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
        Dialogue dialogue = (enemy.entityType != EntityType.BIRD) ? dialogues[Random.Range(0, dialogues.Length)] : 
                                                                    birdDialogues[Random.Range(0, birdDialogues.Length)];
        firstPerson = dialogue.p1First;
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
        if (!ContainNextDialogue())
        {
            if (player != null)
            {
                dialogueEntity2.gameObject.GetComponent<Collider2D>().enabled = false;
                dialogueEntity2.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                dialogueEntity1.DisplaySentence("");
                player.SetDirection(GlobalManager.Instance.gameDirection);
                IsInDialogue = false;
                player.isTalking = false;
                enemy.isTalking = false;
                ClearEntities();
            } 
            return;
        }
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
        firstPerson = !firstPerson;

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
        d1.isTalking = true;
        d2.isTalking = true;
        d1.SetDirection(Direction.IDLE);
        d2.SetDirection(Direction.IDLE);
        StartDialogue();
    }

    private void ClearEntities()
    {
        dialogueEntity1 = null;
        dialogueEntity2 = null;
        player = null;
        enemy = null;
    }
}

[System.Serializable]
public class Dialogue
{
    public string[] text;
    public bool p1First;
}
