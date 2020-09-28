using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River
{
    public GameObject river;
    public River(Vector3 position) {
        river = GameObject.Instantiate(Resources.Load("Prefabs/river", typeof(GameObject))) as GameObject;
        river.transform.localScale = new Vector3(8,2.5f,2);
        river.transform.position = position;
        river.name = "river";
    }
}
