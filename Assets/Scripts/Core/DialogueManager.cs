using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public event Action OnDialogueComplete;

    [Header("UI Elements")]
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI leftText;
    [SerializeField] private Image leftPortrait;
    [SerializeField] private TextMeshProUGUI rightText;
    [SerializeField] private Image rightPortrait;
    [SerializeField] private TextMeshProUGUI middleText;

    [Header("Settings")]
    [SerializeField] private Color activeSpeakerColor; // white, full alpha
    [SerializeField] private Color inactiveSpeakerColor; // disabled gray, alpha
    // TODO: Speed of text

    private Queue<DialogueEntry> queuedLines;
    private InputManager _inputManager; // TODO: Is this needed or can we just use AppManager?

    public void Setup(InputManager inputManager)
    {
        _inputManager = inputManager;
        root.SetActive(false); // Hide dialogue UI by default
    }

    /// <summary>
    /// Starts a dialogue sequence by queuing its lines and displaying the first one
    /// If a sequence is already playing, appends the new lines to the queue
    /// Start listening to input events to advance dialogue
    /// </summary>
    /// <param name="sequence"></param>
    public void PlayDialogue(DialogueSequence sequence)
    {
        // Setup input settings for this dialogue sequence
        // Allows control over which keys or mouse buttons advance dialogue
        sequence.ApplyInputSettings(_inputManager);

        // Already playing dialogue, append new lines to queue
        if (queuedLines != null && queuedLines.Count > 0)
        {
            foreach (var line in sequence.lines)
            {
                queuedLines.Enqueue(new DialogueEntry(line, sequence));
                Debug.Log("Appended dialogue line to queue: " + line.text);
            }
            return;
        }

        queuedLines = new Queue<DialogueEntry>();
        foreach (var line in sequence.lines)
        {
            queuedLines.Enqueue(new DialogueEntry(line, sequence));
            Debug.Log("Queued dialogue line: " + line.text);
        }
        ShowNextLine(); // Prep first line before enabling UI

        // Setup input to listen for advance dialogue events
        _inputManager.OnAdvanceDialogue += ShowNextLine;
    }

    /// <summary>
    /// Displays the next line in the queue, or ends the dialogue if none remain
    /// </summary>
    private void ShowNextLine()
    {
        if (queuedLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueEntry entry = queuedLines.Dequeue();
        DialogueLine line = entry.line;

        // TODO: Animate text display over time instead of instant
        switch (line.alignment)
        {
            case DialogueLine.Alignment.Left:
                UpdateUI(leftText, leftPortrait, line.text, entry.sequence.defaultLeftPortrait, line.alignment);
                break;
            case DialogueLine.Alignment.Right:
                UpdateUI(rightText, rightPortrait, line.text, entry.sequence.defaultRightPortrait, line.alignment);
                break;
            case DialogueLine.Alignment.Center:
                UpdateUI(middleText, null, line.text, null, line.alignment);
                break;
        }
    }

    /// <summary>
    /// Ends the current dialogue sequence and hides the UI
    /// Triggers event to notify listeners that dialogue has completed
    /// Stop listening to input events
    /// </summary>
    private void EndDialogue()
    {
        _inputManager.OnAdvanceDialogue -= ShowNextLine;
        root.SetActive(false);
        OnDialogueComplete?.Invoke();
    }

    private void UpdateUI(TextMeshProUGUI textElement, Image portraitElement, string text, Sprite defaultPortrait, DialogueLine.Alignment alignment)
    {
        if (textElement != null)
            textElement.text = text;
            
        if (portraitElement != null)
            portraitElement.sprite = defaultPortrait;

        root.SetActive(true);
        ActivateSpeaker(alignment);
    }
    
    private void ActivateSpeaker(DialogueLine.Alignment alignment)
    {
        leftText.gameObject.SetActive(false);
        leftPortrait.gameObject.SetActive(false);
        rightText.gameObject.SetActive(false);
        rightPortrait.gameObject.SetActive(false);
        middleText.gameObject.SetActive(false);

        switch (alignment)
        {
            case DialogueLine.Alignment.Left:
                leftText.color = activeSpeakerColor;
                leftPortrait.color = activeSpeakerColor;
                leftText.gameObject.SetActive(true);
                leftPortrait.gameObject.SetActive(true);
                break;
            case DialogueLine.Alignment.Right:
                rightText.color = activeSpeakerColor;
                rightPortrait.color = activeSpeakerColor;
                rightText.gameObject.SetActive(true);
                rightPortrait.gameObject.SetActive(true);
                break;
            case DialogueLine.Alignment.Center:
                middleText.color = activeSpeakerColor;
                middleText.gameObject.SetActive(true);
                break;
        }
    }
}
