using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool oneTimeTrigger = true;

    public void TriggerDialogue ()
    {
        DialogueManager manager = FindObjectOfType<DialogueManager>();
        if (manager)
        {
            foreach (DialogueTracker tracker in manager.trackers)
            {
                if (tracker.name == gameObject.tag)
                {
                    if (oneTimeTrigger && tracker.hasTriggered)
                    {
                        return;
                    }
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue, tracker.finalDialogue);
                    tracker.hasTriggered = true;
                }
            }
        }
    }
}
