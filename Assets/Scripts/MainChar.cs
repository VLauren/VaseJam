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

    public float MaxTilt = 30;

    bool CanControl = true;
    Vector3 moveInput;
    Vector3 controlMovement;

    protected Quaternion TargetRotation;
    protected Animator Animator;
    protected Transform Vase;

    float balanceInput;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Vase = transform.Find("Vase");
    }

    void Update()
    {
        Walk();
        Rotation();

        GetComponent<CharacterController>().Move(controlMovement);

        if(Vase != null)
        {

            Vase.localEulerAngles = new Vector3(0, 0, Vase.localEulerAngles.z - Time.deltaTime * 10 * balanceInput);
            float vaseAngle = Vector3.Angle(Vector3.up, Vase.up);

            print(vaseAngle + " - " +Time.deltaTime * 10 * balanceInput);

            if (vaseAngle > MaxTilt)
            {
                Instantiate(PhysicsVasePrefab, Vase.Find("VaseModel").position, Vase.Find("VaseModel").rotation).transform.localScale = Vase.Find("VaseModel").localScale;
                Destroy(Vase.gameObject);
            }
        }
    }

    public float GetVaseTilt()
    {
        if(Vase != null)
        {
            print(Vector3.SignedAngle(Vector3.up, Vase.up, transform.forward) + " asd?");
            return Vector3.SignedAngle(Vector3.up, Vase.up, transform.forward) / MaxTilt;
        }
        return 0;
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
        balanceInput = value.Get<float>();
    }

}

