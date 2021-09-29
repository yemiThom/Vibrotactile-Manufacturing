using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;

public class EngineComponentController : MonoBehaviour
{
    public int id;

    public EngineComponentObjects EngineComponentObjects;    

    public GameObject[] spawnPoints;
    
    public bool isSnappedOn;
    public bool orderCorrect;

    public Transform originalParent;

    void Start()
    {
        GameEvents.Instance.onEngineComponentSnapDropped += OnEngineComponentSnapped;
        GameEvents.Instance.onEngineComponentUnsnapped += OnEngineComponentUnsnapped;
        GameEvents.Instance.onEngineComponentGrabbed += OnEngineComponentGrabbed;
        GameEvents.Instance.onEngineComponentLetGo += OnEngineComponentLetGo;

        originalParent = gameObject.transform.parent;
    }

    void Update()
    {
        
    }

    void OnEngineComponentSnapped(int id)
    {
        if(id == this.id)
        {
            //Debug.Log("An engine component has been snapped to the cylinder block!");
            SG_Grabable grabableObj = this.GetComponent<SG_Grabable>();

            GameController.Instance.CheckSnapOrder();
            grabableObj.SetInteractable(false);
            //grabableObj.pickupReference = GameController.Instance.engine.transform;
        }
    }

    void OnEngineComponentUnsnapped(int id)
    {
        if(id == this.id)
        {
            //Debug.Log("An engine component has been unsnapped from the cylinder block...");
            SG_Grabable grabableObj = this.GetComponent<SG_Grabable>();
            grabableObj.SetInteractable(true);
            //grabableObj.pickupReference = grabableObj.transform;
        }
    }

    void OnEngineComponentGrabbed(int id, string extrasTxt)
    {
        if(id == this.id)
        {
            UIController.Instance.ShowExtraInfoTxt(extrasTxt);
        }
    }

    void OnEngineComponentLetGo(int id)
    {
        if(id == this.id)
        {
            UIController.Instance.ShowDefaultExtrasTxt();
        }
    }
}
