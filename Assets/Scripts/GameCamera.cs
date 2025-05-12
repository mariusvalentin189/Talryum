using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameCamera : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject panel;
    Transform levelBoss;
    Transform player;
    CinemachineVirtualCamera virtualCamera;
    public bool StartBossLevel { get; set; }
    private void Start()
    {
        anim = GetComponent<Animator>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        player = FindObjectOfType<PlayerController>().transform;
        if (GameObject.FindGameObjectWithTag("BossEnemy"))
        {
            levelBoss = GameObject.FindGameObjectWithTag("BossEnemy").transform;
        }
        FadeOut();
    }
    private void Update()
    {
        if(StartBossLevel)
        {
            FollowBoss();
            StartBossLevel = false;
        }
    }
    public void PlayHitEnemyAnimation()
    {
        anim.SetTrigger("ShakeCamera");
    }
    public void PlayGotHitAnimation()
    {
        anim.SetTrigger("ShakeCamera");
    }
    public void PlayGotHitAnimation2()
    {
        anim.SetTrigger("ShakeCamera2");
    }
    public void FadeOut()
    {
        anim.SetTrigger("FadeOut");
    }
    public void FadeIn()
    {
        anim.SetTrigger("FadeIn");
    }
    public void EnablePanel()
    {
        panel.SetActive(true);
    }
    public void DisablePanel()
    {
        panel.SetActive(false);
    }
    public void FollowPlayer()
    {
        virtualCamera.Follow = player;
        virtualCamera.LookAt = player;
        player.GetComponent<PlayerController>().CanMove = true;
    }
    public void FollowBoss()
    {
        virtualCamera.Follow = levelBoss.transform.GetChild(0);
        virtualCamera.LookAt = levelBoss.transform.GetChild(0);      
    }
    public void FollowNone()
    {
        virtualCamera.Follow = null;
        virtualCamera.LookAt = null;
    }
    public void CloseUp()
    {
        anim.SetTrigger("Death");
    }
}
