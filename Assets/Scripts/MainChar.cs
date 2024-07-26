using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainChar : MonoBehaviour
{
    public static MainChar Instance { get; private set; }

    [SerializeField] float MovementSpeed;
    [SerializeField] float RotationSpeed = 360;

    bool CanControl = true;
    Vector3 moveInput;
    Vector3 controlMovement;

    protected Quaternion TargetRotation;

    protected Animator Animator;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Walk();
        Rotation();

        GetComponent<CharacterController>().Move(controlMovement);
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
        print(value.Get<float>());

        var Vase = transform.Find("Vase");
        Vase.localEulerAngles = new Vector3(0, 0, Vase.localEulerAngles.z + Time.deltaTime * 10);
    }

}

