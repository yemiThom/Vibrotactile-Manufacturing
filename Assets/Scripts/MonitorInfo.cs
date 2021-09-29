using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "MonitorUIInfos", menuName = "MonitorUI/MonitorUIInfo", order = 1)]
public class MonitorInfo : ScriptableObject
{    
    public string welcomeTitleTxt;
    [TextArea(1,10)] public string[] welcomeBodyTxts;
    public string instructionTitleTxt;
    [TextArea(1,10)] public string[] instructionBodyTxts;
    public string finishedTitleTxt;
    [TextArea(1,10)] public string[] finishedBodyTxts;
    [TextArea(1,10)] public string extrasBodyTxt;
    
    public Sprite welcomeImage;
    public Sprite instructionImage;
}
