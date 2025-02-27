using UnityEngine;

public class TPCamController : MonoBehaviour
{
    [SerializeField] float sensitivity;

    [SerializeField] Transform orientation;
    [SerializeField] Transform playerObj;
    [SerializeField] Transform playerModel;
    [SerializeField] Transform crossHair;

    [SerializeField] GameObject basicCam;
    [SerializeField] GameObject combatCam;
    
    bool inCombat;

    CamStyle cam;
    enum CamStyle
    {
        Basic,
        Combat
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = CamStyle.Basic;
    }

    // Update is called once per frame
    void Update()
    {
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
            //Camera.main.transform.position = basicCam.transform.position;
            GameManager.GetInstance().SetPlayerIsInCombatMode(false);
            Vector3 newDir = orientation.forward * Input.GetAxisRaw("Vertical") +
                             orientation.right * Input.GetAxisRaw("Horizontal");

            if (newDir != Vector3.zero)
                playerModel.forward = Vector3.Slerp(playerModel.forward, newDir.normalized, Time.deltaTime * sensitivity);
        }
        else if (cam == CamStyle.Combat)
        {
            combatCam.SetActive(true);
            //Camera.main.transform.position = combatCam.transform.position;
            GameManager.GetInstance().SetPlayerIsInCombatMode(true);
            Vector3 combatDir = crossHair.position - new Vector3(transform.position.x, crossHair.position.y, transform.position.z);

            orientation.forward = combatDir.normalized;
            playerModel.forward = combatDir.normalized;
        }
    }

    void SetCamStyle()
    {
        basicCam.SetActive(false);
        combatCam.SetActive(false);

        if (Input.GetButtonDown("Combat") && !inCombat && Time.deltaTime != 0)
        {
            inCombat = true;
            cam = CamStyle.Combat;
        }
        else if (Input.GetButtonDown("Combat") && inCombat && Time.deltaTime != 0)
        {
            inCombat = false;
            cam = CamStyle.Basic;
        }
    }
}

