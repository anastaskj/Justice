using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricObjectDisplay : MonoBehaviour
{
    [SerializeField] private float m_floorHeight;
    private float m_spriteLowerBound;
    private float m_spriteHalfWidth;
    private readonly float m_tan30 = Mathf.Tan(Mathf.PI / 5);

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteLowerBound = spriteRenderer.bounds.size.y * 0.5f;
        m_spriteHalfWidth = spriteRenderer.bounds.size.x * 0.5f;
    }

    // Use this condition for objects that don�t move in the scene.
#if UNITY_EDITOR
    void LateUpdate()
    {
            // Update the position in the Z axis:
            transform.position = new Vector3
                 (
                     transform.position.x,
                     transform.position.y,
                     (transform.position.y - m_spriteLowerBound + m_floorHeight) * m_tan30
                 );
    }
#endif

    void OnDrawGizmos()
    {
        Vector3 floorHeightPos = new Vector3
                (
                    transform.position.x,
                    transform.position.y - m_spriteLowerBound + m_floorHeight,
                    transform.position.z
                );

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(floorHeightPos + Vector3.left * m_spriteHalfWidth, floorHeightPos + Vector3.right * m_spriteHalfWidth);
    }

}
