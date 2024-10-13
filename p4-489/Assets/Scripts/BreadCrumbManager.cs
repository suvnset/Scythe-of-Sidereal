using UnityEngine;
using System.Collections.Generic;

public class BreadcrumbManager : MonoBehaviour
{
    public GameObject breadcrumbPrefab;
    public float breadcrumbDropInterval = 1f;  // Time between breadcrumb drops
    public float breadcrumbLifetime = 5f;      // Time until breadcrumbs disappear

    private List<GameObject> breadcrumbs = new List<GameObject>();
    private float breadcrumbTimer = 0f;

    void Update()
    {
        breadcrumbTimer += Time.deltaTime;

        // Drop breadcrumbs at intervals
        if (breadcrumbTimer >= breadcrumbDropInterval)
        {
            DropBreadcrumb();
            breadcrumbTimer = 0f;
        }

        // Remove breadcrumbs after their lifetime has passed
        for (int i = breadcrumbs.Count - 1; i >= 0; i--)
        {
            Breadcrumb breadcrumb = breadcrumbs[i].GetComponent<Breadcrumb>();
            if (breadcrumb.IsExpired())
            {
                Destroy(breadcrumb.gameObject);
                breadcrumbs.RemoveAt(i);
            }
        }
    }

    void DropBreadcrumb()
    {
        // Spawn breadcrumb slightly behind the player
        Vector2 breadcrumbPosition = transform.position - (Vector3)transform.up * 0.5f;
        GameObject breadcrumb = Instantiate(breadcrumbPrefab, breadcrumbPosition, Quaternion.identity);
        breadcrumb.GetComponent<Breadcrumb>().SetLifetime(breadcrumbLifetime);
        breadcrumbs.Add(breadcrumb);
    }


    public GameObject GetLatestBreadcrumb()
    {
        // Return the latest breadcrumb if it exists
        if (breadcrumbs.Count > 0)
        {
            return breadcrumbs[breadcrumbs.Count - 1];
        }

        return null;
    }
}
