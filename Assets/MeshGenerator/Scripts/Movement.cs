using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float speed = 2f;

    private void Update()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"),0f);

        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.Self);
    }
}
