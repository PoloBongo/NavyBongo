using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BoatWaveController : MonoBehaviour
{
    public Transform floater; // Les points de flottabilité (coins du bateau)
    public float waterLevel = 0.0f; // Niveau de l'eau
    public float floatHeight = 2.0f; // Hauteur de flottabilité
    public float bounceDamp = 0.05f; // Amortissement des rebonds
    public Vector3 buoyancyCentreOffset; // Décalage du centre de flottabilité
    public Rigidbody rb; // Le Rigidbody du bateau

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        // Calculer la hauteur de l'eau à la position du floater
        float forceFactor = 1f - ((floater.position.y - waterLevel) / floatHeight);
            
        if (forceFactor > 0f)
        {
            Vector3 uplift = -Physics.gravity * (forceFactor - rb.velocity.y * bounceDamp);
            rb.AddForceAtPosition(uplift, floater.position + buoyancyCentreOffset, ForceMode.Acceleration);
        }
    }
    public WaterSurface waterSurface; // Le composant WaterSurface
    public Transform target; 
    void Update()
    {
        if (waterSurface && target)
        {
            float waveHeight = SampleWaveHeightAtPosition(floater.position);
                
            // Set the floater's y position to match the wave height
            Vector3 newPosition = floater.position;
            newPosition.y = waveHeight;
            floater.position = newPosition;
        }
    }
    
    float SampleWaveHeightAtPosition(Vector3 position) 
    {
        float waveHeight = Mathf.Sin(position.x + Time.time) * Mathf.Cos(position.z + Time.time);
        return waveHeight;
    }
}
