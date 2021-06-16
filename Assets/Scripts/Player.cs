using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private AudioSource audioSource;
    [SerializeField] private Renderer rend;
    [SerializeField] private AudioClip[] sounds;
    private static bool isMoving = false;
    private float animationTime = 0;
    private bool isFalling = false;
    private bool isRotating = false;
    private float rotation = 0;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (GameManager.GAME_STATE == GameState.LOSE && !isFalling)
        {
            anim.SetTrigger("Fall");
            /*Vector3 pos = transform.position;
            pos += transform.forward * 0.5f;
            transform.position = pos;*/
            //transform.Rotate(new Vector3(-45, 0, 0));
            GetComponent<BoxCollider>().enabled = false;
            isFalling = true;
            //isRotating = true;
        }
        if (isRotating)
        {
            rotation -= Time.deltaTime * 10;
            if (rotation <= -45)
                isRotating = false;
            Quaternion _rotation = transform.rotation;
            _rotation.x = rotation;
            transform.rotation = _rotation;

        }
    }

    public void MoveTo(Vertice targetVertice, EdgeNode[] nodes)
    {
        isMoving = true;
        List<Transform> targets = new List<Transform>();
        foreach (EdgeNode node in nodes)
            targets.Add(node.transform);
        targets.Add(targetVertice.transform);
        StartCoroutine(JumpTo(targets, (!GameManager.jump ? 0.3165f : 0.5f)));
    }

    private IEnumerator JumpTo(List<Transform> targets, float t)
    {
        rb.velocity = Vector3.zero;
        Transform target = targets[0];
        Vector3 lookPos = target.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
        Vector3 dif = (target.position - transform.position);
        dif.y = 0;
        Vector3 force = transform.forward * dif.magnitude;
        force *= 1 / t;
        force.y = Physics.gravity.magnitude / (2 / t);
        if (!GameManager.jump)
        {
            if (!anim.GetBool("Run"))
                anim.SetBool("Run", true);
        }
        else
            anim.SetTrigger("Jump");
        rb.AddForce(force, ForceMode.Impulse);
        if (GameManager.jump)
        {
            audioSource.clip = sounds[2];
            audioSource.Play();
        }
        yield return new WaitForSeconds(t);
        if (!GameManager.jump)
        {
            audioSource.clip = sounds[1];
            audioSource.Play();
        }
        EdgeNode node = target.GetComponent<EdgeNode>();
        if (node)
            StartCoroutine(node.Fall());
        targets.RemoveAt(0);
        if (targets.Count > 0)
            StartCoroutine(JumpTo(targets, t));
        else
        {
            isMoving = false;
            if (!GameManager.jump)
                anim.SetBool("Run", false);
            anim.SetTrigger("Idle");
        }
    }

    public static bool Moving()
    {
        return isMoving;
    }

    private void Appear()
    {
        rend.materials[1].SetFloat("_CutoffHeight", AnimationFunction(animationTime));
        animationTime = Mathf.Clamp(animationTime + Time.deltaTime, 0, 1);
    }

    private float AnimationFunction(float x)
    {
        return x * 4f;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Collectible")
        {
            Collectible collectible = other.GetComponentInChildren<Collectible>();
            StartCoroutine(collectible.GetCollected());
        }
    }
}
