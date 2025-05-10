using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowFollower : MonoBehaviour
{
    public RectTransform arrow;          // The arrow object
    public Vector2 offset = new Vector2(-50, 0);  // Offset relative to selected element

    private GameObject lastSelected;

    void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current != null && current != lastSelected && current.GetComponent<Selectable>() != null)
        {
            lastSelected = current;

            RectTransform target = current.GetComponent<RectTransform>();

            // Parent the arrow to the selected UI element
            arrow.SetParent(target);

            // Position the arrow at the specified offset relative to the UI element
            arrow.anchoredPosition = offset;
        }
    }
}
