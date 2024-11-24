using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CannonFire : MonoBehaviour
{
    public static Dictionary<GameObject, float> sliderValues = new Dictionary<GameObject, float>();
    // map action
    private PlayerInputAction inputAction;

    [Header("Cannon Settings")]
    [SerializeField] private GameObject cannonBall;
    [SerializeField] private Transform shotPos;
    [SerializeField] private float firePower = 10f;
    [SerializeField] private OrientationCanon orientationCanon;
    private Rigidbody rb;

    [Header("Cannon UI")]
    [SerializeField] private Slider sliderReload;
    [SerializeField] private float reloadSpeed = 0.1f;
    private Coroutine reloadCoroutine;
    private Coroutine resetReloadCoroutine;
    private Coroutine unReloadCoroutine;
    
    private bool canFire = false;
    private bool surchauffeCannon = false;
    private bool cancelReloadCannon = false;
    private bool isReloading = false;
    
    // data
    private GameDataSave gameDataSave;

    // Sound
    [Header("Sound Cannon Settings")]
    [SerializeField] private AudioSource fireSound;

    public void Initialize(PlayerInputAction _inputAction)
    {
        inputAction = _inputAction;
        inputAction.Cannon.Fire.performed += OnFirePerformed;
        inputAction.Cannon.Reload.performed += OnReloadPerformed;
        inputAction.Cannon.Reload.canceled += OnReloadCanceled;

        FindGameDataSave();
    }

    private void OnEnable()
    {
        orientationCanon.OnEnableObject();
        if (!rb) GetRigidbody();
        rb.isKinematic = false;
        sliderReload.gameObject.SetActive(true);
        isReloading = gameDataSave.GetReloadingBool(this.gameObject);
        StopCoroutineResetReloadCoroutine();
        StopCoroutineReloadCoroutine();
        RestoreSliderValue();
        canFire = !(sliderReload.value < 1);
        surchauffeCannon = !(sliderReload.value < 1);
        if (sliderReload.value < 1 && !isReloading)
        {
            cancelReloadCannon = true;
            inputAction.Cannon.Reload.performed -= OnReloadPerformed;
            inputAction.Cannon.Reload.canceled -= OnReloadCanceled;
            inputAction.Cannon.Disable();
            unReloadCoroutine = StartCoroutine(ResetSlider());
        }
        else if (isReloading)
        {
            reloadCoroutine = null;
            if (reloadCoroutine != null || surchauffeCannon || cancelReloadCannon)
                return;

            reloadCoroutine = StartCoroutine(ReloadSlider());
            StopCoroutineResetReloadCoroutine();
        }
        else
        {
            inputAction.Cannon.Reload.performed += OnReloadPerformed;
            inputAction.Cannon.Reload.canceled += OnReloadCanceled;
            inputAction.Cannon.Enable();
        }
    }
    
    private void OnDisable()
    {
        orientationCanon.OnDisableObject();
        if (!rb) GetRigidbody();
        rb.isKinematic = true;
        SaveSliderValue();
        StopCoroutineResetReloadCoroutine();
        StopCoroutineReloadCoroutine();
        canFire = false;
        surchauffeCannon = false;
        cancelReloadCannon = false;
        unReloadCoroutine = null;
        reloadCoroutine = null;
        if (sliderReload) sliderReload.gameObject.SetActive(false);
        if (sliderReload.value < 1)
        {
            inputAction.Cannon.Reload.performed -= OnReloadPerformed;
            inputAction.Cannon.Reload.canceled -= OnReloadCanceled;
            inputAction.Cannon.Disable();
        }

        if (sliderReload.value <= 0.5)
        {
            isReloading = true;
        }
        else if (sliderReload.value > 0.5)
        {
            isReloading = false;
        }
        
        gameDataSave.SetReloadingBool(this.gameObject, isReloading);
    }
    
    private void GetRigidbody()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void SaveSliderValue()
    {
        if (sliderValues.ContainsKey(gameObject))
        {
            sliderValues[gameObject] = sliderReload.value;
        }
        else
        {
            sliderValues.Add(gameObject, sliderReload.value);
        }
    }

    private void RestoreSliderValue() 
    {
        if (sliderValues.ContainsKey(gameObject))
        {
            sliderReload.value = sliderValues[gameObject];
        }
        else
        {
            sliderReload.value = 1;
        }
        sliderReload.gameObject.SetActive(true);
    }
    
    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        if (enabled)
        {
            FireCannon();
        }
    }

    private void OnReloadPerformed(InputAction.CallbackContext context)
    {
        if (this == null) return;
        
        if (!Mathf.Approximately(sliderReload.value, 0) || reloadCoroutine != null || surchauffeCannon || cancelReloadCannon)
            return;

        isReloading = true;
        reloadCoroutine = StartCoroutine(ReloadSlider());
        StopCoroutineResetReloadCoroutine();
    }

    private void StopCoroutineResetReloadCoroutine()
    {
        if (resetReloadCoroutine == null) return;
        StopCoroutine(resetReloadCoroutine);
        resetReloadCoroutine = null;
    }
    
    private void OnReloadCanceled(InputAction.CallbackContext context)
    {
        if (this == null) return;
    }
    
    private void StopCoroutineReloadCoroutine()
    {
        if (reloadCoroutine == null) return;
        StopCoroutine(reloadCoroutine);
        reloadCoroutine = null;
    }

    private IEnumerator ReloadSlider()
    {
        isReloading = true;
        while (true)
        {
            sliderReload.value += reloadSpeed * Time.deltaTime; 
            sliderReload.value = Mathf.Clamp(sliderReload.value, 0, 1);
            if (Mathf.Approximately(sliderReload.value, 1))
            {
                canFire = true;
                reloadCoroutine = null;
                break;
            }
            yield return null;
        }
        isReloading = false;
    }

    private void FireCannon()
    {
        if (!canFire)
        {
            canFire = !(sliderReload.value < 1);
        }
        if (!canFire) return;
        GameObject cannonBallCopy = Instantiate(cannonBall, shotPos.position, shotPos.rotation) as GameObject;
        Rigidbody cannonBallRb = cannonBallCopy.GetComponent<Rigidbody>();
        cannonBallRb.AddForce(shotPos.forward * firePower, ForceMode.Impulse);
        canFire = false;
        surchauffeCannon = true;
        fireSound.Play();
        inputAction.Cannon.Reload.Disable();
        unReloadCoroutine = StartCoroutine(ResetSlider());
        
        gameDataSave.AddTotalCannonFire();
    }
    
    private IEnumerator ResetSlider()
    {
        while (surchauffeCannon || cancelReloadCannon)
        {
            sliderReload.value -= reloadSpeed * Time.deltaTime; 
            sliderReload.value = Mathf.Clamp(sliderReload.value, 0, 1);
            if (Mathf.Approximately(sliderReload.value, 0))
            {
                inputAction.Cannon.Reload.performed += OnReloadPerformed;
                inputAction.Cannon.Enable();
                surchauffeCannon = false;
                cancelReloadCannon = false;
                unReloadCoroutine = null;
                break;
            }
            yield return null;
        }
    }
    
    private void FindGameDataSave()
    {
        GameObject gameDataSaveGameObject = GameObject.FindGameObjectWithTag("GameDataSave");
        if (gameDataSaveGameObject)
            gameDataSave = gameDataSaveGameObject.GetComponent<GameDataSave>();
        else
            Debug.LogError("GameDataSave not found!");
    }

    public void SetSliderReload(Slider _sliderReload)
    {
        sliderReload = _sliderReload;
    }

    private void OnDestroy()
    {
        inputAction.Cannon.Fire.performed -= OnFirePerformed;
        StopCoroutineResetReloadCoroutine();
        StopCoroutineReloadCoroutine();
        canFire = false;
        surchauffeCannon = false;
        cancelReloadCannon = false;
        unReloadCoroutine = null;
        if (sliderReload) sliderReload.gameObject.SetActive(false);
    }
}