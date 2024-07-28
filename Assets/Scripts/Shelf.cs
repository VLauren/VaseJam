using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public Vector3 FallVelocity;
    public Vector3 FallAngularVelocity;

    bool Falling;
    bool CanFall = false;

    void Start()
    {
        StartCoroutine(DelayedSetBreakable());
    }

    IEnumerator DelayedSetBreakable()
    {
        yield return new WaitForSeconds(0.4f);

        CanFall = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.impulse.sqrMagnitude > 3 && !Falling && CanFall)
        {
            Fall();
        }
    }

    public void Fall()
    {
        Falling = true;

        GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, transform.eulerAngles.y, 0) * FallVelocity;
        GetComponent<Rigidbody>().angularVelocity = Quaternion.Euler(0, transform.eulerAngles.y, 0) * FallAngularVelocity;

    }
}
