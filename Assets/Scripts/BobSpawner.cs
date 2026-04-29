using System.Collections;
using UnityEngine;

public class BobSpawner : MonoBehaviour
{
    //Bob settings
    public GameObject Bob;
    public Transform BobSpwan;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SpawnBobs());
    }

    IEnumerator SpawnBobs()
    {
        yield return new WaitForSeconds(7);
        Instantiate(Bob, BobSpwan);
    }
}
