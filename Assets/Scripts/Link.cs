using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    private Vector3 startPoint, endPoint;
    [SerializeField] private float speed = 1;
    private bool isFinished = false;
    private bool isPointsSet = false;
    public List<Transform> connections;

    private void Awake()
    {
        connections = new List<Transform>();
    }

    private void Start()
    {
        transform.position = startPoint;
        transform.rotation = Quaternion.Euler(
                0,
                (-180 / Mathf.PI) * Mathf.Atan(((endPoint).z - (startPoint).z) / ((endPoint).x - (startPoint).x)),
                90);

    }
    private void Update()
    {
        if (isPointsSet && !isFinished)
        {
            transform.position = Vector3.MoveTowards(transform.position, (startPoint + endPoint) / 2, Time.deltaTime * speed);
            Vector3 scale = transform.localScale;
            scale.y = Vector3.Distance(startPoint, endPoint) / 2;
            transform.localScale = Vector3.MoveTowards(transform.localScale, scale, Time.deltaTime * speed);
            if (Vector3.Distance((startPoint + endPoint) / 2, transform.position) <= 0.05)
                isFinished = true;
        }
    }

    public bool Finished()
    {
        return isFinished;
    }

    public void SetPoints(Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        isPointsSet = true;
    }
}
