using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Keys : Singleton<Keys>
{
    public List<Key> keys { get; private set; } = new List<Key>();
    private void Start()
    {
        LoadKeys();
    }
    public bool HasKey(Key key)
    {
        foreach (Key k in keys)
            if (k.id == key.id)
                return true;
        return false;
    }
    public void AddKey(Key key)
    {
        if (!HasKey(key))
            keys.Add(key);
    }
    void LoadKeys()
    {
        int id = LevelManager.Instance.possibleKeys.Length;
        int picked = 0;
        for(int i=0;i<id;i++)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.PickedUpKeys + i))
            {
                picked = PlayerPrefs.GetInt(PlayerPrefsKeys.PickedUpKeys + i);
                if (picked == 1)
                {
                    keys.Add(LevelManager.Instance.possibleKeys[i]);
                    picked = 0;
                }

            }
        }
    }
}
