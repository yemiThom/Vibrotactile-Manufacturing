using UnityEngine;

[CreateAssetMenu(fileName = "EngineComponentObjects", menuName = "EngineComponent/EngineComponentObjects", order = 0)]
public class EngineComponentObjects : ScriptableObject
{
    public string componentName;
    [TextArea(1,10)] public string componentDesc;

    public int stationNumber;
    public int orderNumber;
}
