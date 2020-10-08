using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shore 
{
    public GameObject shore;
    public int priestCount, devilCount;
    public Shore (Vector3 position){
        shore = GameObject.Instantiate(Resources.Load("Prefabs/left_shore", typeof(GameObject))) as GameObject;
        shore.transform.localScale = new Vector3(8,4.8f,2);
        shore.transform.position = position;
        priestCount = devilCount = 0;
    }
}
