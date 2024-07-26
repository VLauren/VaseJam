using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainChar : MonoBehaviour
{
    public static MainChar Instance { get; private set; }

    public float MovementSpeed;
    public float RotationSpeed = 360;

    public GameObject PhysicsVasePrefab;

    bool CanControl = true;
    Vector3 moveInput;
    Vector3 controlMovement;

    protected Quaternion TargetRotation;

    protected Animator Animator;

    float balance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Walk();
        Rotation();

        GetComponent<CharacterController>().Move(controlMovement);

        var Vase = transform.Find("Vase");
        if(Vase != null)
        {
            Vase.localEulerAngles = new Vector3(0, 0, Vase.localEulerAngles.z - Time.deltaTime * 10 * balance);
            float vaseAngle = Vector3.AngleBetween(Vector3.up, Vase.up) * Mathf.Rad2Deg;

            if (vaseAngle > 35)
            {
                Instantiate(PhysicsVasePrefab, Vase.Find("VaseModel").position, Vase.Find("VaseModel").rotation).transform.localScale = Vase.Find("VaseModel").localScale;
                Destroy(Vase.gameObject);
            }
        }
    }

    void Walk()
    {
        if (!CanControl) return;

        if (moveInput.magnitude < 0.1f) moveInput = Vector3.zero;

        controlMovement = Vector3.zero;
        controlMovement = moveInput * Time.deltaTime * MovementSpeed;

        if (Animator != null)
        {
            float newMS = Mathf.MoveTowards(Animator.GetFloat("MovementSpeed"), moveInput.magnitude, Time.deltaTime * 10);
            Animator.SetFloat("MovementSpeed", newMS);
        }
    }
    void Rotation()
    {
        if (!CanControl) return;

        float rCam = Camera.main.transform.eulerAngles.y;
        // Direccion relativa a camara
        controlMovement = Quaternion.Euler(0, rCam, 0) * controlMovement;
        if (controlMovement != Vector3.zero)
        {
            TargetRotation = Quaternion.LookRotation(controlMovement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, Time.deltaTime * RotationSpeed);
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 raw = value.Get<Vector2>();
        moveInput = Vector2.zero;
        if (!CanControl) return;
        moveInput = new Vector3(raw.x, 0, raw.y);
    }

    public void OnBalance(InputValue value)
    {
        balance = value.Get<float>();
    }

}

