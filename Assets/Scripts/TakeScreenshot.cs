using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ScreenCapture.CaptureScreenshot("Jaworzna-heatmap-view.png");
        }
    }
}
