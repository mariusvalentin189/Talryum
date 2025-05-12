using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] TMP_Text valueText;
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(2f * Time.deltaTime * transform.up);
    }
    public void ShowValue(int value)
    {
        valueText.text = "+" + value.ToString();
    }
}
