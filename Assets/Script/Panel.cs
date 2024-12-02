using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    //‰ñ“]
    float rotationSpeed = 100.0f; // ‰ñ“]‘¬“x
    private Quaternion targetRotation; // –Ú•W‚Ì‰ñ“]
    public bool rotating = false; // ‰ñ“]’†‚©‚Ç‚¤‚©‚ğ”»’è‚·‚éƒtƒ‰ƒO
    public bool isInversion = false; // ”½“]‚µ‚Ä‚é‚©
    [SerializeField] CameraMove cameraMove;

    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.rotation; // ‰Šú‚Ì‰ñ“]‚ğ•Û‘¶
    }

    // Update is called once per frame
    void Update()
    {
        // ‰ñ“]’†‚Ìê‡
        if (rotating)
        {
            transform.parent.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // –Ú•W‚Ì‰ñ“]‚É“’B‚µ‚½‚©‚Ç‚¤‚©‚ğƒ`ƒFƒbƒN
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation; // –Ú•W‚Ì‰ñ“]‚ğ³Šm‚Éİ’è
                Transform player = transform.Find("Player");

                if (cameraMove.isSwitch)
                {
                    // ”½“]ó‘Ô‚ğXV
                    isInversion = !isInversion;
                    if (player != null)
                    {
                        Player playerScript = player.GetComponent<Player>();
                        playerScript.InvertGravity();
                    }
                }
                rotating = false; // ‰ñ“]I—¹
            }
        }
    }

    public void rotateSet(Quaternion rotation)
    {
        targetRotation = rotation;
    }
}
