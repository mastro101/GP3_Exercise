using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float speed = 5f;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 position2D = VectorUtility.FromV3ToV2(transform.position);
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"),0f);
        direction.Normalize();
        Vector2 velocity = direction.normalized * speed * Time.deltaTime;

        rb.MovePosition(position2D + velocity);

        //transform.Translate(velocity, Space.Self);
    }
}