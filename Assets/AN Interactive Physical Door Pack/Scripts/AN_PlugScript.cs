using UnityEngine;

public class AN_PlugScript : MonoBehaviour
{
    [Tooltip("Feature for one-time use only")]
    public bool OneTime = false;
    [Tooltip("Plug follows this local EmptyObject")]
    public Transform HeroHandsPosition;
    [Tooltip("SocketObject with collider (sphere, box, etc.) (isTrigger = true)")]
    public Collider Socket;
    public AN_DoorScript DoorObject;

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    bool follow = false, isConnected = false, followFlag = false, youCan = true;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Disable physics simulation initially
    }

    void Update()
    {
        if (youCan)
            Interaction();

        // Frozen if it is connected to the Socket
        if (isConnected)
        {
            gameObject.transform.position = Socket.transform.position;
            gameObject.transform.rotation = Socket.transform.rotation;
            DoorObject.isOpened = true;
        }
        else
        {
            DoorObject.isOpened = false;
        }
    }

    void Interaction()
    {
        if (Input.GetKey(KeyCode.F) && NearView(3f, 90f) && !follow)
        {
            isConnected = false; // Unfreeze
            follow = true;
            followFlag = false;
            rb.isKinematic = true; // Disable physics simulation
        }

        if (follow)
        {
            if (followFlag)
            {
                distance = Vector3.Distance(transform.position, Camera.main.transform.position);
                if (distance > 3f || Input.GetKeyDown(KeyCode.F))
                {
                    follow = false;
                }
            }

            followFlag = true;
            rb.isKinematic = true; // Disable physics simulation
            gameObject.transform.position = HeroHandsPosition.position;
            gameObject.transform.rotation = HeroHandsPosition.rotation;
        }
    }

    bool NearView(float maxDistance, float maxAngle)
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogWarning("Player object not found with tag 'Player'. Make sure the player object has the correct tag assigned.");
            return false;
        }

        distance = Vector3.Distance(transform.position, playerObject.transform.position);
        direction = transform.position - playerObject.transform.position;
        angleView = Vector3.Angle(playerObject.transform.forward, direction);

        return (angleView < maxAngle && distance < maxDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == Socket)
        {
            isConnected = true;
            follow = false;
            DoorObject.rbDoor.AddRelativeTorque(new Vector3(0, 0, 20f));
            rb.isKinematic = true; // Disable physics simulation
        }

        if (OneTime)
            youCan = false;
    }
}
