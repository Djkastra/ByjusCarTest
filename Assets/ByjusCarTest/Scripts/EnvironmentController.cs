using UnityEngine;
using System.Collections;

public class EnvironmentController : MonoBehaviour
{
    private BackgroundScroll[] scrollingLayers;
    void Start()
    {
        scrollingLayers = GetComponentsInChildren<BackgroundScroll>();
    }

    public void SetNewScrollSpeed(float laneSpeed = 0)
    {
        for (int i = 0; i < scrollingLayers.Length; i++)
        {
            scrollingLayers[i].SetScrollSpeed(laneSpeed);
        }
    }
}
