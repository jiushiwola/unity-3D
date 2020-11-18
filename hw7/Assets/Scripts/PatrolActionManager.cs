using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolActionManager : SSActionManager
{
    private GoPatrolAction go_patrol;                            //巡逻兵巡逻

    public void GoPatrol(GameObject patrol)
    {
        go_patrol = GoPatrolAction.GetSSAction(patrol.transform.position);
        this.RunAction(patrol, go_patrol, this);
    }
    //停止所有动作
    public void DestroyAllAction()
    {
        DestroyAll();
    }

    public override void SSActionEvent(SSAction source, int intParam = 0, GameObject objectParam = null)
    {
        if(intParam == 0)
        {
            //侦查兵跟随玩家
            PatrolFollowAction follow = PatrolFollowAction.GetSSAction(objectParam.gameObject.GetComponent<PatrolData>().player);
            this.RunAction(objectParam, follow, this);
        }
        else if (intParam == 1) 
        {
            //侦察兵按照初始位置开始继续巡逻
            GoPatrolAction move = GoPatrolAction.GetSSAction(objectParam.gameObject.GetComponent<PatrolData>().start_position);
            this.RunAction(objectParam, move, this);
            //玩家逃脱
            Singleton<GameEventManager>.Instance.PlayerEscape();
        }
        else {
            
        }
    }
}
