using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    static float RandomForce = 1500;

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
                GetComponent<Rigidbody>().AddForce(contact.normal * 200 + Random.onUnitSphere * 300);
            }

            if (collision.GetContact(0).otherCollider.gameObject.CompareTag("Ground"))
            {
                Vector3 vel = GetComponent<Rigidbody>().velocity;

                Transform spawnPos = transform.root.Find("Object").GetChild(0).Find("SpawnPosition");
                transform.root.Find("Object").gameObject.SetActive(false);
                transform.root.Find("BrokenObject").position = spawnPos.position;
                transform.root.Find("BrokenObject").rotation = spawnPos.rotation;
                transform.root.Find("BrokenObject").gameObject.SetActive(true);

                foreach (Transform tr in transform.root.Find("BrokenObject"))
                {
                    tr.GetComponent<Rigidbody>().velocity = vel;
                    GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * RandomForce);
                    RandomForce += 100;
                }
            }
        }
    }
}
