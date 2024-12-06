using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    //��]
    float rotationSpeed = 100.0f; // ��]���x
    private Quaternion targetRotation; // �ڕW�̉�]
    public bool rotating = false; // ��]�����ǂ����𔻒肷��t���O
    public bool isInversion = false; // ���]���Ă邩
    [SerializeField] CameraMove cameraMove;

    //����
    public bool isHave = false;
    Vector3 scale;

    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.rotation; // �����̉�]��ۑ�
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // ��]���̏ꍇ
        if (rotating)
        {
            transform.parent.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // �ڕW�̉�]�ɓ��B�������ǂ������`�F�b�N
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation; // �ڕW�̉�]�𐳊m�ɐݒ�
                Transform player = transform.Find("Player");

                if (cameraMove.isSwitch)
                {
                    // ���]��Ԃ��X�V
                    isInversion = !isInversion;
                    if (player != null)
                    {
                        Player playerScript = player.GetComponent<Player>();
                        playerScript.InvertGravity();
                    }
                }
                rotating = false; // ��]�I��
            }
        }

        if (isHave)
        {
            transform.localScale = new Vector3(scale.x-0.1f, scale.y - 0.1f, scale.z - 0.1f);
        }
        else
        {
            transform.localScale = scale;
        }
    }

    public void rotateSet(Quaternion rotation)
    {
        targetRotation = rotation;
    }
}
