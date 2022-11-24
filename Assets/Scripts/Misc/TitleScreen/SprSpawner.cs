using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprSpawner : MonoBehaviour
{
    public Sprite[] SpriteItems;
    public GameObject Obj;

    public Vector2 Bounds;
    public float YBound;

    void Start()
    {
        StartCoroutine(SpawnObjs());
    }

    IEnumerator SpawnObjs()
    {
        while (true)
        {
            Vector3 Pos = new Vector3(Random.Range(Bounds.x, Bounds.y), YBound, 200f);
            GameObject New = Instantiate(Obj, Pos - transform.position, Quaternion.identity, transform);
            New.GetComponent<Image>().sprite = SpriteItems[Random.Range(0, SpriteItems.Length)];
            New.GetComponent<Image>().SetNativeSize();
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }
}
