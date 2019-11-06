using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#region Enums
public enum eCarState
{
    IDLE = 0,
    BOUNCE = 1,
    MOVING = 2
}

public enum eCarLane
{
    LEFT = 0,
    RIGHT = 1
} 
#endregion

public class CarController : MonoBehaviour
{
    #region Properties
    private eCarState currentCarState;
    public eCarState CurrrentCarState
    {
        get { return currentCarState; }

        set { currentCarState = value; }
    }
    #endregion

    #region Inspector Fields
    public SpriteRenderer CarWheel;
    public AudioSource audioSource;
    public AudioClip carStart;
    public AudioClip carRunning;
    #endregion

    #region Private Fields
    private Animator CarAnimator;
    private float wheelSize;
    private Action onAnimCompleteCallback;
    private Dictionary<eCarLane, CarLane> CarLanes = new Dictionary<eCarLane, CarLane>(); 
    #endregion

    void Start()
    {
        CarAnimator = GetComponent<Animator>();
        SetCarState(eCarState.IDLE);
        wheelSize = CarWheel.bounds.size.y * CarWheel.transform.localScale.y;

        var carLane = GameObject.FindObjectsOfType<CarLane>();
        for (int i = 0; i < carLane.Length; i++)
        {
            var lane = carLane[i];
            if (!CarLanes.ContainsKey(lane.myLane))
            {
                CarLanes.Add(lane.myLane, lane);
            }
        }
    }

    public float GetLaneSpeed(Vector2 touchPoint)
    {
        float laneSpeed = 0;
        float roadLowerBound = CarLanes[eCarLane.RIGHT].myLaneMin;
        float roadUpperBound = CarLanes[eCarLane.LEFT].myLaneMax;
        float roadCentre = CarLanes[eCarLane.LEFT].myLaneMin;

        Vector3 myCarPosition = transform.position;

        float yTouchInCarSpace = TouchPositionInCarSpace(touchPoint.y);

        if (yTouchInCarSpace > roadUpperBound)
        {
            myCarPosition.y = roadUpperBound;
            laneSpeed = CarLanes[eCarLane.LEFT].myLaneSpeed;
        }
        else if (yTouchInCarSpace < roadLowerBound)
        {
            myCarPosition.y = roadLowerBound;
            laneSpeed = CarLanes[eCarLane.RIGHT].myLaneSpeed;
        }
        else if (yTouchInCarSpace > roadCentre && yTouchInCarSpace < roadUpperBound)
        {
            myCarPosition.y = yTouchInCarSpace;
            laneSpeed = CarLanes[eCarLane.LEFT].myLaneSpeed;
        }
        else if (yTouchInCarSpace < roadCentre && yTouchInCarSpace > roadLowerBound)
        {
            myCarPosition.y = yTouchInCarSpace;
            laneSpeed = CarLanes[eCarLane.RIGHT].myLaneSpeed;
        }

        transform.position = myCarPosition;
        CarAnimator.speed = laneSpeed;
        return laneSpeed;
    }

    public void SetCarState(eCarState state, Action onCompleteCallback = null)
    {
        CurrrentCarState = state;
        switch (state)
        {
            case eCarState.IDLE:
                {
                    CarAnimator.SetBool("bounce", false);
                    CarAnimator.SetBool("moving", false);
                }
                break;

            case eCarState.BOUNCE:
                {
                    PlaySound(carStart);
                    CarAnimator.SetBool("bounce", true);
                    onAnimCompleteCallback = onCompleteCallback;
                }
                break;

            case eCarState.MOVING:
                {
                    PlaySound(carRunning, true);
                    CarAnimator.SetBool("moving", true);
                }
                break;
            default:
                break;
        }
    }

    private void PlaySound(AudioClip audioClip, bool loop = false)
    {
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    private float TouchPositionInCarSpace(float touchPoint)
    {
        return (touchPoint + wheelSize);
    }

    public void BounceAnimOverCallback()
    {
        audioSource.Stop();
        SetCarState(eCarState.MOVING);
        float roadLowerBound = CarLanes[eCarLane.RIGHT].myLaneMin;
        transform.position = new Vector3(transform.position.x, roadLowerBound, transform.position.z);
        if (onAnimCompleteCallback != null)
            onAnimCompleteCallback();
    }
}
