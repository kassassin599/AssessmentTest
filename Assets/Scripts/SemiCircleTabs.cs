using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SemiCircleTabs : MonoBehaviour
{
    public List<Button> buttons; // Assign buttons in the Inspector
    private int activeIndex = 0; // Default active button index
    public float radius = 150f; // Radius for semicircle layout

    void Start()
    {
        PositionButtons(); // Arrange buttons initially

        // Assign click events to buttons
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => SetActiveTab(index));
        }
    }

    void PositionButtons()
    {
        int totalButtons = buttons.Count;
        float angleStep = 90f / (totalButtons - 1); // 90° spread for semicircle

        for (int i = 0; i < totalButtons; i++)
        {
            float angle = -45f + (i * angleStep); // Angle offset for symmetry
            Vector2 newPos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
            
            RectTransform rect = buttons[i].GetComponent<RectTransform>();
            rect.anchoredPosition = newPos;
        }

        SetActiveTab(activeIndex); // Set initial active tab
    }

    void SetActiveTab(int index)
    {
        activeIndex = index;

        for (int i = 0; i < buttons.Count; i++)
        {
            RectTransform rect = buttons[i].GetComponent<RectTransform>();

            if (i == activeIndex)
            {
                rect.anchoredPosition = new Vector2(radius, 0); // Move to center
            }
            else
            {
                float angle = -45f + (i * (90f / (buttons.Count - 1)));
                Vector2 newPos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
                rect.anchoredPosition = newPos;
            }
        }
    }
}
