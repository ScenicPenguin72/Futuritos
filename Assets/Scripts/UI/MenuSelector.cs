using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelector : MonoBehaviour
{
    public GameObject firstSelected;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
    //Funcion igual al menus focus, solo para cuando una escena inicie y no cuando se active el objecto con el script
}
