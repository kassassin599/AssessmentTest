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

    public float initialScale = 1f;
    public float finalScale = 1f;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        centerObject = moveTabs.transform;
    }

    private void Start()
    {
        // Invoke("SetInitialScale",0.1f);
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

            float scaleValue = Mathf.Lerp(initialScale, finalScale, t);
            transform.localScale = new Vector3(scaleValue, scaleValue, 1);

            if (t >= 1f)
            {
                t = 0;
                elapsedTime = 0;
                canMove = false;
                _button.interactable = true;
                currentPosInCicle = nextPosInCicle;
                initialScale = finalScale;
                transform.position = stopPosition.position;
            }
        }
    }
}