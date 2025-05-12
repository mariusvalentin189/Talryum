using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Background : MonoBehaviour
{
    [SerializeField] Vector2 speedMultiplier;
    [SerializeField] float continuousSpeed;
    [SerializeField] bool resetPosition;
    Transform target;
    Vector3 lastCameraPosition;
    float textureUnitSizeX;

    private void Start()
    {
        target = FindObjectOfType<CinemachineVirtualCamera>().transform;
        transform.position += new Vector3(0f, (target.transform.position.y + 1f) * speedMultiplier.y, 0f); //base pos for player on y axis is -1f
        lastCameraPosition = target.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        transform.position += new Vector3(continuousSpeed * speedMultiplier.x, 0f, 0f);
        Vector3 deltaMovement = target.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * speedMultiplier.x, deltaMovement.y * speedMultiplier.y, 0f);
        lastCameraPosition = target.position;
        if(Mathf.Abs(target.position.x - transform.position.x) >= textureUnitSizeX && resetPosition)
        {
            float offsetX = (target.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(target.position.x + offsetX, transform.position.y, transform.position.z);
        }
    }
}
