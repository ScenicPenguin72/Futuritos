using UnityEngine;
using System.Collections.Generic;

public class ColorCycler : MonoBehaviour
{
    public List<SpriteRenderer> spritesToCycle;

    public float switchTime = 1f;
    public float finalColorDelay = 1f;

    public Color color1 = new Color(1f, 0.5f, 0f, 1f); 
    public Color color2 = new Color(1f, 0f, 0f, 1f);   

    private SpriteRenderer exemptSprite;
    private Dictionary<SpriteRenderer, Color> originalColors = new();
    private float timer = 0f;
    private int colorIndex = 0;
    void Start()
    {
        exemptSprite = spritesToCycle[Random.Range(0, spritesToCycle.Count)];

        foreach (var sr in spritesToCycle)
        {
            originalColors[sr] = sr.color;
        }

        timer = switchTime;
    }

    void Update()
    {
        if (spritesToCycle.Count == 0 || exemptSprite == null || colorIndex >= 2) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (colorIndex == 0)
            {
                foreach (var sr in spritesToCycle)
                {
                    if (sr != exemptSprite)
                        sr.color = color1;
                }

                colorIndex = 1;
                timer = finalColorDelay; 
            }
            else if (colorIndex == 1)
            {
                foreach (var sr in spritesToCycle)
                {
                    if (sr != exemptSprite)
                    {
                        sr.color = color2;

                        GenerarAutos autosScript = sr.GetComponent<GenerarAutos>();
                        if (autosScript != null)
                        {
                            autosScript.enabled = true;
                        }
                    }
                }

                colorIndex = 2; 
            }

        }
    }
}
