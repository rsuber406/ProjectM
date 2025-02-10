using UnityEngine;

public class TPCamController : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerObj;
    [SerializeField] Transform playerModel;
    [SerializeField] Transform crossHair;
    [SerializeField] Rigidbody rb;
    [SerializeField] float sensitivity;

    [SerializeField] GameObject basicCam;
    [SerializeField] GameObject combatCam;
    [SerializeField] GameObject mainCam;

    bool inCombat;

    CamStyle cam;
    enum CamStyle
    {
        Basic
        , Combat
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = CamStyle.Basic;
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateCamera();
        GetCamStyle();
    }

    void GetCamStyle()
    {
        SetCamStyle();

        Vector3 direction = playerObj.position - new Vector3(transform.position.x, playerObj.position.y, transform.position.z);
        orientation.forward = direction.normalized;

        if (cam == CamStyle.Basic)
        {
            basicCam.SetActive(true);

            Vector3 newDir = orientation.forward * Input.GetAxisRaw("Vertical") +
                             orientation.right * Input.GetAxisRaw("Horizontal");

            if (newDir != Vector3.zero)
                playerModel.forward = Vector3.Slerp(playerModel.forward, newDir.normalized, Time.deltaTime * sensitivity);
        }
        else if (cam == CamStyle.Combat)
        {
            combatCam.SetActive(true);

            Vector3 combatDir = crossHair.position - new Vector3(transform.position.x, crossHair.position.y, transform.position.z);

            orientation.forward = combatDir.normalized;
            playerModel.forward = combatDir.normalized;
        }
    }

    void SetCamStyle()
    {
        basicCam.SetActive(false);
        combatCam.SetActive(false);

        if (Input.GetButtonDown("Combat") && !inCombat)
        {
            inCombat = true;
            cam = CamStyle.Combat;
        }
        else if (Input.GetButtonDown("Combat") && inCombat)
        {
            inCombat = false;
            cam = CamStyle.Basic;
        }
    }

    void UpdateCamera()
    {
        if (!inCombat)
        {
            mainCam.transform.position = combatCam.transform.position;
            //currentCam = Vector3.Lerp(combatCam.transform.position, basicCam.transform.position, Time.deltaTime * 10f);
            //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, basicCam.transform.position, Time.deltaTime * 10f);
            combatCam.transform.position = new Vector3(basicCam.transform.position.x, basicCam.transform.position.y, basicCam.transform.position.z);

        }
        else if (inCombat)
        {
            mainCam.transform.position = basicCam.transform.position;
            //currentCam = Vector3.Lerp(basicCam.transform.position, combatCam.transform.position, Time.deltaTime * 10f);
            //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, combatCam.transform.position, Time.deltaTime * 10f);
            basicCam.transform.position = new Vector3(combatCam.transform.position.x, combatCam.transform.position.y, combatCam.transform.position.z);
        }
        //Camera.main.transform.position = new Vector3(currentCam.x, currentCam.y, currentCam.z);
    }
}

