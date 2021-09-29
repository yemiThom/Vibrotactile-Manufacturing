using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private static GameEvents instance;
    public static GameEvents Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<GameEvents>();
            }

            return instance;
        }
    }    

    public event Action<int> onEngineComponentSnapDropped;
    public event Action<int> onEngineComponentUnsnapped;  
    public event Action<int, string> onEngineComponentGrabbed;
    public event Action<int> onEngineComponentLetGo;

    public void EngineComponentSnapDropped(int id)
    {
        if(onEngineComponentSnapDropped != null)
        {
            onEngineComponentSnapDropped(id);
        }
    }

    public void EngineComponentUnsnapped(int id)
    {
        if(onEngineComponentUnsnapped != null)
        {
            onEngineComponentUnsnapped(id);
        }
    }
    
    public void EngineComponentGrabbed(int id, string extrasTxt)
    {
        if(onEngineComponentGrabbed != null)
        {
            onEngineComponentGrabbed(id, extrasTxt);
        }
    }

    public void EngineComponentLetGo(int id)
    {
        if(onEngineComponentLetGo != null)
        {
            onEngineComponentLetGo(id);
        }
    }
}
