using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monitor : MonoBehaviour
{
    public MonitorInfo monitorInfo;

    public CanvasGroup welcomeCanvasCG;
    public CanvasGroup instructionCanvasCG;
    public CanvasGroup extraInfoCanvasCG;

    public TextMeshProUGUI welcomeTitleTxt;
    public TextMeshProUGUI welcomeBodyTxt;
    public TextMeshProUGUI instructionTitleTxt;
    public TextMeshProUGUI instructionBodyTxt;
    public TextMeshProUGUI extraInfoBodyTxt;

    public Image welcomeImage;
    public Image instructionImage;
    public Image welcomeProgressCircle;
}
