using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public bool isSwitch = false;
    bool isMove = false;

    // カメラ移動
    [SerializeField] Transform target; // 移動先のターゲット位置
    [SerializeField] Transform switchTarget; // Switch後の移動先のターゲット位置
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

            // 位置を補間
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            // 回転を補間
            if (Quaternion.Dot(startRotation, targetRotation) < 0.0f)
            {
                targetRotation = new Quaternion(-targetRotation.x, -targetRotation.y, -targetRotation.z, -targetRotation.w);
            }

            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, fractionOfJourney);

            // ターゲット位置に到達したかどうかをチェック
            if (fractionOfJourney >= 1.0f)
            {
                isMove = false;
            }
        }
    }
}
