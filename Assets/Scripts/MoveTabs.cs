using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTabs : MonoBehaviour
{
    [SerializeField] private Transform[] targetpositions;
    public Button[] buttons;

    public float radius = 200f;
    public int addAngle = 77;

    private int mainPositionID = 2;

    public Transform[] Targetpositions
    {
        get => targetpositions;
        set => targetpositions = value;
    }

    private void Start()
    {
        PlotPositionsInCircle();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.position = targetpositions[i].position;
            buttons[i].GetComponent<ButtonStats>().currentPosInCicle = targetpositions[i].GetComponent<CheckPos>().id;
        }
    }

    private void PlotPositionsInCircle()
    {
        for (int i = 0; i < targetpositions.Length; i++)
        {
            targetpositions[i].GetComponent<CheckPos>().id = i;

            float angle = (i / (float)targetpositions.Length) * 2 * Mathf.PI;

            float x = transform.position.x + radius * Mathf.Cos(angle + addAngle);

            float y = transform.position.y + radius * Mathf.Sin(angle + addAngle);

            targetpositions[i].position = new Vector3(x, y);
        }
    }

    public void OnButtonClicked(ButtonStats buttonStats)
    {
        if (buttonStats.currentPosInCicle != mainPositionID)
        {
            int stepsReq = mainPositionID - buttonStats.currentPosInCicle;

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<ButtonStats>().SetSteps(stepsReq);
            }
        }
    }
}