using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;

    public float timeBetweenLetter;
    public float timeBetweenSentence;

    private bool isTalking;
    private bool isFinalDialogue;

    public Animator animator;

    public DialogueTracker[] trackers;

    private Queue<string> sentences;
    private string speakerName;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        isTalking = false;
    }

    public void StartDialogue (Dialogue dialogue, bool finalDialogue)
    {
        if (!isFinalDialogue)
        {
            isTalking = true;
            animator.SetBool("IsOpen", true);

            sentences.Clear();

            speakerName = dialogue.name;
            isFinalDialogue = finalDialogue;

            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        FindObjectOfType<AudioManager>().Play("Dialogue");

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {

        dialogueText.text = speakerName + ": ";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(timeBetweenLetter);
        }

        FindObjectOfType<AudioManager>().VolumeOff("Dialogue");
        yield return new WaitForSeconds(timeBetweenSentence);
        FindObjectOfType<AudioManager>().Stop("Dialogue");
        FindObjectOfType<AudioManager>().VolumeOn("Dialogue");

        DisplayNextSentence();
    }

    void EndDialogue()
    {
        isTalking = false;
        animator.SetBool("IsOpen", false);
    }
}
