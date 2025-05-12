using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelManager : Singleton<BossLevelManager>
{
    [SerializeField] GameObject[] objectsToDestroy;
    [SerializeField] GameObject[] objectsToEnable;
    [SerializeField] GameObject[] objectsToDisable;
    [SerializeField] GameObject[] objectsToEnableOnEntrance;
    private void Awake()
    {
        int b;
        Boss boss = FindObjectOfType<Boss>();
        if (PlayerPrefs.HasKey(PlayerPrefsKeys.BossId + boss.bossId))
        {
            b = PlayerPrefs.GetInt(PlayerPrefsKeys.BossId + boss.bossId);
            if (b == 1)
            {
                boss.DisableBoss();
                Destroy(gameObject);
            }
        }
    }
    public void EnableEntrance()
    {
        foreach (GameObject obj in objectsToDestroy)
            Destroy(obj);
        foreach (GameObject obj in objectsToDisable)
            obj.SetActive(false);
        foreach (GameObject obj in objectsToEnable)
            obj.SetActive(true);
    }
    public void DisableObjects()
    {
        foreach (GameObject obj in objectsToEnable)
            obj.SetActive(false);
    }
    public void EnableObjectsOnEntrance()
    {
        foreach (GameObject obj in objectsToEnableOnEntrance)
            obj.SetActive(true);
    }
}
