using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    [SerializeField] private GameObject effectPrefab;

    private Renderer rend;
    private Collider coll;

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        coll = GetComponentInParent<Collider>();
    }

    private void Start()
    {
        transform.Rotate(new Vector3(0, Random.Range(0, 360f), 0), Space.World);
    }
    private void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * 10, 0), Space.World);
    }

    public IEnumerator GetCollected()
    {
        coll.enabled = false;
        rend.enabled = false;
        Vector3 pos = transform.position;
        pos.y += 0.3f;
        GameObject effect = Instantiate(effectPrefab, pos, effectPrefab.transform.rotation);
        effect.transform.localScale = effect.transform.localScale * 2;
        Destroy(effect, 1f);
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.parent.gameObject);
    }
}
