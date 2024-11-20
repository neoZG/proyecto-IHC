// using UnityEngine;
// using UnityEngine.InputSystem;

// public class WristUI : MonoBehaviour
// {
//     public InputActionAsset inputActions;
//     public Canvas wristUICanvas;

//     private InputAction _menu;
//     private void Start()
//     {
//         wristUICanvas.enabled = false;  // Start with inventory hidden
//         _menu = inputActions.FindActionMap("XRI LeftHand").FindAction("Menu");
//         _menu.Enable();
//         _menu.performed += ToggleMenu;
//     }

//     private void OnDestroy()
//     {
//         _menu.performed -= ToggleMenu;
//     }

//     public void ToggleMenu(InputAction.CallbackContext context)
//     {
//         wristUICanvas.enabled = !wristUICanvas.enabled;  // Toggle the visibility of the inventory UI
//     }

//     // Optionally, you can use a function here to close the inventory and make sure items stay in place
//     public void CloseInventory()
//     {
//         wristUICanvas.enabled = false;
//         // No need to do anything special; the item already stayed in place
//     }
// }







// using UnityEngine;
// using UnityEngine.InputSystem;

// public class WristUI : MonoBehaviour
// {
//     public InputActionAsset inputActions;

//     public Transform handPosition;  // The visible position (e.g., wrist/hand)
//     public Transform waistPosition; // The hidden position (e.g., behind/back)

//     private RectTransform _canvasRectTransform;
//     private InputAction _menu;

//     private bool isVisible = true; // Track the visibility state of the canvas
//     private Vector3 targetPosition; // The target position to move the canvas to
//     private float transitionSpeed = 5f; // Speed at which the canvas moves

//     private void Start()
//     {
//         _canvasRectTransform = GetComponent<RectTransform>();

//         // Initialize the menu action and enable it
//         _menu = inputActions.FindActionMap("XRI LeftHand").FindAction("Menu");
//         _menu.Enable();
//         _menu.performed += ToggleMenu;

//         // Initially set the position to the hand position
//         SetCanvasPosition(handPosition);
//     }

//     private void OnDestroy()
//     {
//         _menu.performed -= ToggleMenu;
//     }

//     public void ToggleMenu(InputAction.CallbackContext context)
//     {
//         isVisible = !isVisible;

//         if (isVisible)
//         {
//             // Move canvas to the hand position
//             targetPosition = handPosition.position;
//         }
//         else
//         {
//             // Move canvas to the waist/back position
//             targetPosition = waistPosition.position;
//         }
//     }

//     private void Update()
//     {
//         // Smoothly transition to the target position
//         _canvasRectTransform.position = Vector3.Lerp(_canvasRectTransform.position, targetPosition, Time.deltaTime * transitionSpeed);
//     }

//     private void SetCanvasPosition(Transform targetTransform)
//     {
//         // Update the canvas position and rotation to match the selected target
//         _canvasRectTransform.position = targetTransform.position;
//         _canvasRectTransform.rotation = targetTransform.rotation;
//     }
// }



// using UnityEngine;
// using UnityEngine.InputSystem;
// public class WristUI : MonoBehaviour
// {
//     public InputActionAsset inputActions;

//     private Canvas _wristUICanvas;
//     private InputAction _menu;
//     private void Start()
//     {
//         _wristUICanvas = GetComponent<Canvas>();
//         _menu = inputActions.FindActionMap("XRI LeftHand").FindAction("Menu");
//         _menu.Enable();
//         _menu.performed += ToggleMenu;
//     }

//     private void OnDestroy()
//     {
//         _menu.performed -= ToggleMenu;
//     }

//     public void ToggleMenu(InputAction.CallbackContext context)
//     {
//         _wristUICanvas.enabled = !_wristUICanvas.enabled;
//     }
// }









using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristUI : MonoBehaviour
{
    public InputActionAsset inputActions;
    public Canvas wristUICanvas;

    private InputAction _menu;
    private InputAction _toggleObjects;

    // List of objects to activate/deactivate
    public List<GameObject> objectsToToggle = new List<GameObject>();
    private bool objectsAreActive = true;

    // List of collected objects
    private List<string> collectedObjects = new List<string>();
    public UnityEngine.UI.Text collectedObjectsText;  // UI element to display collected items

    private void Start()
    {
        wristUICanvas.enabled = false;  // Start with UI hidden
        _menu = inputActions.FindActionMap("XRI LeftHand").FindAction("Menu");
        _toggleObjects = inputActions.FindActionMap("XRI LeftHand").FindAction("ToggleObjects");
        _menu.Enable();
        _menu.performed += ToggleMenu;
    }

    private void OnDestroy()
    {
        _menu.performed -= ToggleMenu;
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        wristUICanvas.enabled = !wristUICanvas.enabled;  // Toggle UI visibility
    }

    public void ToggleObjects()
    {
        objectsAreActive = !objectsAreActive;

        foreach (var obj in objectsToToggle)
        {
            if (obj != null)
            {
                obj.SetActive(objectsAreActive);
            }
        }
    }

    public void CollectObject(string objectName)
    {
        if (!collectedObjects.Contains(objectName))
        {
            collectedObjects.Add(objectName);
            UpdateCollectedObjectsUI();
        }
    }

    private void UpdateCollectedObjectsUI()
    {
        collectedObjectsText.text = "Collected Items:\n" + string.Join("\n", collectedObjects);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Example of collecting an object when it enters the trigger
        if (other.CompareTag("Collectible"))
        {
            CollectObject(other.gameObject.name);
            Destroy(other.gameObject);  // Optionally destroy the collected object
        }
    }
}
