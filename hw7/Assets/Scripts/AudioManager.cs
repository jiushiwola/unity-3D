using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip gameoverClip;
    public AudioClip shootClip;

    public void PlayMusic(AudioClip clip)
    {
        FirstSceneController scene = SSDirector.GetInstance().CurrentScenceController as FirstSceneController;
        //在一个玩家的位置播放音乐
        AudioSource.PlayClipAtPoint(clip, scene.player.transform.position);
    }
    void OnEnable()
    {
        GameEventManager.GameoverChange += Gameover;
        GameEventManager.ShootChange += Shoot;
    }
    void OnDisable()
    {
        GameEventManager.GameoverChange -= Gameover;
        GameEventManager.ShootChange -= Shoot;
    }
    //播放游戏结束时音乐
    void Gameover()
    {
        PlayMusic(gameoverClip);
    }

    void Shoot() {
        FirstSceneController scene = SSDirector.GetInstance().CurrentScenceController as FirstSceneController;
        //在一个玩家的位置播放音乐
        AudioSource.PlayClipAtPoint(shootClip, scene.player.transform.position);
    }
}
