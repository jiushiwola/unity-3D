using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    int score;
    public RoundController roundController;
    public UserGUI userGUI;
    // Start is called before the first frame update
    void Start()
    {
        roundController = (RoundController)SSDirector.getInstance().currentSceneController;
        roundController.scoreController = this;
        userGUI = this.gameObject.GetComponent<UserGUI>();
    }

    public void Record(GameObject disk) {
        score += disk.GetComponent<DiskAttributes>().score;
        userGUI.score = score;
    }

    
}
