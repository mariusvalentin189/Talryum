using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevTools : Singleton<DevTools>
{
    public bool playerInvulnerable;
    public bool isDebugging;
    public LevelType levelType;
    public bool isDev;
    public bool isTestLevel;
    HealthManager playerHealth;
    PlayerController player;
    Sword playerSword;
    int minDamage, maxDamage;
    float moveSpeed, jumpForce;
    private void Awake()
    {
        if (levelType != LevelType.menu)
        {
            playerHealth = FindObjectOfType<HealthManager>();
            player = FindObjectOfType<PlayerController>();
            playerSword = FindObjectOfType<Sword>();
            minDamage = playerSword.MinDamage;
            maxDamage = playerSword.MaxDamage;
            moveSpeed = player.MoveSpeed;
            jumpForce = player.JumpForce;
            if (isTestLevel)
            {
                GameUI.Instance.ClearGameData();
                playerHealth.GodMode = true;
                playerSword.MinDamage = 99;
                playerSword.MaxDamage = 99;
            }
        }
    }
    public void LoadTestScene()
    {
        GameUI.Instance.ClearGameData();
        SceneManager.LoadSceneAsync("TestScene");
    }
    private void Update()
    {
        if (levelType == LevelType.menu)
            return;
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            isDev = !isDev;
        }
        if(playerHealth.GodMode)
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                player.MoveSpeed += 10f * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                player.MoveSpeed -= 10f * Time.deltaTime;
                player.MoveSpeed = Mathf.Clamp(player.MoveSpeed, moveSpeed, 100f);
            }
        }
        if (isDev)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameUI.Instance.ClearGameData();
                SceneManager.LoadSceneAsync(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameUI.Instance.ClearGameData();
                SceneManager.LoadSceneAsync(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameUI.Instance.ClearGameData();
                SceneManager.LoadSceneAsync(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                GameUI.Instance.ClearGameData();
                SceneManager.LoadSceneAsync(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                GameUI.Instance.ClearGameData();
                SceneManager.LoadSceneAsync(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                GameUI.Instance.ClearGameData();
                SceneManager.LoadSceneAsync(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                GameUI.Instance.ClearGameData();
                SceneManager.LoadSceneAsync(7);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                GameUI.Instance.ClearGameData();
                SceneManager.LoadSceneAsync(8);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                GameUI.Instance.ClearGameData();
                SceneManager.LoadSceneAsync(13);
            }
            if (Input.GetKeyDown(KeyCode.C))
                PlayerCoins.Instance.AddCoins(100);
        }
        if (isDev || isTestLevel)
        {
            //God mode
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (!playerHealth.GodMode)
                {
                    playerHealth.GodMode = true;
                    playerSword.MinDamage = 50;
                    playerSword.MaxDamage = 100;
                    player.MoveSpeed *= 3;
                    player.JumpForce *= 1;
                }
                else
                {
                    playerHealth.GodMode = false;
                    playerSword.MinDamage = minDamage;
                    playerSword.MaxDamage = maxDamage;
                    player.MoveSpeed = moveSpeed;
                    player.JumpForce = jumpForce;
                }
            }
        }
    }
}
