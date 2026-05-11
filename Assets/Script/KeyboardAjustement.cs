using UnityEngine;
using UnityEngine.UI;

public class KeyboardAdjuster : MonoBehaviour
{
    public LayoutElement keyboardSpacer;

    public float keyboardSpaceHeight = 350f;

    void Update()
    {
        if (TouchScreenKeyboard.visible)
        {
            keyboardSpacer.preferredHeight = keyboardSpaceHeight;
        }
        else
        {
            keyboardSpacer.preferredHeight = 0f;
        }
    }
}