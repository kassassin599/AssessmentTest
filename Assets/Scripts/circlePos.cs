using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circlePos : MonoBehaviour
{
    public int radius;
    public GameObject[] positions;

    private void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            positions[i] = GetComponentInChildren<GameObject>();

            positions[i].transform.position =
                new Vector3(10 * Mathf.Cos(2 * Mathf.PI), 10 * Mathf.Sin(2 * Mathf.PI), 0);
        }
    }
}