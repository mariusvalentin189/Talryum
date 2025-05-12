using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageUI : MonoBehaviour
{
    [SerializeField] TMP_Text damageText;
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(2f * Time.deltaTime * transform.up);
    }
    public void ShowDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
}
