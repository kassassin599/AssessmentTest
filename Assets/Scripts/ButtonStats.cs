using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStats : MonoBehaviour
{
    public int skillID;
    public int currentPosInCicle = 0;
    public int nextPosInCicle;
    public bool canMove = false;

    private Button _button;

    int steps = 0;

    public MoveTabs moveTabs;

    public Transform centerObject;
    public Transform startPosition;
    public Transform stopPosition;

    private float startAngle, stopAngle;
    private float elapsedTime = 0f;
    public float duration = 3f;

    public float initialScale;
    public float finalScale;

    [SerializeField] bool altMoving = false;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        centerObject = moveTabs.centerButton.transform;
    }

    private void Start()
    {
        SetInitialScale();
    }

    private void SetInitialScale()
    {
        if (currentPosInCicle == 2)
        {
            transform.localScale = new Vector3(1.25f, 1.25f, 1);
        }
        else if (currentPosInCicle == 3 || currentPosInCicle == 1)
        {
            transform.localScale = new Vector3(1f, 1f, 1);
        }
        else if (currentPosInCicle == 4 || currentPosInCicle == 0)
        {
            transform.localScale = new Vector3(0.75f, 0.75f, 1);
        }

        initialScale = transform.localScale.x;
    }

    public void OnClickCenterButton()
    {
        altMoving = true;

        if (moveTabs.centerButtonOpen)
        {
            startPosition = moveTabs.Targetpositions[currentPosInCicle];

            stopPosition = moveTabs.AltTargetPositions[currentPosInCicle];
        }
        else
        {
            startPosition = moveTabs.AltTargetPositions[currentPosInCicle];

            stopPosition = moveTabs.Targetpositions[currentPosInCicle];
        }

        startAngle = Mathf.Atan2(
            startPosition.position.y - centerObject.position.y,
            startPosition.position.x - centerObject.position.x
        ) * Mathf.Rad2Deg;

        stopAngle = Mathf.Atan2(
            stopPosition.position.y - centerObject.position.y,
            stopPosition.position.x - centerObject.position.x
        ) * Mathf.Rad2Deg;

        if (moveTabs.centerButtonOpen)
        {
            if (stopAngle < startAngle)
                stopAngle += 360f;

            finalScale = 0.75f;

            finalRadius = moveTabs.altRadius;
        }
        else
        {
            if (stopAngle > startAngle)
                stopAngle += 360f;

            if (currentPosInCicle == 2)
            {
                finalScale = 1.25f;
            }
            else if (currentPosInCicle == 3 || currentPosInCicle == 1)
            {
                finalScale = 1f;
            }
            else if (currentPosInCicle == 4 || currentPosInCicle == 0)
            {
                finalScale = 0.75f;
            }

            finalRadius = Vector3.Distance(centerObject.position, startPosition.position);
        }


        canMove = true;
    }

    public void SetSteps(int stepsReq)
    {
        this.steps = stepsReq;
        _button.interactable = false;

        startPosition = moveTabs.Targetpositions[currentPosInCicle];

        nextPosInCicle = currentPosInCicle + steps;
        if (nextPosInCicle == 5)
            nextPosInCicle = 0;
        else if (nextPosInCicle == 6)
            nextPosInCicle = 1;
        else if (nextPosInCicle == -1)
            nextPosInCicle = 4;
        else if (nextPosInCicle == -2)
            nextPosInCicle = 3;
        stopPosition = moveTabs.Targetpositions[nextPosInCicle];

        startAngle = Mathf.Atan2(
            startPosition.position.y - centerObject.position.y,
            startPosition.position.x - centerObject.position.x
        ) * Mathf.Rad2Deg;

        stopAngle = Mathf.Atan2(
            stopPosition.position.y - centerObject.position.y,
            stopPosition.position.x - centerObject.position.x
        ) * Mathf.Rad2Deg;

        if (stepsReq > 0)
        {
            if (stopAngle < startAngle)
                stopAngle += 360f;
        }
        else if (stepsReq < 0)
        {
            if (startAngle < stopAngle) startAngle += 360f;
        }

        CalculateScale();

        radius = Vector3.Distance(centerObject.position, startPosition.position);

        canMove = true;
    }

    private void CalculateScale()
    {
        if (nextPosInCicle == 2)
        {
            finalScale = 1.25f;
        }
        else if (nextPosInCicle == 3 || nextPosInCicle == 1)
        {
            finalScale = 1f;
        }
        else if (nextPosInCicle == 4 || nextPosInCicle == 0)
        {
            finalScale = 0.75f;
        }
    }

    [SerializeField] private float radius;
    [SerializeField] private float finalRadius;

    public void Update()
    {
        if (canMove)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            float currentAngle = Mathf.Lerp(startAngle, stopAngle, smoothT);
            float radians = currentAngle * Mathf.Deg2Rad;

            float x = centerObject.position.x + radius * Mathf.Cos(radians);
            float y = centerObject.position.y + radius * Mathf.Sin(radians);

            transform.position = new Vector3(x, y, transform.position.z);

            float scaleValue = Mathf.Lerp(initialScale, finalScale, t);
            transform.localScale = new Vector3(scaleValue, scaleValue, 1);

            if (altMoving)
            {
                transform.position = Vector3.Lerp(startPosition.position, stopPosition.position, t);
            }

            if (t >= 1f)
            {
                t = 0;
                elapsedTime = 0;
                canMove = false;
                _button.interactable = false;
                if (moveTabs.centerButtonOpen)
                {
                    currentPosInCicle = nextPosInCicle;
                    _button.interactable = true;
                }

                initialScale = finalScale;
                radius = finalRadius;
                transform.position = stopPosition.position;
                altMoving = false;
            }
        }
    }
}