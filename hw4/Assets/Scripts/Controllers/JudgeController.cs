using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeController : MonoBehaviour
{
    public FirstController mainController;
    public Shore leftShoreModel;
    public Shore rightShoreModel;
    public Boat boatModel;
    // Start is called before the first frame update
    void Start()
    {
        mainController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
        this.leftShoreModel = mainController.leftShoreController.GetShore();
        this.rightShoreModel = mainController.rightShoreController.GetShore();
        this.boatModel = mainController.boatController.GetBoatModel();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainController.isRunning)
            return;
        if (mainController.time <= 0)
        {
            mainController.JudgeCallback(false, "Game Over!");
            return;
        }
        this.gameObject.GetComponent<UserGUI>().gameMessage = "";
        //判断是否已经胜利
        if (rightShoreModel.priestCount == 3)
        {
            mainController.JudgeCallback(false, "You Win!");
            return;
        }
        else
        {
            
            int leftPriestNum, leftDevilNum, rightPriestNum, rightDevilNum;
            leftPriestNum = leftShoreModel.priestCount + (boatModel.isRight ? 0 : boatModel.priestCount);
            leftDevilNum = leftShoreModel.devilCount + (boatModel.isRight ? 0 : boatModel.devilCount);
            if (leftPriestNum != 0 && leftPriestNum < leftDevilNum)
            {
                mainController.JudgeCallback(false, "Game Over!");
                return;
            }
            rightPriestNum = rightShoreModel.priestCount + (boatModel.isRight ? boatModel.priestCount : 0);
            rightDevilNum = rightShoreModel.devilCount + (boatModel.isRight ? boatModel.devilCount : 0);
            if (rightPriestNum != 0 && rightPriestNum < rightDevilNum)
            {
                mainController.JudgeCallback(false, "Game Over!");
                return;
            }
        }
    }
}

