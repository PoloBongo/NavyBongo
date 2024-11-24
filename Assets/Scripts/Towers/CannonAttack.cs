using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CannonAttack : MonoBehaviour
{
    private CannonController cannonController;
    private bool canAttack;
    private GameObject player;

    [SerializeField] private GameObject cannon;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPos;
    [SerializeField] private float speedRota = 10f;
    [SerializeField] private float cooldownAck = 5f;
    [SerializeField] private LayerMask visibilityLayerMask;

    private float timeSinceLastAttack = 0f;
    private bool isVisible = false;

    public void Initialize()
    {
        cannonController = this.GetComponent<CannonController>();
        if (!cannonController) Debug.LogError("CannonController cannot be null");
        canAttack = cannonController.GetCanRisposte();
        FindPlayerBoat();
    }
    
    private void OnEnable()
    {
        InstantiateBoat.OnUpdateTarget += HandleInitFoundNewPlayer;
    }

    private void OnDisable()
    {
        InstantiateBoat.OnUpdateTarget -= HandleInitFoundNewPlayer;
    }

    private void HandleInitFoundNewPlayer(GameObject _player)
    {
        player = _player;
    }

    public void UpdateCheckRiposte(bool _value)
    {
        canAttack = _value;
    }

    private void FindPlayerBoat()
    {
        GameObject playerBoat = GameObject.FindGameObjectWithTag("Player");
        if (playerBoat != null)
            player = playerBoat;
        else
            Debug.LogError("Player boat not found!");
    }

    private void Update()
    {
        if (!player || !canAttack) return;
        
        bool isAligned = RotateCannonTowardsPlayer();

        if (isVisible)
        {
            if (isAligned)
            {
                timeSinceLastAttack += Time.deltaTime;
                if (timeSinceLastAttack >= cooldownAck)
                {
                    Shoot();
                    timeSinceLastAttack = 0f;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!player) return;
        
        if (PlayerIsVisible())
        {
            isVisible = true;
        }
        else
        {
            isVisible = false;
        }
    }

    private bool RotateCannonTowardsPlayer()
    {
        Vector3 directionToPlayer = player.transform.position - cannon.transform.position;
        directionToPlayer.y = 0;

        Vector3 correctedDirection = -directionToPlayer;
        Quaternion targetRotation = Quaternion.LookRotation(correctedDirection);

        // on fait tourner le cannon vers le player
        cannon.transform.rotation = Quaternion.RotateTowards(
            cannon.transform.rotation,
            targetRotation,
            speedRota * Time.deltaTime
        );

        // check si le cannon est bien aligné au player
        return Quaternion.Angle(cannon.transform.rotation, targetRotation) < 1f;
    }

    // Tire une balle depuis le canon
    private void Shoot()
    {
        if (bulletPrefab && shootPos && player)
        {
            GameObject cannonBallCopy = Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);
            Rigidbody cannonBallRb = cannonBallCopy.GetComponent<Rigidbody>();

            Vector3 directionToPlayer = (player.transform.position - shootPos.position).normalized;

            // effet de random pour pas que ça touche tout le temps
            float randomFactor = Random.Range(0f, 0.2f);
            directionToPlayer += new Vector3(0, randomFactor, 0);
            directionToPlayer.Normalize();

            float distanceToPlayer = Vector3.Distance(player.transform.position, shootPos.position);

            // on ajuste la puissance selon la distance du player
            float firePower = distanceToPlayer * 2f;
            // on fix une limite de puissance pour le côté un peu réaliste
            firePower = Mathf.Clamp(firePower, 10f, 150f);
            
            cannonBallRb.AddForce(directionToPlayer * firePower, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("bulletPrefab, shootPos or player are null !");
        }
    }
    
    private bool PlayerIsVisible()
    {
        if (!player) return false;

        Vector3 directionToPlayer = player.transform.position - shootPos.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (Physics.Raycast(shootPos.position, directionToPlayer.normalized, out RaycastHit hit, distanceToPlayer, visibilityLayerMask))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                return false;
            }
        }

        return true;
    }
}