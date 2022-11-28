using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject map;
    public GameObject[] players;
        
    [SerializeField] [Range(0, 10)]   private float moveSpeed = 2;
    [SerializeField] [Range(0, 10)]   private float zoomSpeed = 1.5f;
    [SerializeField] [Range(-10, 10)] private float zoomOffset = 1;
    [SerializeField] [Range(0, 10)]   private float zoomScale = 2.8f;

    [SerializeField] [Range(0, 10)]   private float minOrthograpic = 4;
    [SerializeField] [Range(0, 100)]  private float maxOrthograpic = 15;

    private float xMin, xMax, yMin, yMax;
    private float camSize, camRatio;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        players = new GameObject[4];

        BoxCollider mapBounds = map.GetComponent<BoxCollider>();
        cam = GetComponent<Camera>();

        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        yMin = mapBounds.bounds.min.y;
        yMax = mapBounds.bounds.max.y;
    }

    void Update()
    {
        if(players[0] != null)
            MoveCameraOrthographic(players);
    }

    public void AddCameraTracking(GameObject player)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                players[i] = player;
                Debug.Log("Added " + player.name + " to camera tracking");
                return;
            }
        }
        Debug.Log("Tracking list full! Couldn't add " + player.name + " to camera tracking");
    }

    public void RemoveCameraTracking(GameObject player)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == player)
            {
                players[i] = null;
                Debug.Log("Removed " + player.name + " from camera tracking");
            }
        }
        Debug.Log("Can't Remove " + player.name + " in camera tracking list, does not exist in tracking");
    }


    void MoveCameraOrthographic(GameObject[] players)
    {

        //Set Size
        float currentSize = cam.orthographicSize;
        float targetSize = zoomOffset + GetCameraTargetSize(players);
        float increment = (targetSize - currentSize) * Time.deltaTime * zoomSpeed;

        cam.orthographicSize = Mathf.Clamp(currentSize + increment, minOrthograpic, maxOrthograpic);

        //Move camera
        Vector3 middle = GetCameraDestination(players);
        middle.z = 0;
        transform.position += middle * Time.deltaTime * moveSpeed;

        //transform.position = ClampCamera();
    }

    //Doesn't work with multiple trackers
    Vector3 ClampCamera()
    {
        camSize = cam.orthographicSize;
        camRatio = (xMax + camSize) / 2.0f;

        float camX = Mathf.Clamp(transform.position.x, xMin + camRatio, xMax - camRatio);
        float camY = Mathf.Clamp(transform.position.y, yMin + camSize, yMax - camSize);
        return new Vector3(camX, camY, -10);
    }

    float GetCameraTargetSize(GameObject[] players)
    {
        float aspect = GetComponent<Camera>().aspect;
        List<Vector3> playerPositions = new List<Vector3>();

        //Scale distance in proportions to the camera aspect
        for (int i = 0; i < players.Length; i++)
        {
            if(players[i] != null)
                playerPositions.Add(new Vector3(players[i].transform.position.x, players[i].transform.position.y * aspect));
        }

        float maxDistance = minOrthograpic;
        //Calculate the max distance of all player pairs
        foreach (Vector3 position in playerPositions)
        {
            for (int i = 1; i < playerPositions.Count; i++)
            {
                float distance = Vector2.Distance(position, playerPositions[i]);
                if (maxDistance < distance)
                {
                    maxDistance = distance;
                }
            }
        }

        return maxDistance/zoomScale;
    }

    Vector3 GetCameraDestination(GameObject[] players)
    {
        Vector3 destination = Vector2.zero;
        foreach (GameObject player in players)
        {
            if(player != null)
                destination += (player.transform.position - transform.position);
        }

        destination = destination / players.Length;

        return destination;
    }

    void MoveCameraPerspective(Vector3 player, Vector3 cube)
    {
        Vector3 middle = ((player - transform.position) + (cube - transform.position)) / 2;

        transform.position += middle * Time.deltaTime * moveSpeed;
        transform.position = Vector3.Lerp(transform.position, new(transform.position.x, transform.position.y, -(zoomOffset + Vector2.Distance(player, cube))), Time.deltaTime);
    }
}
