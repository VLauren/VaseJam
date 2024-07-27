using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainChar : MonoBehaviour
{
    public static MainChar Instance { get; private set; }

    public float MovementSpeed;
    public float MovementSpeedFast;
    public float GravityAccel = -1;
    public float RotationSpeed = 360;

    public GameObject PhysicsVasePrefab;

    public float MaxTilt = 30;

    [Space(10)]
    public float TiltControlStrength = 10;
    public float OscilationStrength;
    public float OscilationFreq;
    public float FallingStrength;

    [Space(10)]
    public bool CanControl = true;

    Vector3 MoveInput;
    Vector3 ControlMovement;
    float BalanceInput;

    protected Quaternion TargetRotation;
    protected float VerticalVelocity;
    protected Animator Animator;
    protected Transform Vase;


    float OscilationVal;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Vase = transform.Find("Vase");
        InvokeRepeating("ChangeOsc", OscilationFreq, OscilationFreq);

        if (transform.Find("Model"))
            Animator = transform.Find("Model").GetComponent<Animator>();
    }

    void ChangeOsc()
    {
        OscilationVal = -1 + Random.value * 2;
    }

    void Update()
    {
        Walk();
        Rotation();
        Gravity();
        VaseLogic();

        var keyboard = Keyboard.current;
        if (keyboard.rKey.wasPressedThisFrame)
        {
            Time.timeScale = 1;
            Game.Instance.currentLevelPoints = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(keyboard.escapeKey.wasPressedThisFrame)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
        if(keyboard.nKey.wasPressedThisFrame)
        {
            Game.Instance.DebugNextLevel();
        }
    }

    IEnumerator VaseFall()
    {
        float vaseAngle = Vector3.SignedAngle(Vector3.up, Vase.up, transform.forward);


        GameObject physVase = Instantiate(PhysicsVasePrefab, Vase.Find("VaseModel").position, Vase.Find("VaseModel").rotation);
        physVase.transform.localScale = Vase.Find("VaseModel").localScale;
        physVase.GetComponentInChildren<BreakableObject>().Breakable = false;

        Destroy(Vase.gameObject);

        physVase.GetComponentInChildren<BreakableObject>().GetComponent<Rigidbody>().AddForce((transform.right * (vaseAngle < 0 ? -1 : 1) + Vector3.up) * 1000);

        CanControl = false;

        MoveInput = Vector3.zero;
        ControlMovement = Vector3.zero;

        if (Animator != null)
            Animator.SetTrigger("Fall");
        // yield return new WaitForSeconds(0.5f);

        Game.Instance.LevelEnd();

        // physVase.GetComponentInChildren<BreakableObject>().Breakable = true;
        yield return null;

        float t = 0;
        while(t < 0.5f)
        {
            t += Time.deltaTime * 0.5f;
            Time.timeScale = 1 - t;
            yield return null;
        }

        while(t > 0f)
        {
            t -= Time.deltaTime * 0.5f;
            Time.timeScale = 1 - t;
            yield return null;
        }
        Time.timeScale = 1;
    }

    public float GetVaseTilt()
    {
        if(Vase != null)
        {
            return Vector3.SignedAngle(Vector3.up, Vase.up, transform.forward) / MaxTilt;
        }
        return 0;
    }

    void Walk()
    {
        if (!CanControl) return;

        if (MoveInput.magnitude < 0.1f) MoveInput = Vector3.zero;

        ControlMovement = Vector3.zero;
        ControlMovement = MoveInput * Time.deltaTime * (Game.Instance.ShouldGoFast() ? MovementSpeedFast : MovementSpeed);

        if (Animator != null)
        {
            float newMS = Mathf.MoveTowards(Animator.GetFloat("MovementSpeed"), MoveInput.magnitude * (Game.Instance.ShouldGoFast() ? 1 : 0.75f), Time.deltaTime * 10);
            Animator.SetFloat("MovementSpeed", newMS);
        }
    }
    void Rotation()
    {
        if (!CanControl) return;

        float rCam = Camera.main.transform.eulerAngles.y;
        // Direccion relativa a camara
        ControlMovement = Quaternion.Euler(0, rCam, 0) * ControlMovement;
        if (ControlMovement != Vector3.zero)
        {
            TargetRotation = Quaternion.LookRotation(ControlMovement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, Time.deltaTime * RotationSpeed);
        }
    }

    void Gravity()
    {
        if (!CanControl) return;

        if (GetComponent<CharacterController>().isGrounded && VerticalVelocity < 0)
            VerticalVelocity = 0;
        VerticalVelocity += GravityAccel * Time.fixedDeltaTime;

        bool grounded = GetComponent<CharacterController>().isGrounded;

        GetComponent<CharacterController>().Move(ControlMovement + new Vector3(0, VerticalVelocity, 0));
    }

    void VaseLogic()
    {
        if(Vase != null)
        {
            // Control de inclinacion
            if (CanControl)
                Vase.localEulerAngles = new Vector3(0, 0, Vase.localEulerAngles.z - Time.deltaTime * TiltControlStrength * BalanceInput);

            // Oscilacion aleatoria
            Vase.localEulerAngles = new Vector3(0, 0, Vase.localEulerAngles.z - Time.deltaTime * OscilationStrength * OscilationVal);

            float vaseAngle = Vector3.SignedAngle(Vector3.up, Vase.up, transform.forward);

            // Caida
            Vase.localEulerAngles = new Vector3(0, 0, Vase.localEulerAngles.z + Time.deltaTime * FallingStrength * vaseAngle);

            vaseAngle = Vector3.Angle(Vector3.up, Vase.up);

            if (vaseAngle > MaxTilt)
            {
                StartCoroutine(VaseFall());
            }
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 raw = value.Get<Vector2>();
        MoveInput = Vector2.zero;
        if (!CanControl) return;
        MoveInput = new Vector3(raw.x, 0, raw.y);
    }

    public void OnBalance(InputValue value)
    {
        BalanceInput = value.Get<float>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<MainChar>() == null)
        {
            if (CanControl)
            {
                StartCoroutine(VaseFall());

                if(other.CompareTag("Pelota"))
                {
                    print("???? pelota");
                    other.GetComponent<Rigidbody>().velocity = transform.forward * 10 + transform.up * 5;
                }
            }
        }
    }

    public void StopChar()
    {
        CanControl = false;
        MoveInput = Vector3.zero;
        ControlMovement = Vector3.zero;
        FallingStrength = 0;
        OscilationStrength = 0;

        Animator.SetFloat("MovementSpeed", 0);

        Game.Instance.RepeatLevel();
    }
}

