using UnityEngine;
using UnityEngine.Events;

public class PressurePlateComponent : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;
    
    [SerializeField]
    private Transform plateTransform;
    
    [SerializeField]
    private float minDistance = 0.1f;
    
    [SerializeField]
    private UnityEvent onPlatePressed;
    [SerializeField]
    private UnityEvent onPlateReleased;
    
    [SerializeField]
    private float currentDistance;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currentDistance = Vector3.Distance(targetTransform.position, plateTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        float newDistance = Vector3.Distance(targetTransform.position, plateTransform.position);
        
        // si la nouvelle distance et plus grande que la minDistance et que la distance actuelle est plus petite que la nouvelle distance
        if (newDistance > minDistance && currentDistance < newDistance)
        {
            onPlateReleased.Invoke();
        }
        else if (newDistance < minDistance && currentDistance > newDistance)
        {
            onPlatePressed.Invoke();
        }
        
        currentDistance = newDistance;
    }
}
