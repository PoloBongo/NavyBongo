using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    private TowerController towerController;
    private bool canAttack;
    private GameObject player;

    [SerializeField] private GameObject cannon;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPos;
    [SerializeField] private float speedRota = 10f;
    [SerializeField] private float cooldownAck = 5f;

    private float timeSinceLastAttack = 0f;

    public void Initialize()
    {
        towerController = this.GetComponent<TowerController>();
        if (!towerController) Debug.LogError("TowerController cannot be null");
        canAttack = towerController.GetCanRisposte();
        FindPlayerBoat();
    }

    public void UpdateCheckRiposte()
    {
        canAttack = towerController.GetCanRisposte();
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
            firePower = Mathf.Clamp(firePower, 10f, 200f);
            
            cannonBallRb.AddForce(directionToPlayer * firePower, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("bulletPrefab, shootPos or player are null !");
        }
    }
}