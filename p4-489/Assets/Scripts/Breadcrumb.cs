using UnityEngine;

public class Breadcrumb : MonoBehaviour
{
    private float lifetime;

    public void SetLifetime(float time)
    {
        lifetime = time;
    }

    public bool IsExpired()
    {
        lifetime -= Time.deltaTime;
        return lifetime <= 0;
    }
}
