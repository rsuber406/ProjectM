using UnityEngine;

public class TPCamController : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerObj;
    [SerializeField] Transform playerModel;
    [SerializeField] Rigidbody rb;
    [SerializeField] float rotation;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //orientation.localRotation = playerObj.transform.localRotation;

        Vector3 direction = playerObj.position - new Vector3(transform.position.x, playerObj.position.y, transform.position.z);
        orientation.forward = direction.normalized;

        Vector3 newDir = orientation.forward * Input.GetAxisRaw("Vertical") +
                         orientation.right * Input.GetAxisRaw("Horizontal");

        if (newDir != Vector3.zero)
        {
            playerModel.forward = Vector3.Slerp(playerModel.forward, newDir.normalized, Time.deltaTime * rotation);
        }

    }
}
