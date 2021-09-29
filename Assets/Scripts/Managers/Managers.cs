using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_Instance; } } // 유일한 매니저를 갖고온다.

    InputManager _input = new InputManager();
    ResourcesManager _resource = new ResourcesManager();
    UIManager _ui = new UIManager();

    public static InputManager Input { get { return Instance._input; } }
    public static ResourcesManager Resources { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }

    void Start()
    {
        Init();
    }
    
    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        if(s_Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers"};
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();
        }      
    }
}
