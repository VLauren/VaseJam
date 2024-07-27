using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float MovementSpeed;
    public float GravityAccel = -1;
    public float WalkAnimationSpeed = 0.5f;
    // public float RotationSpeed = 360;

    public float WalkDistance = 10;

    public GameObject PhysicsVasePrefab;


    protected Quaternion TargetRotation;
    protected float VerticalVelocity;
    protected Animator Animator;
    protected Transform Vase;

    protected float WalkedDistance = 0;

    private void Start()
    {
        Vase = transform.Find("Vase");

        if (transform.Find("Model"))
            Animator = transform.Find("Model").GetComponent<Animator>();
    }


    void Update()
    {
        Walk();
        Rotation();
        Gravity();
        VaseLogic();
    }

    IEnumerator VaseFall()
    {
        float vaseAngle = Vector3.SignedAngle(Vector3.up, Vase.up, transform.forward);

        GameObject physVase = Instantiate(PhysicsVasePrefab, Vase.Find("VaseModel").position, Vase.Find("VaseModel").rotation);
        physVase.transform.localScale = Vase.Find("VaseModel").localScale;
        physVase.GetComponentInChildren<BreakableObject>().Breakable = false;

        Destroy(Vase.gameObject);

        physVase.GetComponentInChildren<BreakableObject>().GetComponent<Rigidbody>().AddForce((transform.right * (vaseAngle < 0 ? -1 : 1) + Vector3.up) * 1000);

        if (Animator != null)
            Animator.SetTrigger("Fall");

        MovementSpeed = 0;

        yield return null;
    }

    void Walk()
    {
        if (Animator != null)
        {
            float newMS = Mathf.MoveTowards(Animator.GetFloat("MovementSpeed"), MovementSpeed > 0 ? WalkAnimationSpeed : 0, Time.deltaTime * 10);
            Animator.SetFloat("MovementSpeed", newMS);
        }
    }
    void Rotation()
    {
    }

    void Gravity()
    {
        if (GetComponent<CharacterController>().isGrounded && VerticalVelocity < 0)
            VerticalVelocity = 0;
        VerticalVelocity += GravityAccel * Time.fixedDeltaTime;

        bool grounded = GetComponent<CharacterController>().isGrounded;

        GetComponent<CharacterController>().Move(transform.forward * MovementSpeed * Time.deltaTime + new Vector3(0, VerticalVelocity, 0));

        WalkedDistance += MovementSpeed * Time.deltaTime;
        if (WalkedDistance > WalkDistance)
            MovementSpeed = 0;
    }

    void VaseLogic()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != gameObject && Vase != null)
        {
            StartCoroutine(VaseFall());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * WalkDistance);
    }
}
