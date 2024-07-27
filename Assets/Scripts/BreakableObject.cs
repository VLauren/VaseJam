using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public bool Breakable = true;

    private void OnCollisionEnter(Collision collision)
    {
        if(Breakable)
        {
            print("Collision! ");

            Vector3 spawnPos = transform.root.Find("Object").GetChild(0).Find("SpawnPosition").position;
            transform.root.Find("Object").gameObject.SetActive(false);
            transform.root.Find("BrokenObject").position = spawnPos;
            transform.root.Find("BrokenObject").gameObject.SetActive(true);

            foreach (var tr in transform.root.Find("BrokenObject"))
            {

            }
        }
    }
}
