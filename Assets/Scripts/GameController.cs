using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<GameController>();
            }

            return instance;
        }
    }

    [SerializeField]
    private GameObject[] playerTeleportPoints;

    [SerializeField]
    private List<GameObject> stationOneComponents;
    [SerializeField]
    private List<GameObject> stationTwoComponents;
    [SerializeField]
    private List<GameObject> stationThreeComponents;
    [SerializeField]
    private List<GameObject> stationFourComponents;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject pulleyStand;
    [SerializeField]
    private GameObject yRotator;
    [SerializeField]
    private GameObject zRotator;
    public GameObject engine;

    public int stationNumber = 1;    
    private int teleportPointInt = 0;
    private int snapCounter = 1;
    private int orderNumber = 1;

    public float turnSpeed = 15f;

    private Quaternion engineRestRotation;

    void Awake() {
        StartCoroutine(ShowHideEngineComponents(stationOneComponents, false));
        StartCoroutine(ShowHideEngineComponents(stationTwoComponents, false));
        StartCoroutine(ShowHideEngineComponents(stationThreeComponents, false));
        StartCoroutine(ShowHideEngineComponents(stationFourComponents, false));

        player = GameObject.FindGameObjectWithTag("Player");
        pulleyStand = GameObject.FindGameObjectWithTag("PulleyStand");
        yRotator = GameObject.FindGameObjectWithTag("YRotator");
        zRotator = GameObject.FindGameObjectWithTag("ZRotator");
        engine = GameObject.FindGameObjectWithTag("BaseEngine");

        engineRestRotation = pulleyStand.transform.rotation;
    }

    void Start()
    {
        StartCoroutine(TeleportPlayerAfterSecs(0.5f));

        StartCoroutine(ShowHideEngineComponents(stationOneComponents, true));
    }

    void TeleportPlayer()
    {
        if(stationNumber > 1)
            UIController.Instance.TurnOffMonitor(stationNumber - 2);

        UIController.Instance.TurnOnMonitor(stationNumber - 1);

        float xPos = playerTeleportPoints[teleportPointInt].transform.position.x;
        float yPos = playerTeleportPoints[teleportPointInt].transform.position.y;
        float zPos = playerTeleportPoints[teleportPointInt].transform.position.z;

        player.transform.position = new Vector3(xPos, yPos + 0.5f, zPos);

        TeleportEngine();

        teleportPointInt++;
    }

    void TeleportEngine()
    {
        EngineComponentController componentController = engine.GetComponent<EngineComponentController>();

        float xPos = componentController.spawnPoints[teleportPointInt].transform.position.x;
        float yPos = componentController.spawnPoints[teleportPointInt].transform.position.y;
        float zPos = componentController.spawnPoints[teleportPointInt].transform.position.z;

        pulleyStand.transform.position = new Vector3(xPos, yPos, zPos);
        pulleyStand.transform.rotation = engineRestRotation;
    }

    void SpawnAllStationComponents(List<GameObject> components)
    {
        foreach (GameObject component in components)
        {
            EngineComponentController componentController = component.GetComponent<EngineComponentController>();

            if(componentController)
            {
                for (int i = 0; i < componentController.spawnPoints.Length; i++)
                {
                    float xPos = componentController.spawnPoints[i].transform.position.x;
                    float yPos = componentController.spawnPoints[i].transform.position.y;
                    float zPos = componentController.spawnPoints[i].transform.position.z;

                    component.transform.position = new Vector3(xPos, yPos, zPos);

                    i++;
                }
            }
        }
    }

    void RespawnStationComponent(GameObject component)
    {
        EngineComponentController componentController = component.GetComponent<EngineComponentController>();
        GameEvents.Instance.EngineComponentUnsnapped(componentController.id);
        component.transform.parent = componentController.originalParent;
        componentController.isSnappedOn = false;

        for (int i = 0; i < componentController.spawnPoints.Length; i++)
        {
            float xPos = componentController.spawnPoints[i].transform.position.x;
            float yPos = componentController.spawnPoints[i].transform.position.y;
            float zPos = componentController.spawnPoints[i].transform.position.z;

            component.transform.position = new Vector3(xPos, yPos, zPos);

            i++;
        }
    }

    IEnumerator CheckAllComponentsSnapped(List<GameObject> components)
    {
        int snappedCount = 0;
        int snappableCount = 0;

        foreach (GameObject component in components)
        {
            EngineComponentController componentController = component.GetComponent<EngineComponentController>();

            if(componentController)
            {
                snappableCount++;

                if(componentController.isSnappedOn)
                    snappedCount ++;
            }
        }

        if(snappedCount == snappableCount)
        {
            //Debug.Log("All components are snapped!");
            snapCounter = 1;
            stationNumber++;

            UIController.Instance.CallForFinishedTxts();

            yield return new WaitUntil(() => UIController.Instance.GetDoneBool() == true);

            StartCoroutine(TeleportPlayerAfterSecs(0.9f));

            switch (stationNumber)
            {
                case 2:
                    StartCoroutine(ShowHideEngineComponents(stationTwoComponents, true));
                    break;
                case 3:
                    StartCoroutine(ShowHideEngineComponents(stationThreeComponents, true));
                    break;
                case 4:
                    StartCoroutine(ShowHideEngineComponents(stationFourComponents, true));
                    break;
                default:
                    StartCoroutine(ShowHideEngineComponents(stationOneComponents, true));
                
                break;
            }
        }
    }

    public void OnYRotButtonPressed(bool rotFwd)
    {
        if(rotFwd)
        {
            yRotator.transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed);
        }
        else
        {
            yRotator.transform.Rotate(Vector3.up * Time.deltaTime * -turnSpeed);
        }
    }

    public void OnZRotButtonPressed(bool rotFwd)
    {
        if(rotFwd)
        {
            zRotator.transform.Rotate(Vector3.forward * Time.deltaTime * turnSpeed);
        }
        else
        {
            zRotator.transform.Rotate(Vector3.forward * Time.deltaTime * -turnSpeed); 
        }
    }

    public void CheckSnapOrder()
    {
        List<GameObject> components;

        switch (teleportPointInt)
        {
            case 1:
                //Debug.Log("teleportPointInt: " + teleportPointInt);
                components = stationOneComponents;
                break;
            case 2:
                //Debug.Log("teleportPointInt: " + teleportPointInt);
                components = stationTwoComponents;
                break;
            case 3:
                //Debug.Log("teleportPointInt: " + teleportPointInt);
                components = stationThreeComponents;
                break;
            case 4:
                //Debug.Log("teleportPointInt: " + teleportPointInt);
                components = stationFourComponents;
                break;
            default:
                //Debug.Log("teleportPointInt: " + teleportPointInt);
                components = stationOneComponents;
                break;
        }

        //Debug.Log("Components: " + components.ToString());

        foreach (GameObject component in components)
        {
            EngineComponentController componentController = component.GetComponent<EngineComponentController>();

            if(componentController && componentController.isSnappedOn && !componentController.orderCorrect)
            {
                //Debug.Log("Found object that is snapped...");
                if(componentController.EngineComponentObjects.orderNumber == snapCounter)
                {
                    Debug.Log("Order number is same as counter...so set orderCorrect to true");
                    Debug.Log("Snap counter: " + snapCounter);
                    componentController.orderCorrect = true;

                    if(CheckSameOrderComponents(components))
                        UIController.Instance.ShowInstructionTxts(snapCounter);
                }
                else if(componentController.EngineComponentObjects.orderNumber == snapCounter + 1)
                {
                    Debug.Log("Order number is greater than counter by 1...so add to snapCounter, and checkAllComponentsSnapped");
                    //StartCoroutine(UIController.Instance.ShowInstructionTxts(orderNumber));
                    snapCounter++;
                    orderNumber++;
                    Debug.Log("Snap counter: " + snapCounter);
                    componentController.orderCorrect = true;

                    if(CheckSameOrderComponents(components))
                        UIController.Instance.ShowInstructionTxts(snapCounter);

                    StartCoroutine(CheckAllComponentsSnapped(components));
                }
                else if(componentController.EngineComponentObjects.orderNumber <= snapCounter - 1 || componentController.EngineComponentObjects.orderNumber >= snapCounter + 1)
                {
                    Debug.Log("Order number is less than or greater than counter by 1...so unsnap and respawn component");
                    Debug.Log("Snap counter: " + snapCounter);
                    if(snapCounter > 1 && snapCounter > orderNumber){
                        snapCounter--;
                    }    
                    RespawnStationComponent(component);
                }
            }
        }
    }

    bool CheckSameOrderComponents(List<GameObject> components)
    {
        int sameSnapped = 0;

        List<GameObject> sameOrder = new List<GameObject>();

        foreach (GameObject component in components)
        {
            EngineComponentController componentController = component.GetComponent<EngineComponentController>();
            
            if(componentController)
            {
                if(componentController.EngineComponentObjects.orderNumber == snapCounter)
                {
                    sameOrder.Add(component);
                }
            }
        }

        for (int i = 0; i < sameOrder.Count; i++)
        {
            EngineComponentController componentController = sameOrder[i].GetComponent<EngineComponentController>();
            if(componentController.isSnappedOn)
                sameSnapped++;
        }

        if(sameSnapped == sameOrder.Count)
            return true;

        return false;
    }

    IEnumerator ShowHideEngineComponents(List<GameObject> components, bool show)
    {
        yield return new WaitForSeconds(1f);

        if(show)
        {
            foreach (GameObject component in components)
            {
                component.GetComponent<MeshRenderer>().enabled = true;
            }

            SpawnAllStationComponents(components);
        }
        else
        {
            foreach (GameObject component in components)
            {
                component.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    IEnumerator TeleportPlayerAfterSecs(float time)
    {
        yield return new WaitForSeconds(time);

        TeleportPlayer();
    }
}
