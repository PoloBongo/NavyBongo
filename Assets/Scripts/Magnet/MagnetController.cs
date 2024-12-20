using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagnetController : MonoBehaviour
{
    private Camera mainCamera;
    private PlayerInputAction inputAction;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float forceMultiplier = 1f;

    private GameObject targetedObject;
    private Rigidbody targetedRigidbody;
    private Vector2 lastMousePosition;
    private float targetHeight;

    private bool isRightClicking = false;
    private bool isHolding = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void Initialize(PlayerInputAction _playerInputAction)
    {
        inputAction = _playerInputAction;

        inputAction.Magnet.Click.performed += ctx => isRightClicking = true;
        inputAction.Magnet.Click.canceled += ctx => isRightClicking = false;
    }

    private void Update()
    {
        if (isRightClicking && !isHolding)
        {
            TryPickupObject();
        }

        if (!isRightClicking && isHolding)
        {
            ThrowObject();
        }

        lastMousePosition = Mouse.current.position.ReadValue();
    }

    private void FixedUpdate()
    {
        if (isHolding && targetedRigidbody)
        {
            Vector3 targetPosition = targetedRigidbody.position;
            targetPosition.y = targetHeight;
            Vector3 newPosition = Vector3.Lerp(targetedRigidbody.position, targetPosition, Time.fixedDeltaTime * moveSpeed);
            targetedRigidbody.MovePosition(newPosition);
        }
    }

    private void TryPickupObject()
    {
        if (isHolding) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Throwable"))
            {
                targetedObject = hit.collider.gameObject;
                targetedRigidbody = targetedObject.GetComponent<Rigidbody>();

                if (targetedRigidbody)
                {
                    targetedRigidbody.useGravity = false;
                    targetedRigidbody.velocity = Vector3.zero;
                    targetedRigidbody.angularVelocity = Vector3.zero;
                    targetHeight = targetedRigidbody.position.y + 2f;
                    isHolding = true;
                }
            }
        }
    }

    private void ThrowObject()
    {
        if (targetedRigidbody)
        {
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 mouseDelta = currentMousePosition - lastMousePosition;

            float maxMouseDelta = 10f;
            if (mouseDelta.magnitude > maxMouseDelta)
            {
                mouseDelta = mouseDelta.normalized * maxMouseDelta;
            }

            Vector3 throwDirection = mainCamera.transform.right * mouseDelta.x;

            float adjustedForceMultiplier = forceMultiplier * 0.3f;
            float force = Mathf.Clamp(mouseDelta.magnitude * adjustedForceMultiplier, 0, 20f);

            targetedRigidbody.useGravity = true;
            targetedRigidbody.AddForce(throwDirection * force, ForceMode.Impulse);

            targetedObject = null;
            targetedRigidbody = null;
            isHolding = false;
        }
    }
}
