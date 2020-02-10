using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFriction : MonoBehaviour
{
    public float rayRange;

    PhysicsMaterial2D low, hight;

    Ray ray;

    public void Init()
    {

    }

    private void Start()
    {
        ray = new Ray(transform.position, Vector3.down * rayRange);
    }

    RaycastHit hit;
    private void Update()
    {
        if (Physics.Raycast(ray, out hit))
        {
            //hit.collider.material = low;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ray);
    }
}