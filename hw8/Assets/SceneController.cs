using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject ParticleRing;
    // Start is called before the first frame update
    void Start()
    {
        ParticleRing = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ParticleRing"), new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI() {
        if (ParticleRing.GetComponent<ParticleHalo>().time >= 5) 
        {
            if (GUI.Button(new Rect(Screen.width/2-20,Screen.height/2-10,40,40), "+")) {
                
                ParticleRing.GetComponent<ParticleHalo>().minRadius = ParticleRing.GetComponent<ParticleHalo>().minRadius == 5 ? 7 : 5;
                ParticleRing.GetComponent<ParticleHalo>().maxRadius = ParticleRing.GetComponent<ParticleHalo>().maxRadius == 12 ? 8 : 12;
            }
        }
    }
}
