using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollTexture : MonoBehaviour
{
    [SerializeField] Vector2 speed;
    Image image;
    void Awake()
    {
        image = GetComponent<Image>();
        image.material = new Material(image.material); //Clone the original material
    }
    private void Update()
    {
        image.material.mainTextureOffset += speed * Time.deltaTime;
    }
}
