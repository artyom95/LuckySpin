using UnityEngine;

public class AchieveDetector 
{
    private GameObject _arrow;

    public void Initialize(GameObject arrow)
    {
        _arrow = arrow;
    }


    public Item FindAchieve()
    {
        var sphereCenter = _arrow.transform;
        float sphereRadius = 10f;

        var overlappedColliders = SphereOverlap(sphereCenter, sphereRadius);

        foreach (var collider in overlappedColliders)
        {
            if (collider != null)
            {
                collider.gameObject.TryGetComponent<Item>(out var component);
                return component;
            }
        }

        return null;
    }
        

    private Collider[] SphereOverlap(Transform center, float radius)
    {
             
        var colliders = Physics.OverlapSphere(center.position, radius);
        return colliders;
    }
}