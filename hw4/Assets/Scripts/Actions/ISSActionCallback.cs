using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SSActionEventType:int {Started, Completed}
public interface ISSActionCallback
{
    //回调函数
    void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null);
}
