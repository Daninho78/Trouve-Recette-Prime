using UnityEngine;

public class KeyboardAdjuster : MonoBehaviour
{
    public RectTransform panel;

    private Vector2 originalPos;

    void Start()
    {
        originalPos = panel.anchoredPosition;
    }

    void Update()
    {
        if (TouchScreenKeyboard.visible)
        {
            panel.anchoredPosition = new Vector2(originalPos.x, 300);
        }
        else
        {
            panel.anchoredPosition = originalPos;
        }
    }
}