using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public bool StartBreakable = false;
    public bool Breakable = false;

    private void Start()
    {
        if (StartBreakable)
            StartCoroutine(DelayedSetBreakable());
    }

    IEnumerator DelayedSetBreakable()
    {
        // yield return null;
        // yield return null;

        yield return new WaitForSeconds(0.4f);

        Breakable = true;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (Breakable)
        {
            foreach (var contact in collision.contacts)
            {
                GetComponent<Rigidbody>().AddForce(contact.normal * 200 + Random.onUnitSphere * 150);
            }

            if (collision.GetContact(0).otherCollider.gameObject.CompareTag("Ground"))
            {
                print("Collision! ");

                Vector3 vel = GetComponent<Rigidbody>().velocity;

                Vector3 spawnPos = transform.root.Find("Object").GetChild(0).Find("SpawnPosition").position;
                transform.root.Find("Object").gameObject.SetActive(false);
                transform.root.Find("BrokenObject").position = spawnPos;
                transform.root.Find("BrokenObject").gameObject.SetActive(true);

                foreach (Transform tr in transform.root.Find("BrokenObject"))
                {
                    tr.GetComponent<Rigidbody>().velocity = vel;
                    GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * 1500);
                }
            }
        }
    }
}
