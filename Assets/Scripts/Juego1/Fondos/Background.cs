using UnityEngine;
public class Background : MonoBehaviour
{
    public float scrollSpeed;
    private float offset;
    private Material material;
    public bool direction = false;
    public int facing = 1;
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }
    void Update()
    {
        offset+= Time.deltaTime * scrollSpeed;
        material.SetTextureOffset("_MainTex", new Vector2(direction == false ? 0: offset*facing, direction == false ? offset*facing: 0));

    }
}
