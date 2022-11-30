using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed;

    private void FixedUpdate()
    {
        Vector3 target = new Vector3(player.position.x, player.position.y, -10);
        Vector3 pos = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);

        transform.position = pos;
    }
}