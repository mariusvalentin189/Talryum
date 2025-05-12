using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeKingTrigger : MonoBehaviour
{
    [SerializeField] GameObject Gate;
    [SerializeField] Key throneRoomKey;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Keys.Instance.AddKey(throneRoomKey);
            PlayerPrefs.SetInt(PlayerPrefsKeys.PickedUpKeys + throneRoomKey.id, 1);
            Destroy(Gate);
        }
    }
}
