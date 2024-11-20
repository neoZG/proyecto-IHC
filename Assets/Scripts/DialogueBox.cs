using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;          // Dialogue lines
    public float[] lineDurations;   // Duration for each line in seconds
    public float textSpeed;         // Speed of typing animation
    private int index;              // Current line index

    // Start is called before the first frame update
    void Start()
    {
        // Do not start dialogue here anymore
        textComponent.text = string.Empty;
        gameObject.SetActive(false);  // Ensure dialogue box is initially hidden
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                SkipToNextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index]; // Instantly display the full text
            }
        }
    }

    // This method can be called externally to start the dialogue
    public void StartDialogue()
    {
        index = 0;
        gameObject.SetActive(true);  // Show the dialogue box when starting
        StartCoroutine(DisplayLine());
    }

    IEnumerator DisplayLine()
    {
        // Start typing the current line
        textComponent.text = string.Empty;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        // Automatically transition to the next line after its duration
        yield return new WaitForSeconds(lineDurations[index] - lines[index].Length * textSpeed);
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(DisplayLine());
        }
        else
        {
            gameObject.SetActive(false); // Hide the dialogue box at the end
        }
    }

    void SkipToNextLine()
    {
        StopAllCoroutines(); // Stop the current typing or waiting process
        NextLine();          // Immediately move to the next line
    }
}
