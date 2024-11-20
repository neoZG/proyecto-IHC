using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CollectibleObject : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private ObjectCounter _objectCounter;

    void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _objectCounter = FindObjectOfType<ObjectCounter>();

        if (_grabInteractable != null && _objectCounter != null)
        {
            _grabInteractable.selectEntered.AddListener(OnGrabbed);
        }
    }

    private void OnDestroy()
    {
        if (_grabInteractable != null && _objectCounter != null)
        {
            _grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        _objectCounter.OnObjectGrabbed(_grabInteractable);
    }
}
