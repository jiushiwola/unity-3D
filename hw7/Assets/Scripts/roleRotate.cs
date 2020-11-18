using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roleRotate : MonoBehaviour
{
    public float speed = 3.0f;
    //public SceneController sceneController;
 
    void Start() {
        //sceneController = SSDirector.getInstance().currentScenceController as SceneController;
    }
 
    void Update() {
        //if (sceneController.gamestate == 1) {
            float mousX = Input.GetAxis("Mouse X") * speed;
            transform.Rotate(new Vector3(0, mousX, 0));
        //}
    }

}
