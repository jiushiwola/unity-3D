using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MyException : System.Exception
{
    public MyException() { }
    public MyException(string message) : base(message) { }
}
public class DiskAttributes : MonoBehaviour
{
    //public GameObject gameobj;
    public int score;
    public float speedX;
    public float speedY;
}
public class DiskFactory : MonoBehaviour
{
    
    List<GameObject> used;
    List<GameObject> free;
    System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        
        used = new List<GameObject>();
        free = new List<GameObject>();
        rand = new System.Random();
        //Disk disk = GetDisk(1); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetDisk(int round) {
        GameObject disk;
        if (free.Count != 0) {
            disk = free[0];
            //used.Add(free[0]);
            free.Remove(disk);
            //disk.SetActive(true);
        }
        else {
            disk = GameObject.Instantiate(Resources.Load("Prefabs/disk", typeof(GameObject))) as GameObject;
            disk.AddComponent<DiskAttributes>();
            disk.AddComponent<Rigidbody>();
            //used.Add(disk.GetComponent<DiskAttributes>());
        }
        
        //根据不同round设置diskAttributes的值

        //随意的旋转角度
        disk.transform.localEulerAngles = new Vector3(-rand.Next(20,40),0,0);

        DiskAttributes attri = disk.GetComponent<DiskAttributes>();
        attri.score = rand.Next(1,4);
        //由分数来决定速度、颜色、大小
        attri.speedX = (rand.Next(1,5) + attri.score + round) * 0.2f;
        attri.speedY = (rand.Next(1,5) + attri.score + round) * 0.2f;
        
        
        if (attri.score == 3) {
            disk.GetComponent<Renderer>().material.color = Color.red;
            disk.transform.localScale += new Vector3(-0.5f,0,-0.5f);
        }
        else if (attri.score == 2) {
            disk.GetComponent<Renderer>().material.color = Color.green;
            disk.transform.localScale += new Vector3(-0.2f,0,-0.2f);
        }
        else if (attri.score == 1) {
            disk.GetComponent<Renderer>().material.color = Color.blue;
            
        }
        
        //飞碟可从四个方向飞入（左上、左下、右上、右下）
        int direction = rand.Next(1,5);
        //print(attri.score);
        //print(direction);
        //direction = 3;
        if (direction == 1) {
            disk.transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight * 1.5f, 8)));
            attri.speedY *= -1;
        }
        else if (direction == 2) {
            disk.transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight * 0f, 8)));
            
        }
        else if (direction == 3) {
            disk.transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight * 1.5f, 8)));
            attri.speedX *= -1;
            attri.speedY *= -1;
        }
        else if (direction == 4) {
            disk.transform.Translate(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight * 0f, 8)));
            attri.speedX *= -1;
        }
        used.Add(disk);
        disk.SetActive(true);
        Debug.Log("generate disk");
        return disk;
    }

    public void FreeDisk(GameObject disk) {
        disk.SetActive(false);
        //将位置和大小恢复到预制，这点很重要！
        disk.transform.position = new Vector3(0, 0,0);
        disk.transform.localScale = new Vector3(2f,0.1f,2f);
        if (!used.Contains(disk)) {
            throw new MyException("Try to remove a item from a list which doesn't contain it.");
        }
        Debug.Log("free disk");
        used.Remove(disk);
        free.Add(disk);
    }
}
