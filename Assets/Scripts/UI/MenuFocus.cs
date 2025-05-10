using UnityEngine;
using UnityEngine.EventSystems;

public class MenuFocus : MonoBehaviour
{
    public GameObject firstSelected;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
