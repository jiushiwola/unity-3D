using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicActionManager : SSActionManager, IActionCallback, IActionManager
{
    public RoundController sceneController;
    public PhysicFlyAction action;
    public DiskFactory factory;
    
    // Start is called before the first frame update
    protected new void Start()
    {
        sceneController = (RoundController)SSDirector.getInstance().currentSceneController;
        sceneController.actionManager = this as IActionManager;
        factory = Singleton<DiskFactory>.Instance;
    }

    // Update is called once per frame
    // protected new void Update()
    // {
        
    // }

    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null) {
            // Debug.Log("飞碟被击中或飞出视界，动作结束，飞碟被销毁");
            // if (source.transform != null) {
            //     Destroy(source.transform.gameObject);
            // }
            factory.FreeDisk(source.transform.gameObject);
    }

    public override void MoveDisk(GameObject disk) {
        action = PhysicFlyAction.GetSSAction(disk.GetComponent<DiskAttributes>().speedX);
        RunAction(disk, action, this);

    }

    public void Fly(GameObject disk) {
        MoveDisk(disk);
    }

    public int RemainActionCount() {
        return actions.Count;
    }
}
