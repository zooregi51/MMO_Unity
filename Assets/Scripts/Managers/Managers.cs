using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_Instance; } } // 유일한 매니저를 갖고온다.

    #region Contents
    GameManagerEx _game = new GameManagerEx();
    public static GameManagerEx Game {get {return Instance._game; }}
    #endregion

    #region Core
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourcesManager _resource = new ResourcesManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();

    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourcesManager Resources { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion

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

            s_Instance._data.Init();
            s_Instance._pool.Init();
            s_Instance._sound.Init();
        }      
    }

    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Init();
    }
}
