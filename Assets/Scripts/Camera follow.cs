using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float followSpeed = 2f;
    [SerializeField] public Transform target;
    private Vector3 refVelo;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.SmoothDamp(transform.position, newPos,ref refVelo, followSpeed * Time.deltaTime,20f);
    }
}
