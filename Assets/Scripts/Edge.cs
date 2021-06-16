using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    private List<EdgeNode> nodes; // Ordered from V1 to V2
    private static Graph graph;
    [SerializeField] private Vertice[] vertices;

    private void Awake()
    {
        nodes = new List<EdgeNode>(transform.GetComponentsInChildren<EdgeNode>());
        if (!graph)
            graph = FindObjectOfType<Graph>();
    }

    public bool Between(Vertice v1, Vertice v2)
    {
        bool result = false;
        if (v1 == vertices[0] && v2 == vertices[1] || v1 == vertices[1] && v2 == vertices[0])
            result = true;
        return result;
    }

    public EdgeNode[] GetNodes(Vertice startVertice)
    {
        EdgeNode[] result = nodes.ToArray();
        if (startVertice == vertices[0])
            result = nodes.ToArray();
        else if (startVertice == vertices[1])
        {
            List<EdgeNode> tempNodes = new List<EdgeNode>(nodes);
            tempNodes.Reverse();
            result = tempNodes.ToArray();
        }
        return result;
    }

    public EdgeNode[] GetNodes()
    {
        return nodes.ToArray();
    }

    public Vertice[] GetVertices()
    {
        Vertice[] result = null;
        result = (Vertice[])vertices.Clone();
        return result;
    }

    public bool RemoveNode(EdgeNode node)
    {
        bool result = false;
        result = nodes.Remove(node);
        if (result && nodes.Count == 0)
            graph.RemoveEdge(this);
        return result;
    }
}
