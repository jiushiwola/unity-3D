using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHalo : MonoBehaviour
{
    private ParticleSystem particleSys;  // 粒子系统
    private ParticleSystem.Particle[] particleArr;  // 粒子数组
    private CirclePosition[] circle; // 极坐标数组

    public int count = 10000;       // 粒子数量
    public float size = 0.03f;      // 粒子大小
    public float minRadius = 5.0f;  // 最小半径
    public float maxRadius = 12.0f; // 最大半径
    public bool clockwise = false;   // 顺时针|逆时针
    public float speed = 2f;        // 速度
    public float pingPong = 0.02f;  // 游离范围
    // Start is called before the first frame update
    public Gradient colorGradient;
    void Start ()
    {   
        clockwise = false;
        // 初始化粒子数组
        particleArr = new ParticleSystem.Particle[count];
        circle = new CirclePosition[count];
 
        // 初始化粒子系统
        particleSys = this.GetComponent<ParticleSystem>();
        particleSys.startSpeed = 0;            // 粒子位置由程序控制
        particleSys.startSize = size;          // 设置粒子大小
        particleSys.loop = false;
        particleSys.maxParticles = count;      // 设置最大粒子量
        particleSys.Emit(count);               // 发射粒子
        particleSys.GetParticles(particleArr);
 
        // 初始化梯度颜色控制器
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3];
        alphaKeys[0].time = 0.0f; alphaKeys[0].alpha = 0.0f;
        alphaKeys[1].time = 0.01f; alphaKeys[1].alpha = 1.0f;
        alphaKeys[2].time = 0.99f; alphaKeys[2].alpha = 1.0f;
        // alphaKeys[3].time = 0.6f; alphaKeys[3].alpha = 1.0f;
        // alphaKeys[4].time = 0.9f; alphaKeys[4].alpha = 0.4f;
        // alphaKeys[5].time = 1.0f; alphaKeys[5].alpha = 0.9f;
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].time = 0.0f; colorKeys[0].color = Color.white;
        colorKeys[1].time = 1.0f; colorKeys[1].color = Color.white;
        colorGradient.SetKeys(colorKeys, alphaKeys);
 
        RandomlySpread();   // 初始化各粒子位置
    }

    private int tier = 10;  // 速度差分层数
    public float time = 0f;
    void Update ()
    {
        time = (time + Time.deltaTime > 5 ? 5 : time + Time.deltaTime);
        int appear_count = (int)(time / 5.0f * count);
        for (int i = 0; i < count; i++)
        {
            if (clockwise)  // 顺时针旋转
                circle[i].angle -= (i % tier + 1) * (speed / circle[i].radius / tier);
            else            // 逆时针旋转
                circle[i].angle += (i % tier + 1) * (speed / circle[i].radius / tier);
 
            // 保证angle在0~360度
            circle[i].angle = (360.0f + circle[i].angle) % 360.0f;
            float theta = circle[i].angle / 180 * Mathf.PI;
            // 粒子在半径方向上游离
            circle[i].time += Time.deltaTime;
            circle[i].radius += Mathf.PingPong(circle[i].time / minRadius / maxRadius, pingPong) - pingPong / 2.0f;
            if (circle[i].radius < circle[i].startRadius) {
                circle[i].radius += 0.001f;
            }
            else {
                circle[i].radius -= 0.001f;
            }
            if (circle[i].radius > maxRadius) {
                circle[i].radius -= 0.01f;
            }
            if (circle[i].radius < minRadius) {
                circle[i].radius += 0.01f;
            }
            // if (circle[i].radius < minRadius) {
            //     circle[i].radius = (maxRadius + minRadius) / 2;
            // }
 
            particleArr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta),  circle[i].radius * Mathf.Sin(theta), 0);
            if (i > appear_count) {
                particleArr[i].color = colorGradient.Evaluate(0f);
            }
            else {
                particleArr[i].color = colorGradient.Evaluate(circle[i].angle / 360.0f);
            }
            
        }
     
        particleSys.SetParticles(particleArr, particleArr.Length);
    }

    void RandomlySpread()
    {
        for (int i = 0; i < count; ++i)
        {   // 随机每个粒子距离中心的半径，同时希望粒子集中在平均半径附近
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);
            // float radius = (float)i / count * 10f;
            // if (radius == 0) radius = 0.0001f;
            // 随机每个粒子的角度
            //float angle = Random.Range(0.0f, 360.0f);
            float angle = (float)i / count * 360.0f;
            //Debug.Log(angle);
            float theta = angle / 180 * Mathf.PI;
 
            // 随机每个粒子的游离起始时间
            float time = Random.Range(0.0f, 360.0f);
 
            circle[i] = new CirclePosition(radius, angle, time, radius);
 
            particleArr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta), circle[i].radius * Mathf.Sin(theta), 0f);
        }
 
        particleSys.SetParticles(particleArr, particleArr.Length);
    }
}

public class CirclePosition
{
    public float radius = 0f, angle = 0f, time = 0f, startRadius = 0f;
    public CirclePosition(float radius, float angle, float time, float startRadius)
    {
        this.radius = radius;   // 半径
        this.angle = angle;     // 角度
        this.time = time;       // 时间
        this.startRadius = startRadius;
    }
}

    