using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private bool isPlayerPlaced = false;
    private static bool canMove = false;
    private bool isPlayerDisappeared = false;
    public static Vertice currentVertice = null;
    private static Graph graph;
    private Player player;
    private Portal portal;
    private Camera cam;
    private bool isLinksDestroyed = false;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject effect;


    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
        cam = Camera.main;
        canMove = false;
    }

    private IEnumerator Disappear()
    {
        isPlayerDisappeared = true;
        yield return new WaitForSeconds(0.5f);
        Vector3 pos = player.transform.position;
        pos.y = 1;
        Instantiate(effect, pos, effect.transform.rotation);
        player.gameObject.SetActive(false);
        player = null;
        currentVertice.ChangeColor(new Color(255 / 255f, 105 / 255f, 97 / 255f));
    }

    private void Update()
    {
        if (GameManager.GAME_STATE == GameState.WIN && !isPlayerDisappeared)
        {
            StartCoroutine(Disappear());
        }
        if (!player || player && !Player.Moving())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Vertice")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        StartCoroutine(SelectVertice(hit));
                    }
                }
            }
        }
        if (GameManager.GAME_STATE == GameState.LOSE && !isLinksDestroyed)
        {
            foreach (Link link in graph.links)
            {
                if (link.connections.Contains(currentVertice.transform))
                    Destroy(link.gameObject);
            }
            Instantiate(effect, currentVertice.transform.position, effect.transform.rotation);
            Destroy(currentVertice.gameObject);
            isLinksDestroyed = true;
        }
    }

    private IEnumerator SelectVertice(RaycastHit hit)
    {
        Vertice selectedVertice = hit.collider.GetComponent<Vertice>();
        if (!isPlayerPlaced)
        {
            GameManager.StartGame();
            currentVertice = selectedVertice;
            PlacePlayer();
            //graph.SetVerticeColors(currentVertice);
            //graph.path.Enqueue(currentVertice.transform.position);
            yield return null;
        }
        else if (currentVertice.CheckAdjacencyWith(selectedVertice))
        {
            Edge edge = graph.FindEdge(currentVertice, selectedVertice);
            EdgeNode[] nodes = edge.GetNodes(currentVertice);
            player.MoveTo(selectedVertice, nodes);
            selectedVertice.animate = false;
            yield return new WaitUntil(() => !Player.Moving());
            selectedVertice.animate = true;
            StartCoroutine(graph.CreateLink(currentVertice.transform, selectedVertice.transform));
            currentVertice = selectedVertice;
            //graph.SetVerticeColors(currentVertice);
            //graph.path.Enqueue(currentVertice.transform.position);

            if (currentVertice.Seperate())
            {
                if (graph.Clean())
                    GameManager.Win();
                else
                    GameManager.Lose();
                    
            }
        }
    }
    private void PlacePlayer()
    {
        Vector3 pos = currentVertice.transform.position;
        pos.y = 0.34f;
        //portal = Instantiate(portalPrefab, pos, portalPrefab.transform.rotation).GetComponent<Portal>();
        player = Instantiate(playerPrefab, pos, Quaternion.identity).GetComponent<Player>();
        pos.y = 1;
        Instantiate(effect, pos, effect.transform.rotation);
        isPlayerPlaced = true;
    }

    public static void SetPlayerPlaced()
    {
        canMove = true;
    }
}
