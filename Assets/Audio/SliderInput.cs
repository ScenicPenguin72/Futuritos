using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderInput : MonoBehaviour
{
    public Slider slider;
    public float step = 0.1f;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            float input = Input.GetAxisRaw("Horizontal");
            if (input != 0)
            {
                slider.value += input * step * Time.deltaTime * 10f; 
            }
        }
    }
}
