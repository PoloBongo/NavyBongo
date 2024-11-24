using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ShopControls : MonoBehaviour
{
    [SerializeField] GameDataSave gameDataSave;
    private GameObject player;
    private PlayerInputAction inputAction;

    private void FoundPlayerBoat()
    {
        BoatController foundBoatController = (BoatController)FindAnyObjectByType(typeof(BoatController));
        player = foundBoatController.gameObject;
        if (!player) Debug.LogError("Player Not Found in " + gameObject.name);
        gameDataSave.SetStockPlayer(player);
        gameDataSave.SetActualPosPlayerBoat(player.transform);
    }
    
    private void FindGameDataSave()
    {
        GameObject gameDataSaveGameObject = GameObject.FindGameObjectWithTag("GameDataSave");
        if (gameDataSaveGameObject)
            gameDataSave = gameDataSaveGameObject.GetComponent<GameDataSave>();
        else
            Debug.LogError("GameDataSave not found!");
    }

    public void Initialize(PlayerInputAction _playerInputAction)
    {
        inputAction = _playerInputAction;
        inputAction.Shop.OpenShop.performed += OpenShop;
    }
    
    private void OpenShop(InputAction.CallbackContext context)
    {
        FindGameDataSave();
        if (gameDataSave.GetPlayerInputAction() != null) gameDataSave.GetPlayerInputAction().Disable();
        FoundPlayerBoat();
        Time.timeScale = 0f;
        SceneManager.LoadSceneAsync("ShopInGame", LoadSceneMode.Additive);
    }
}
