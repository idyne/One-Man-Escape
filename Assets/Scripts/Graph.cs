using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    private List<Edge> edges;
    private List<Vertice> vertices;
    [SerializeField] private GameObject collectiblePrefab, linkPrefab;
    public Queue<Vector3> path;
    private bool isPathLinked = false;
    public List<Link> links;


    private void Awake()
    {
        edges = new List<Edge>(FindObjectsOfType<Edge>());
        vertices = new List<Vertice>(FindObjectsOfType<Vertice>());
        path = new Queue<Vector3>();
        links = new List<Link>();
    }

    private void Start()
    {
        SetAdjacencies();
        //PlaceCollectibles();
    }

    private void Update()
    {
        /*if(GameManager.GAME_STATE == GameState.WIN && !isPathLinked)
        {
            LinkPath();
        }*/
        
    }
    public Edge FindEdge(Vertice v1, Vertice v2)
    {
        Edge result = null;
        foreach (Edge edge in edges)
        {
            if (edge.Between(v1, v2))
            {
                result = edge;
                break;
            }
        }
        return result;
    }

    private void SetAdjacencies()
    {
        foreach (Edge edge in edges)
        {
            Vertice[] vertices = edge.GetVertices();
            vertices[0].AddAdjacency(vertices[1]);
        }
    }

    public void PlaceCollectibles()
    {
        foreach (Edge edge in edges)
        {
            EdgeNode[] nodes = edge.GetNodes();
            EdgeNode node = nodes[Random.Range(0, nodes.Length)];
            Vector3 pos = node.transform.position;
            pos.y = 0.4f;
            Instantiate(collectiblePrefab, pos, collectiblePrefab.transform.rotation);
        }
    }

    public void RemoveEdge(Edge edge)
    {
        Vertice[] vertices = edge.GetVertices();
        vertices[0].RemoveAdjacency(vertices[1]);
        edges.Remove(edge);
    }

    public bool Clean()
    {
        return edges.Count == 0;
    }

    public void SetVerticeColors(Vertice currentVertice)
    {
        foreach (Vertice vertice in vertices)
        {
            if (currentVertice == vertice)
                vertice.ChangeColor(new Color(230/255f, 230/255f, 137/255f));
            else if (currentVertice.CheckAdjacencyWith(vertice))
                vertice.ChangeColor(new Color(119/255f, 221/255f, 119/255f));
            else
                vertice.ChangeColor(new Color(255/255f, 105/255f, 97/255f));
        }
    }

    /*public void LinkPath()
    {
        Vector3 startPoint = path.Dequeue();
        Vector3 endPoint = path.Dequeue();
        StartCoroutine(CreateLink(startPoint, endPoint));
        isPathLinked = true;
    }*/

    public IEnumerator CreateLink(Transform startPoint, Transform endPoint)
    {
        Link link = Instantiate(linkPrefab, Vector3.zero, linkPrefab.transform.rotation).GetComponent<Link>();
        link.SetPoints(startPoint.position, endPoint.position);
        link.connections.Add(startPoint);
        link.connections.Add(endPoint);
        links.Add(link);
        yield return new WaitUntil(() => link.Finished());
        /*if (path.Count > 0)
            StartCoroutine(CreateLink(endPoint, path.Dequeue()));*/
    }
}
