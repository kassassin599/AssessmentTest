using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoveTabs : MonoBehaviour
{
    [SerializeField] private Transform[] targetpositions;
    [SerializeField] private GameObject[] skillPanels;
    public Button[] buttons;

    public float radius = 200f;
    public int addAngle = 77;

    private int mainPositionID = 2;

    public Button centerButton;
    [SerializeField] private GameObject[] turnOffGameobjects;

    [SerializeField] private Transform[] altTargetPositions;
    public float altRadius = 200f;

    public bool centerButtonOpen = true;

    public Transform[] Targetpositions
    {
        get => targetpositions;
        set => targetpositions = value;
    }

    public Transform[] AltTargetPositions
    {
        get => altTargetPositions;
        set => altTargetPositions = value;
    }

    private void Awake()
    {
        PlotPositionsInCircle();

        for (int i = 0; i < altTargetPositions.Length; i++)
        {
            altTargetPositions[i].GetComponent<CheckPos>().id = i;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.position = targetpositions[i].position;
            buttons[i].GetComponent<ButtonStats>().currentPosInCicle = targetpositions[i].GetComponent<CheckPos>().id;
            buttons[i].GetComponent<ButtonStats>().skillID = targetpositions[i].GetComponent<CheckPos>().id;
        }

        OnClickSkillButton(2);
    }


    public Animator canterButtonAnimator;

    public void OnClickCenterButton()
    {
        if (centerButtonOpen)
        {
            canterButtonAnimator.Play("CenterButtonClose");

            for (int i = 0; i < turnOffGameobjects.Length; i++)
            {
                turnOffGameobjects[i].SetActive(false);
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                // buttons[i].transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                buttons[i].interactable = true;
                buttons[i].GetComponent<ButtonStats>().OnClickCenterButton();
            }

            centerButton.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

            AltPlotPositionsInCircle();
        }
        else
        {
            canterButtonAnimator.Play("CenterButtonOpen");

            for (int i = 0; i < turnOffGameobjects.Length; i++)
            {
                turnOffGameobjects[i].SetActive(true);
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                // buttons[i].transform.localScale = new Vector3(1f, 1f, 1f);
                buttons[i].interactable = false;
                buttons[i].GetComponent<ButtonStats>().OnClickCenterButton();
            }

            centerButton.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        centerButtonOpen = !centerButtonOpen;
    }

    private void OnClickSkillButton(int skillID)
    {
        for (int i = 0; i < skillPanels.Length; i++)
        {
            skillPanels[i].SetActive(false);
        }

        skillPanels[skillID].SetActive(true);
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

    private void AltPlotPositionsInCircle()
    {
        for (int i = 0; i < altTargetPositions.Length; i++)
        {
            float angle = (i / (float)altTargetPositions.Length) * 2 * Mathf.PI;

            float x = transform.position.x + altRadius * Mathf.Cos(angle + addAngle);

            float y = transform.position.y + altRadius * Mathf.Sin(angle + addAngle);

            altTargetPositions[i].position = new Vector3(x, y);
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

        OnClickSkillButton(buttonStats.skillID);
    }
}