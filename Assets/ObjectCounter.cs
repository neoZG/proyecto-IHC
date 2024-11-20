// using System.Collections;
// using System.Collections.Generic;
// using TMPro;  // Import TextMeshPro namespace
// using UnityEngine;

// public class ObjectCounter : MonoBehaviour
// {
//     private int _objectCounter = 0;
//     private TextMeshProUGUI _text;

//     // Start is called before the first frame update
//     void Start()
//     {
//         _text = GetComponent<TextMeshProUGUI>();
//         if (_text == null)
//         {
//             Debug.LogError("TextMeshProUGUI component is missing on this GameObject!");
//             return;
//         }

//         FireBullet.GunFired += IncreaseCounter;
//         UpdateText();
//     }

//     private void OnDestroy()
//     {
//         FireBullet.GunFired -= IncreaseCounter;
//     }

//     public void ResetCounter()
//     { 
// 
//         _objectCounter = 0;
//         UpdateText();
//     }

//     private void IncreaseCounter()
//     {
//         _objectCounter++;
//         UpdateText();
//     }

//     private void UpdateText()
//     {
//         _text.text = _objectCounter.ToString();
//     }
// }



using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectCounter : MonoBehaviour
{
    private int _objectCounter = 0;
    private TextMeshProUGUI _text;
    private HashSet<GameObject> collectedObjects = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        if (_text == null)
        {
            Debug.LogError("TextMeshProUGUI component is missing on this GameObject!");
            return;
        }

        UpdateText();
    }

    public void OnObjectGrabbed(XRBaseInteractable interactable)
    {
        Debug.Log($"Entered OnObjectGrabbed function");
        GameObject grabbedObject = interactable.gameObject;
        Debug.Log($"Grabbed: {grabbedObject.name}");

        // Add to collection if not already collected
        if (grabbedObject.CompareTag("Collectible") && !collectedObjects.Contains(grabbedObject))
        {
            Debug.Log($"ON IT");
            collectedObjects.Add(grabbedObject);
            IncreaseCounter();
            AddToMenu(grabbedObject.name);
        }
    }

    public void OnObjectReleased(XRBaseInteractable interactable)
    {
        GameObject releasedObject = interactable.gameObject;

        // Optional: Log the release
        Debug.Log($"Released: {releasedObject.name}");
    }

    private void IncreaseCounter()
    {
        _objectCounter++;
        UpdateText();
    }

    public void ResetCounter()
    {
        _objectCounter = 0;
        collectedObjects.Clear();
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"Collected: {_objectCounter}";
    }

    private void AddToMenu(string objectName)
    {
        Debug.Log($"Added {objectName} to menu");
        // Logic to update the menu UI with the object's name
    }
}
