using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.forward, Time.deltaTime / 10);
    }
}
