using UnityEngine;

public class StartPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject MainCamera;

    private void Start()
    {
        GameObject playerPrefab = Instantiate(player, transform.position, Quaternion.identity);
        GameObject MainCameraPreFab =Instantiate(MainCamera, transform.position, Quaternion.identity);
        MainCameraPreFab.GetComponent<Camerafollow>().target = playerPrefab.transform;
    }

}
