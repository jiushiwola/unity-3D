using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    public CCActionManager actionManager;
    public ShoreCtrl leftShoreController, rightShoreController;
    public River river;
    public BoatCtrl boatController;
    public RoleCtrl[] roleControllers;
    //public MoveCtrl moveController;
    public bool isRunning;
    public float time;

    public void JudgeCallback(bool isRuning, string message)
    {
        this.gameObject.GetComponent<UserGUI>().gameMessage = message;
        this.gameObject.GetComponent<UserGUI>().time = (int)time;
        this.isRunning = isRunning;
    }
    public void LoadResources() {

        //role
        roleControllers = new RoleCtrl[6];
        for (int i = 0; i < 6; ++i) {
            roleControllers[i] = new RoleCtrl();
            roleControllers[i].CreateRole(Position.role_shore[i], i < 3 ? true : false, i);
        }

        //shore
        leftShoreController = new ShoreCtrl();
        leftShoreController.CreateShore(Position.left_shore);
        leftShoreController.GetShore().shore.name = "left_shore";
        rightShoreController = new ShoreCtrl();
        rightShoreController.CreateShore(Position.right_shore);
        rightShoreController.GetShore().shore.name = "right_shore";

        //将人物添加并定位至左岸  
        foreach (RoleCtrl roleController in roleControllers)
        {
            roleController.GetRoleModel().role.transform.localPosition = leftShoreController.AddRole(roleController.GetRoleModel());
        }
        //boat
        boatController = new BoatCtrl();
        boatController.CreateBoat(Position.left_boat);

        //river
        river = new River(Position.river);

        //move
        //moveController = new MoveCtrl();

        isRunning = true;
        time = 60;


    }

    public void MoveBoat() {
        if (isRunning == false || actionManager.IsMoving()) return;
        
        Vector3 destination = boatController.GetBoatModel().isRight ? Position.left_boat : Position.right_boat;
        actionManager.MoveBoat(boatController.GetBoatModel().boat, destination, 5);
        
        boatController.GetBoatModel().isRight = !boatController.GetBoatModel().isRight;
    
    }

    public void MoveRole(Role roleModel) {
        if (isRunning == false || actionManager.IsMoving()) return;
        Vector3 destination, mid_destination;
        if (roleModel.inBoat)
        {
            
            if (boatController.GetBoatModel().isRight)
                destination = rightShoreController.AddRole(roleModel);
            else
                destination = leftShoreController.AddRole(roleModel);
            if (roleModel.role.transform.localPosition.y > destination.y)
                mid_destination = new Vector3(destination.x, roleModel.role.transform.localPosition.y, destination.z);
            else
                mid_destination = new Vector3(roleModel.role.transform.localPosition.x, destination.y, destination.z);
            actionManager.MoveRole(roleModel.role, mid_destination, destination, 5);
            roleModel.onRight = boatController.GetBoatModel().isRight;
            boatController.RemoveRole(roleModel);
        }
        else
        {
            
            if (boatController.GetBoatModel().isRight == roleModel.onRight)
            {
                if (roleModel.onRight)
                {
                    rightShoreController.RemoveRole(roleModel);
                }
                else
                {
                    leftShoreController.RemoveRole(roleModel);
                }
                destination = boatController.AddRole(roleModel);
                if (roleModel.role.transform.localPosition.y > destination.y)
                    mid_destination = new Vector3(destination.x, roleModel.role.transform.localPosition.y, destination.z);
                else
                    mid_destination = new Vector3(roleModel.role.transform.localPosition.x, destination.y, destination.z);
                actionManager.MoveRole(roleModel.role, mid_destination, destination, 5);
            }
        }
    }

    public void Check() {
        // if (isRunning == false) return;
        // this.gameObject.GetComponent<UserGUI>().gameMessage = "";
        // if (rightShoreController.GetShore().priestCount == 3) {
        //     this.gameObject.GetComponent<UserGUI>().gameMessage = "You Win!";
        //     isRunning = false;
        // }
        // else {
        //     int leftPriestCount, rightPriestCount, leftDevilCount, rightDevilCount;
        //     leftPriestCount = leftShoreController.GetShore().priestCount + (boatController.GetBoatModel().isRight ? 0 : boatController.GetBoatModel().priestCount);
        //     rightPriestCount = rightShoreController.GetShore().priestCount + (boatController.GetBoatModel().isRight ? boatController.GetBoatModel().priestCount : 0);
        //     leftDevilCount = leftShoreController.GetShore().devilCount + (boatController.GetBoatModel().isRight ? 0 : boatController.GetBoatModel().devilCount);
        //     rightDevilCount = rightShoreController.GetShore().devilCount + (boatController.GetBoatModel().isRight ? boatController.GetBoatModel().devilCount : 0);
        //     if (leftPriestCount != 0 && leftPriestCount < leftDevilCount || rightPriestCount != 0 && rightPriestCount < rightDevilCount) {
        //         this.gameObject.GetComponent<UserGUI>().gameMessage = "Game Over!";
        //         isRunning = false;
        //     }
        // }
    }

    void Awake() {
        SSDirector.GetInstance().CurrentSceneController = this;
        LoadResources();
        this.gameObject.AddComponent<UserGUI>();
        this.gameObject.AddComponent<CCActionManager>();
        this.gameObject.AddComponent<JudgeController>();
    }

    void Update() {
        if (isRunning)
        {
            time -= Time.deltaTime;
            this.gameObject.GetComponent<UserGUI>().time = (int)time;
        }
    }
}
