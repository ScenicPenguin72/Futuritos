using UnityEngine;

public class EnemyAnimationDrain : MonoBehaviour
{
    public float drainAmount = 0.05f;

    // Call this via an animation event
    public void DrainWithAnimation()
    {
        if (NivelAgua.instance != null)
        {
            NivelAgua.instance.ReduceSlider(drainAmount);
        }
    }
}
