using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CarController carController;
    public EnvironmentController envController;
    private Camera myCamera;

    void Start()
    {
        myCamera = Camera.main;
        carController.SetCarState(eCarState.BOUNCE, () => { envController.SetNewScrollSpeed(); });
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (carController != null && carController.CurrrentCarState == eCarState.MOVING)
            {
                Vector2 position = myCamera.ScreenToWorldPoint(Input.mousePosition);
                float carLaneSpeed = carController.GetLaneSpeed(position);
                envController.SetNewScrollSpeed(carLaneSpeed);
            }
        }
    }
}
