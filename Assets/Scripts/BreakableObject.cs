using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    static float RandomForce = 1500;

    public bool StartBreakable = false;
    public bool Breakable = false;

    public int ScoreMultiplier = 100;

    public bool SonidoJarron = true;

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

            if (collision.GetContact(0).otherCollider.gameObject.CompareTag("Ground") || collision.GetContact(0).otherCollider.gameObject.CompareTag("Pelota"))
            {
                Breakable = false;

                Vector3 vel = GetComponent<Rigidbody>().velocity;

                Transform spawnPos = transform.parent.parent.Find("Object").GetChild(0).Find("SpawnPosition");
                transform.parent.parent.Find("Object").gameObject.SetActive(false);
                transform.parent.parent.Find("BrokenObject").position = spawnPos.position;
                transform.parent.parent.Find("BrokenObject").rotation = spawnPos.rotation;
                transform.parent.parent.Find("BrokenObject").gameObject.SetActive(true);

                foreach (Transform tr in transform.parent.parent.Find("BrokenObject"))
                {
                    tr.GetComponent<Rigidbody>().velocity = vel;
                    GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * RandomForce);
                    RandomForce += 100;
                }

                Game.Instance.AddScore((int)(collision.impulse.magnitude * ScoreMultiplier));

                Game.Instance.SpawnFuego(collision.GetContact(0).point, Quaternion.Euler(0, Random.value * 360, 0));

                if(SonidoJarron)
                {
                    int index = Random.Range(1, 7);
                    AudioManager.Play("jarron_rompe" + index, false, 0.35f);
                }
            }
        }
    }
}
