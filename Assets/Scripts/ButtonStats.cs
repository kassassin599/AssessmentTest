using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStats : MonoBehaviour
{
    public int currentPosInCicle = 0;

    public bool canMove = false;

    private Button _button;

    public float moveSpeed = 50f; // Speed of movement
    private float t = 0f; // Lerp time

    int steps = 0;
    int currentStep = 0;

    public MoveTabs moveTabs;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
    }

    public void SetSteps(int stepsReq)
    {
        this.steps = stepsReq;
        _button.interactable = false;
        canMove = true;
    }

    public void Update()
    {
        if (!canMove || steps == 0)
            return;
        if (steps > 0)
            Move();
        else if (steps < 0)
            MoveBack();
    }

    private void Move()
    {
        t += Time.deltaTime * moveSpeed;

        int nextPosInCicle = currentPosInCicle + 1;

        if (nextPosInCicle >= 8)
        {
            nextPosInCicle = 0;
        }

        transform.position = Vector3.Lerp(moveTabs.Targetpositions[currentPosInCicle].position,
            moveTabs.Targetpositions[nextPosInCicle].position, t);

        if (t >= 1f)
        {
            t = 0f; // Reset Lerp time
            currentPosInCicle = nextPosInCicle;
            bool canstay = moveTabs.Targetpositions[currentPosInCicle].GetComponent<CheckPos>().canHoldButton;
            if (!canstay)
            {
                steps += 1;
            }

            steps -= 1;
            if (steps <= 0)
            {
                canMove = false;
                _button.interactable = true;
            }
        }
    }

    private void MoveBack()
    {
        t += Time.deltaTime * moveSpeed;

        int prevPosInCicle = currentPosInCicle - 1;

        if (prevPosInCicle < 0)
        {
            prevPosInCicle = 7;
        }

        transform.position = Vector3.Lerp(moveTabs.Targetpositions[currentPosInCicle].position,
            moveTabs.Targetpositions[prevPosInCicle].position, t);

        if (t >= 1f)
        {
            t = 0f; // Reset Lerp time
            currentPosInCicle = prevPosInCicle;
            bool canstay = moveTabs.Targetpositions[currentPosInCicle].GetComponent<CheckPos>().canHoldButton;
            if (!canstay)
            {
                steps -= 1;
            }

            steps += 1;
            if (steps >= 0)
            {
                canMove = false;
                _button.interactable = true;
            }
        }
    }
}