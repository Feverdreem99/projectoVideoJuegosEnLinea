using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform target;

    [Header("Configuración")]
    public float smoothSpeed = 20f; 
    

    public float offsetY = 4f; 

    private float startX;
    private float startZ;

    void Start()
    {
        startX = transform.position.x;
        startZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float targetY = target.position.y + offsetY;
        Vector3 desiredPosition = new Vector3(startX, targetY, startZ);
        
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}