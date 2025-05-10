using UnityEngine;

public class EnemigoRobot : MonoBehaviour
{
    public float drainRate = 0.02f; // per second
    public bool TirarAgua = false;
    void Update()
    {
        if (NivelAgua.instance != null && TirarAgua)
        {
            NivelAgua.instance.ReduceSlider(drainRate * Time.deltaTime);
        }
    }
    public void setBool()
    {
        TirarAgua = !TirarAgua;   
    }
}
