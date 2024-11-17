using UnityEngine;
using UnityEngine.InputSystem;

public class OrientationCanon : MonoBehaviour
{
    private PlayerInputAction inputAction;
    private float elevationDirection = 0f;
    private float rotationDirectionY = 0f;
    [SerializeField] private float elevationSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minElevation = -10f;
    [SerializeField] private float maxElevation = 45f;
    
    [SerializeField] private CannonFire canonFire;

    public void Initialize(PlayerInputAction _playerInputAction)
    {
        inputAction = _playerInputAction;
        inputAction.Cannon.Elevate.performed += OnElevatePerformed;
        inputAction.Cannon.Elevate.canceled += OnElevateCanceled;
        inputAction.Cannon.Rotate.performed += OnRotatePerformed;
        inputAction.Cannon.Rotate.canceled += OnRotateCanceled;
        
        canonFire.Initialize(inputAction);
    }

    private void OnElevatePerformed(InputAction.CallbackContext context)
    {
        elevationDirection = context.ReadValue<float>();
    }
    
    private void OnElevateCanceled(InputAction.CallbackContext context)
    {
        elevationDirection = 0f;
    }

    private void OnRotatePerformed(InputAction.CallbackContext context)
    {
        rotationDirectionY = context.ReadValue<float>();
    }

    private void OnRotateCanceled(InputAction.CallbackContext context)
    {
        rotationDirectionY = 0f;
    }

    private void FixedUpdate()
    {
        // permet de lever/baisser le canon
        if (elevationDirection != 0f)
        {
            float newElevation = transform.localEulerAngles.x + (elevationDirection * elevationSpeed * Time.fixedDeltaTime);
            newElevation = Mathf.Clamp(newElevation, minElevation, maxElevation);
            transform.localEulerAngles = new Vector3(newElevation, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        // permet de rotate le canon
        if (rotationDirectionY == 0f) return;
        Quaternion deltaRotation = Quaternion.Euler(0, rotationDirectionY * rotationSpeed * Time.fixedDeltaTime, 0);
        transform.rotation = deltaRotation * transform.rotation;
    }
}
