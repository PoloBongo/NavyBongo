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
        if (!rb) GetRigidbody();
        rb.isKinematic = false;
        sliderReload.gameObject.SetActive(true);
        StopCoroutineResetReloadCoroutine();
        StopCoroutineReloadCoroutine();
        RestoreSliderValue();
        canFire = !(sliderReload.value < 1);
        surchauffeCannon = !(sliderReload.value < 1);
        cancelReloadCannon = false;
        ResetReloadSliderFunc();
    }
    
    private void OnDisable()
    {
        if (!rb) GetRigidbody();
        rb.isKinematic = true;
        SaveSliderValue();
        StopCoroutineResetReloadCoroutine();
        StopCoroutineReloadCoroutine();
        canFire = false;
        surchauffeCannon = false;
        cancelReloadCannon = false;
        unReloadCoroutine = null;
        if (sliderReload) sliderReload.gameObject.SetActive(false);
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
        if (reloadCoroutine == null)
        {
            reloadCoroutine = StartCoroutine(ReloadSlider()); 
        }

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
        ResetReloadSliderFunc();
        StopCoroutineReloadCoroutine();
    }
    
    private void StopCoroutineReloadCoroutine()
    {
        if (reloadCoroutine == null) return;
        StopCoroutine(reloadCoroutine);
        reloadCoroutine = null;
    }

    private void ResetReloadSliderFunc()
    {
        if (resetReloadCoroutine == null && sliderReload.value < 1 && unReloadCoroutine == null)
        {
            resetReloadCoroutine = StartCoroutine(ResetReloadSlider());
        }
    }

    private IEnumerator ReloadSlider()
    {
        while (true && !surchauffeCannon)
        {
            sliderReload.value += reloadSpeed * Time.deltaTime; 
            sliderReload.value = Mathf.Clamp(sliderReload.value, 0, 1);
            if (Mathf.Approximately(sliderReload.value, 1))
            {
                canFire = true;
                break;
            }
            yield return null;
        }
    }
    
    private IEnumerator ResetReloadSlider()
    {
        cancelReloadCannon = true;
        while (cancelReloadCannon)
        {
            sliderReload.value -= reloadSpeed * Time.deltaTime; 
            sliderReload.value = Mathf.Clamp(sliderReload.value, 0, 1);
            if (Mathf.Approximately(sliderReload.value, 0))
            {
                canFire = false;
                surchauffeCannon = false;
                cancelReloadCannon = false;
                break;
            };
            yield return null;
        }
    }

    private void FireCannon()
    {
        if (!canFire) return;
        GameObject cannonBallCopy = Instantiate(cannonBall, shotPos.position, shotPos.rotation) as GameObject;
        Rigidbody cannonBallRb = cannonBallCopy.GetComponent<Rigidbody>();
        cannonBallRb.AddForce(shotPos.forward * firePower, ForceMode.Impulse);
        canFire = false;
        fireSound.Play();
        unReloadCoroutine = StartCoroutine(ResetSlider());
        
        gameDataSave.AddTotalCannonFire();
    }
    
    private IEnumerator ResetSlider()
    {
        surchauffeCannon = true;
        while (surchauffeCannon)
        {
            sliderReload.value -= reloadSpeed * Time.deltaTime; 
            sliderReload.value = Mathf.Clamp(sliderReload.value, 0, 1);
            if (Mathf.Approximately(sliderReload.value, 0))
            {
                surchauffeCannon = false;
                unReloadCoroutine = null;
                break;
            }
            yield return null;
        }
    }
    
    private void FindGameDataSave()
    {
        GameObject gameDataSaveGameObject = GameObject.FindGameObjectWithTag("GameDataSave");
        if (gameDataSaveGameObject != null)
            gameDataSave = gameDataSaveGameObject.GetComponent<GameDataSave>();
        else
            Debug.LogError("GameDataSave not found!");
    }
}
