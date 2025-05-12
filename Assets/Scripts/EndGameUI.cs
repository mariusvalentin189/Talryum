using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameUI : MonoBehaviour
{
    bool canContinue;
    [SerializeField] TMP_Text keyText;
    private void Start()
    {
        keyText.text = $"Press {PlayerKeys.Instance.interactKey} to continue";
        StartCoroutine(CanContinue());
    }
    void Update()
    {
        if(Input.GetKeyDown(PlayerKeys.Instance.interactKey) && canContinue)
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
    IEnumerator CanContinue()
    {
        yield return new WaitForSeconds(0.2f);
        canContinue = true;
        Time.timeScale = 0;
    }
}
