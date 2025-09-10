using UnityEngine;

public interface IInteractable
{
    void OnClicked(GameObject gameObject);
    void OnHoverEnter(GameObject gameObject);
    void OnHoverExit(GameObject gameObject);
    void OnRelease(GameObject gameObject);
}