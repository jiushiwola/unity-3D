using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position  //存储所有对象的位置
{
    //固定位置（世界坐标）
    public static Vector3 left_shore = new Vector3(-8,-3,0);
    public static Vector3 right_shore = new Vector3(8,-3,0);
    public static Vector3 river = new Vector3(0,-4,0);
    public static Vector3 left_boat = new Vector3(-2.3f,-2.3f,-0.4f);
    public static Vector3 right_boat = new Vector3(2.4f, -2.3f, -0.4f);

    //角色相对于岸边的位置(相对坐标)
    public static Vector3[] role_shore = new Vector3[] {new Vector3(0.4f,0.77f,0), new Vector3(0.2f,0.77f,0), new Vector3(0,0.77f,0), new Vector3(-0.2f,0.77f,0), new Vector3(-0.4f,0.77f,0), new Vector3(-0.6f,0.77f,0)};
    
    //角色相对于船的位置(相对坐标)
    public static Vector3[] role_boat = new Vector3[] {new Vector3(0.2f,3,0), new Vector3(-0.2f,3,0)};


}
