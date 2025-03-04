using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStats : MonoBehaviour
{
    public int currentPosInCicle = 0;
    public int nextPosInCicle;
    public bool canMove = false;

    private Button _button;

    int steps = 0;

    public MoveTabs moveTabs;

    public Transform centerObject; // The object to orbit around
    public Transform startPosition; // The position to start from
    public Transform stopPosition; // The position where movement should stop

    private float startAngle, stopAngle; // Start and stop angles in degrees
    private float elapsedTime = 0f; // Timer to track progress
    public float duration = 3f; // Time to complete movement

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        centerObject = moveTabs.transform;
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

        canMove = true;
    }

    public void Update()
    {
        if (canMove)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float currentAngle = Mathf.Lerp(startAngle, stopAngle, t);
            float radians = currentAngle * Mathf.Deg2Rad;

            float radius = Vector3.Distance(centerObject.position, startPosition.position);
            float x = centerObject.position.x + radius * Mathf.Cos(radians);
            float y = centerObject.position.y + radius * Mathf.Sin(radians);

            transform.position = new Vector3(x, y, transform.position.z);

            if (t >= 1f)
            {
                t = 0;
                elapsedTime = 0;
                canMove = false;
                _button.interactable = true;
                currentPosInCicle = nextPosInCicle;
                transform.position = stopPosition.position;
            }
        }
    }
}