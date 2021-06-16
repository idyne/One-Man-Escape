using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeNode : MonoBehaviour
{
    private Edge edge;
    private Animator anim;
    private Rigidbody rb;
    private float animationTime = 0;
    private float animationSpeed = 4f;
    private bool fall = false;

    private void Awake()
    {
        edge = transform.parent.parent.GetComponent<Edge>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Update()
    {
        if (!fall && GameManager.GAME_STATE == GameState.START)
            Animate();
    }

    private void Start()
    {
        transform.Rotate(new Vector3(0, Random.Range(0, 360f), 0), Space.World);
    }
    public IEnumerator Fall()
    {
        fall = true;
        //anim.enabled = true;
        edge.RemoveNode(this);
        transform.parent.GetComponent<Collider>().enabled = false;
        rb.isKinematic = false;
        rb.AddForceAtPosition(new Vector3(0, -1, 0), new Vector3(0f, 0f, 0f), ForceMode.Impulse);
        rb.useGravity = true;
        //anim.SetTrigger("Fall");
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }
    private void Animate()
    {
        Vector3 pos = transform.position;
        pos.y = AnimationSinFunction(animationTime);
        transform.position = pos;
        animationTime = (animationTime + Time.deltaTime) % (animationSpeed * Mathf.PI / 5);
    }

    private float AnimationSinFunction(float x)
    {
        return Mathf.Sin((10 / animationSpeed) * x + Mathf.PI / 2) / 20 - 0.25f;
    }
}
