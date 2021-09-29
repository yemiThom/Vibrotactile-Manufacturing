using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    public static UIController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<UIController>();
            }

            return instance;
        }
    } 


    [SerializeField]
    List<GameObject> UIScreens = new List<GameObject>();

    Monitor stationMonitor;
    
    float welcomeTxtTiming = 10f;

    bool startCircleProgress = false;
    bool changeProgressCycle = false;
    bool doneWithFinishedTxts = false;

    void Start()
    {
        UIScreens.AddRange(GameObject.FindGameObjectsWithTag("Monitor"));

    }

    void Update()
    {
        if(startCircleProgress)
            SetWelcomeProgressCircle();
    }

    public void TurnOnMonitor(int station)
    {
        GameObject screenToTurnOn = UIScreens[station];
        Monitor monitorScreen = screenToTurnOn.GetComponent<Monitor>();
        stationMonitor = monitorScreen;

        SetCanvasGroupAlpha(monitorScreen, "welcome", 1);

        StartCoroutine("ShowWelcomeTxts");
    }

    public void TurnOffMonitor(int station)
    {
        GameObject screenToTurnOff = UIScreens[station];
        Monitor monitorScreen = screenToTurnOff.GetComponent<Monitor>();

        SetCanvasGroupAlpha(monitorScreen, "welcome", 0);
        SetCanvasGroupAlpha(monitorScreen, "instruction", 0);
        SetCanvasGroupAlpha(monitorScreen, "extra", 0);
    }

    void SetCanvasGroupAlpha(Monitor monitor, string canvasType, int alpha)
    {
        switch (canvasType)
        {
            case "welcome":
                monitor.welcomeCanvasCG.alpha = alpha;
                break;
            case "instruction":
                monitor.instructionCanvasCG.alpha = alpha;
                break;
            case "extra":
                monitor.extraInfoCanvasCG.alpha = alpha;
                break;
        }
    }

    void SetWelcomeProgressCircle()
    {
        float totalTime = welcomeTxtTiming;

        totalTime = totalTime - Time.deltaTime;

        if(!changeProgressCycle)
        {
            stationMonitor.welcomeProgressCircle.fillAmount -= 1/totalTime*Time.deltaTime;
        }
        else
        {
            stationMonitor.welcomeProgressCircle.fillAmount += 1/totalTime*Time.deltaTime;
        }        
    }

    IEnumerator ShowWelcomeTxts()
    {
        for (int i = 0; i < stationMonitor.monitorInfo.welcomeBodyTxts.Length; i++)
        {
            startCircleProgress = true;

            stationMonitor.welcomeTitleTxt.text = stationMonitor.monitorInfo.welcomeTitleTxt;
            stationMonitor.welcomeBodyTxt.text = stationMonitor.monitorInfo.welcomeBodyTxts[i];

            yield return new WaitForSeconds(welcomeTxtTiming);

            changeProgressCycle = !changeProgressCycle;
            stationMonitor.welcomeProgressCircle.fillClockwise = !stationMonitor.welcomeProgressCircle.fillClockwise;
        }
            
        startCircleProgress = false;

        ShowInstructionTxts(0);
    }

    IEnumerator ShowFinishedTxts()
    {
        doneWithFinishedTxts = false;
        
        for (int i = 0; i < stationMonitor.monitorInfo.finishedBodyTxts.Length; i++)
        {
            startCircleProgress = true;

            stationMonitor.welcomeTitleTxt.text = stationMonitor.monitorInfo.finishedTitleTxt;
            stationMonitor.welcomeBodyTxt.text = stationMonitor.monitorInfo.finishedBodyTxts[i];

            yield return new WaitForSeconds(welcomeTxtTiming);

            changeProgressCycle = !changeProgressCycle;
            stationMonitor.welcomeProgressCircle.fillClockwise = !stationMonitor.welcomeProgressCircle.fillClockwise;
        }
            
        startCircleProgress = false;
        doneWithFinishedTxts = true;
    }

    public void ShowInstructionTxts(int step)
    {
        SetCanvasGroupAlpha(stationMonitor, "welcome", 0);
        SetCanvasGroupAlpha(stationMonitor, "instruction", 1);
        SetCanvasGroupAlpha(stationMonitor, "extra", 1);

        ShowDefaultExtrasTxt();

        stationMonitor.instructionTitleTxt.text = stationMonitor.monitorInfo.instructionTitleTxt;
        if(step < stationMonitor.monitorInfo.instructionBodyTxts.Length)
            stationMonitor.instructionBodyTxt.text = stationMonitor.monitorInfo.instructionBodyTxts[step];
    }

    public void ShowDefaultExtrasTxt()
    {
        stationMonitor.extraInfoBodyTxt.text = stationMonitor.monitorInfo.extrasBodyTxt;
    }

    public void ShowExtraInfoTxt(string extraInfoTxt)
    {
        stationMonitor.extraInfoBodyTxt.text = extraInfoTxt;
    }

    public void CallForFinishedTxts()
    {
        SetCanvasGroupAlpha(stationMonitor, "welcome", 1);
        SetCanvasGroupAlpha(stationMonitor, "instruction", 0);
        SetCanvasGroupAlpha(stationMonitor, "extra", 0);

        StartCoroutine(ShowFinishedTxts());
    }

    public bool GetDoneBool()
    {
        return doneWithFinishedTxts;
    }
}
