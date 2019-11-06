using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float normalSpeed = 2f;

    private bool canScroll;
    private float currentSpeed;

    private Renderer myRenderer;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    float currentTime = 0;
    void Update()
    {
        if (canScroll)
        {
            currentTime += Time.deltaTime;
            Vector2 offset = new Vector2(currentTime * currentSpeed, 0.0f);
            myRenderer.material.mainTextureOffset = offset;
            if(currentTime >= 100f)
            {
                currentTime = 0; // Reset offset value after each 100 sec
            }
        }
    }

    public void SetScrollSpeed(float SpeedMultiplier = 0)
    {
        canScroll = true;
        currentSpeed = SpeedMultiplier > 0 ? (SpeedMultiplier * normalSpeed) : normalSpeed;
    }
}
