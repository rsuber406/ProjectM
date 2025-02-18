using UnityEngine;
using System.Collections;
public class LightningBolt : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int segments;
    [SerializeField] private float jaggedness;
    [SerializeField] private float width;
    
    private LineRenderer lineRenderer;

    
    private Vector3 startPosition;
    private Vector3 endPosition;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.enabled = true;
        lineRenderer= GetComponent<LineRenderer>();
        startPosition = transform.position;
        endPosition = GameManager.GetInstance().GetPlayerPosition();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.enabled = true;
        GenerateLightning();
    }

    // Update is called once per frame
    
    void GenerateLightning()
    {
    Vector3 direction = (endPosition - startPosition).normalized;
    float length = (endPosition - startPosition).magnitude;
    
   Vector3 perpendicularDirection = Vector3.Cross(direction, Vector3.up).normalized;
    Debug.DrawLine(startPosition, endPosition, Color.red, 2f);
    lineRenderer.positionCount = segments + 1;
        for (int i = 0; i < segments; i++)
        {
        float normalizedSegmentLength = (float)i / segments;
        Vector3 point = Vector3.Lerp(startPosition, endPosition, normalizedSegmentLength);
        float offset = (Random.value - 0.5f) * jaggedness * length;
        point += perpendicularDirection *  offset;
        lineRenderer.SetPosition(i, point);
        }
        HitPlayer();
        StartCoroutine(DestroyLightningBolts());
    }

    IEnumerator DestroyLightningBolts()
    {
        yield return new WaitForSeconds(0.02f);
        Destroy(gameObject);
    }

    void HitPlayer()
    {
        RaycastHit hit;
        Vector3 direction = (GameManager.GetInstance().GetPlayerPosition() - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
        }
    }
  
}
