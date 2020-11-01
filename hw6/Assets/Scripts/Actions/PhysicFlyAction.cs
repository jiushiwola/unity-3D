using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//飞碟从界面左右两侧飞入，离开界面时运动结束
public class PhysicFlyAction : SSAction
{
    public float speedX;
    public static PhysicFlyAction GetSSAction(float x) {
        PhysicFlyAction action = ScriptableObject.CreateInstance<PhysicFlyAction>();
        action.speedX = x;
        return action;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(speedX * 10, 0, 0);
        gameObject.GetComponent<Rigidbody>().drag = 1;
    }

    // Update is called once per frame
    public override void Update()
    {
        //Debug.Log("flyaction update");
        if (this.transform.gameObject.activeSelf == false) {//飞碟已经被"销毁"
            //Debug.Log("1");
            this.destroy = true;
            this.callback.SSActionEvent(this);
            return;
        }
        
        Vector3 vec3 = Camera.main.WorldToScreenPoint (this.transform.position);
        if (vec3.x < -100 || vec3.x > Camera.main.pixelWidth + 100 || vec3.y < -100 || vec3.y > Camera.main.pixelHeight + 100) {
            //Debug.Log("2");
            this.destroy = true;
            this.callback.SSActionEvent(this);
            return;
        }
        
        
        
    }
}

