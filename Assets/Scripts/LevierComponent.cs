using System;
using UnityEngine;
using UnityEngine.Events;

public class LevierComponent : MonoBehaviour
{
    [SerializeField] private Transform manche;
    
    [SerializeField] private UnityEvent onLeverPulledUp;
    [SerializeField] private UnityEvent onLeverPulledDown;
    
    [SerializeField] private bool uniqueEvent = false;

    [SerializeField] private float mancheRotation;

    private void Awake()
    {
        mancheRotation = manche.localRotation.x;
    }

    private void Update()
    {
        float rotation = manche.localRotation.x;
        if (rotation < 0 && mancheRotation >= 0f)
        {
            this.onLeverPulledDown?.Invoke();
            Debug.Log("Lever pulled down");
        }
        else if (rotation > 0 && mancheRotation < 0f)
        {
            this.onLeverPulledUp?.Invoke();
            Debug.Log("Lever pulled up");
        }
        
        mancheRotation = rotation;
    }
}
