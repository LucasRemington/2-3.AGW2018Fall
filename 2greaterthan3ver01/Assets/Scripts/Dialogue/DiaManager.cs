using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaManager : MonoBehaviour
{
    public Text text;
    public Image diaBox;
    public Animator animator;
    private Queue<string> sentences;
    [HideInInspector] public GameObject player;

    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (Input.GetButtonDown("XboxB") && player.GetComponent<BasicMovement>().paused)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Conversation, start!");
        animator.SetBool("IsOpen", true);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            text.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        Debug.Log("End conversation!");
        player.GetComponent<BasicMovement>().paused = false;
        animator.SetBool("IsOpen", false);
    }
}
