using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertice : MonoBehaviour
{
    private List<Vertice> adjacents;
    private float animationTime = 0;
    private float animationSpeed = 4f;
    public bool animate = true;

    private Renderer rend;

    private void Awake()
    {
        adjacents = new List<Vertice>();
        animationSpeed = Random.Range(3f, 5f);
        rend = GetComponent<Renderer>();
        animate = true;
    }

    private void Update()
    {
        if ((Controller.currentVertice != this || Player.Moving()) && animate)
        {
            //Animate();
        }
    }
    public bool CheckAdjacencyWith(Vertice vertice)
    {
        bool result = false;
        foreach (Vertice adjacent in adjacents)
        {
            if (adjacent == vertice)
            {
                result = true;
                break;
            }
        }
        return result;
    }

    public void AddAdjacency(Vertice vertice)
    {
        if (!adjacents.Contains(vertice))
        {
            adjacents.Add(vertice);
            vertice.AddAdjacency(this);
        }
    }

    public void RemoveAdjacency(Vertice vertice)
    {
        if (adjacents.Remove(vertice))
            vertice.RemoveAdjacency(this);
    }

    public bool Seperate()
    {
        return adjacents.Count == 0;
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

    public void ChangeColor(Color color)
    {
        rend.materials[3].SetColor("_BaseColor", color);
        //rend.materials[5].color = color;
    }

   
}
