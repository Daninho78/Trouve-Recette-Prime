using UnityEngine;
using UnityEngine.UI;

public class ScrollToIngredientItem : MonoBehaviour
{
    public ScrollRect scrollRect;

    public void ScrollToBottom()
    {
        if (scrollRect == null)
            return;

        Canvas.ForceUpdateCanvases();

        scrollRect.StopMovement();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}