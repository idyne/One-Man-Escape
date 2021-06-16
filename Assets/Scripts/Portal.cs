using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool up = true;
    private bool disappear = false;
    private float time = 0.142f;
    private void Update()
    {
        if (up)
            Up();
        else
            Disappear();
    }

    public void Up()
    {
        Vector3 pos = transform.position;
        pos.y = (time * 2.4f);
        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * 2.4f);
        time += Time.deltaTime;
        if (time > 1)
        {
            up = false;
            disappear = true;
        }
    }
    private void Disappear()
    {
        disappear = false;
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime * 10);
        if (transform.localScale.magnitude <= 0.1f)
        {
            Controller.SetPlayerPlaced();
            Destroy(gameObject);
        }
    }
}
