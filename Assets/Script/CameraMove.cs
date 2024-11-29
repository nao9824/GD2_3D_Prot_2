using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public bool isSwitch = false;
    bool isMove = false;

    // �J�����ړ�
    [SerializeField] Transform target; // �ړ���̃^�[�Q�b�g�ʒu
    [SerializeField] Transform switchTarget; // Switch��̈ړ���̃^�[�Q�b�g�ʒu
    float speed = 4.0f;
    Vector3 startPosition;
    Quaternion startRotation;
    Vector3 targetPosition;
    Quaternion targetRotation;
    float startTime;
    float journeyLength;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isSwitch)
            {
                isSwitch = true;
                isMove = true;
                targetPosition = switchTarget.position;
                targetRotation = switchTarget.rotation;
                Camera.main.orthographic = false;
            }
            else
            {
                isSwitch = false;
                isMove = true;
                targetPosition = target.position;
                targetRotation = target.rotation;
                Camera.main.orthographic = true;

            }

            startPosition = transform.position;
            startRotation = transform.rotation;
            startTime = Time.time;
            journeyLength = Vector3.Distance(startPosition, targetPosition);
        }

        if (isMove)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;

            // �ʒu����
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            // ��]����
            if (Quaternion.Dot(startRotation, targetRotation) < 0.0f)
            {
                targetRotation = new Quaternion(-targetRotation.x, -targetRotation.y, -targetRotation.z, -targetRotation.w);
            }

            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, fractionOfJourney);

            // �^�[�Q�b�g�ʒu�ɓ��B�������ǂ������`�F�b�N
            if (fractionOfJourney >= 1.0f)
            {
                isMove = false;
            }
        }
    }
}
