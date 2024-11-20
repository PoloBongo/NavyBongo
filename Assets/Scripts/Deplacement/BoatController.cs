using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatController : MonoBehaviour
{
    private PlayerInputAction inputAction;
    private Rigidbody rb;

    [Header("Boat Movement Settings")]
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float backwardSpeed = 5f;
    [SerializeField] private float turnSpeed = 50f;
    [SerializeField] private float waterDrag = 0.99f;

    [Header("Particles and Sound Settings")]
    [SerializeField] private ParticleSystem boatParticlesSpeed;
    [SerializeField] private AudioSource boatSpeedSound;
    private Coroutine boatParticlesSpeedRef;

    private float forwardInput = 0f;
    private float turnInput = 0f;
    private bool antiResetSpeedSound = false;

    private bool antiBongo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = waterDrag;
        if (boatParticlesSpeed) boatParticlesSpeed.Stop();
    }

    public void Initialize(PlayerInputAction _playerInputAction)
    {
        inputAction = _playerInputAction;
        inputAction.Movement.Move.performed += OnMovePerformed;
        inputAction.Movement.Move.canceled += OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (this == null) return;
        Vector2 input = context.ReadValue<Vector2>();
        forwardInput = input.y;
        turnInput = input.x;

        if (forwardInput < 0)
        {
            if (antiResetSpeedSound) return;
            antiResetSpeedSound = true;
            if (this != null)
                boatParticlesSpeedRef = StartCoroutine(StartParticlesSpeedAndSound());
        }
        else
        {
            if (!antiResetSpeedSound) return;
            antiResetSpeedSound = false;
            StopParticlesSpeedAndSound();
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (this == null) return;
        forwardInput = 0f;
        turnInput = 0f;
        StopParticlesSpeedAndSound();
    }

    private void FixedUpdate()
    {
        // Avancer & reculer avec le bateau
        if (Mathf.Abs(forwardInput) > 0.1f)
        {
            Vector3 forwardForce = transform.forward * ((forwardInput < 0 ? forwardSpeed : backwardSpeed) * forwardInput);
            rb.AddForce(forwardForce, ForceMode.Force);
        }
        else
        {
            antiResetSpeedSound = false;
        }

        // Tourner le bateau (ça marche en même temps que pour l'avancer donc on peut faire les deux en même temps)
        if (!(Mathf.Abs(turnInput) > 0.1f)) return;
        float turnAngle = turnInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAngle, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    private IEnumerator StartParticlesSpeedAndSound()
    {
        yield return new WaitForSeconds(1.5f);
        if (boatParticlesSpeed) boatParticlesSpeed.Play();
        boatSpeedSound.Play();
        boatParticlesSpeedRef = null;
    }

    private void StopParticlesSpeedAndSound()
    {
        if (boatParticlesSpeed != null && boatParticlesSpeed.isPlaying) 
        {
            boatParticlesSpeed.Stop();
        }

        if (boatSpeedSound != null && boatSpeedSound.isPlaying) 
        {
            boatSpeedSound.Stop();
        }

        if (boatParticlesSpeedRef != null) 
        {
            StopCoroutine(boatParticlesSpeedRef);
            boatParticlesSpeedRef = null;
        }
    }
}
