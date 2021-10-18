using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    // 1. 함수의 상태를 저장/복원 가능!
        // -> 엄청 오래 걸리는 작업을 잠시 끊거나
        // -> 원하는 타이밍에 함수를 잠시 Stop/복원하는 경우
    // 2. return -> 우리가 원하는 타입으로 가능 (class도 가능)

    Coroutine co;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        Managers.UI.ShowSceneUI<UI_Inven>();
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDic;

        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);
        Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
        //co = StartCoroutine("CoExplpodeAfterSeconds", 4.0f);
        //StartCoroutine("CoStopExplode", 2.0f);
    }

    IEnumerator CoStopExplode(float seconds)
    {
        Debug.Log("Stop Enter");
        yield return new WaitForSeconds(seconds);
        Debug.Log("Stop Execute!!!!");
        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }

    IEnumerator CoExplpodeAfterSeconds(float seconds)
    {
        Debug.Log("Expolde Enter");
        yield return new WaitForSeconds(seconds);
        Debug.Log("Expolde Execute!!!!");
        co = null;
    }

    public override void Clear()
    {

    }
}
