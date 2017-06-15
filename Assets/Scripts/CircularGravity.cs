using UnityEngine;

public class CircularGravity : MonoBehaviour
{

    public float PullRadius;
    public float GravitationalPull;
    public float MinRadius;
    public float DIstanceMultiplier;

    public LayerMask LayersToPull;

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
                Gizmos.DrawSphere(transform.position, MinRadius);

    }
    // Update is called once per frame
    void FixedUpdate()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, PullRadius, LayersToPull);
        foreach (Collider collider in colliders)
        {

                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb == null) continue;
                Debug.Log(rb);

                Vector3 direction = transform.position - collider.transform.position;

                if (direction.magnitude < MinRadius) continue;

                float distance = direction.sqrMagnitude * DIstanceMultiplier + 1;
                Debug.Log(distance);
                rb.AddForce(direction.normalized * (GravitationalPull / distance) * rb.mass * Time.fixedDeltaTime);
    

        }


    }
}