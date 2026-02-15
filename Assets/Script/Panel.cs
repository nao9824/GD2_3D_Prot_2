using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    // 回転と移動の速度
    public float rotationSpeed = 0.1f; // 回転速度
    public float movementSpeed = 0.1f; // 移動速度
    private Quaternion targetRotation; // 目標の回転
    private Vector3 targetPosition; // 目標の位置
    private float rotationTime = 0f; // 回転の経過時間
    private float movementTime = 0f; // 移動の経過時間
    private float duration = 1.0f; // 移動と回転にかける時間
    public bool rotating = false; // 回転中かどうかを判定するフラグ
    public bool isInversion = false; // 反転してるか
    [SerializeField] CameraMove cameraMove;

    // 持つ
    public bool isHave = false;
    Vector3 scale;

    void Start()
    {
        targetRotation = transform.rotation; // 初期の回転を保存
        targetPosition = transform.position; // 初期の位置を保存
        scale = transform.localScale;
    }

    void Update()
    {
        if (rotating)
        {
            rotationTime += Time.deltaTime / duration; // 経過時間を更新
            movementTime += Time.deltaTime / duration;

            // 回転を滑らかに補間
            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, targetRotation, rotationTime);

            // 位置を滑らかに補間
            transform.parent.position = Vector3.Lerp(transform.parent.position, targetPosition, movementTime);

            // 目標の回転と位置に到達したかどうかをチェック
            if (rotationTime >= 1.0f && movementTime >= 1.0f)
            {
                transform.parent.rotation = targetRotation; // 目標の回転を正確に設定
                transform.parent.position = targetPosition; // 目標の位置を正確に設定

                if (cameraMove.isSwitch)
                {
                    // 反転状態を更新
                    isInversion = !isInversion;
                    Transform player = transform.parent.Find("Player");
                    if (player != null)
                    {
                        Player playerScript = player.GetComponent<Player>();
                        playerScript.InvertGravity();
                        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1.24f);
                    }
                }

                rotating = false; // 回転終了
                rotationTime = 0f;
                movementTime = 0f;
            }
        }

        // 持っている場合のスケール変更
        if (isHave)
        {
            transform.localScale = new Vector3(scale.x - 0.1f, scale.y - 0.1f, scale.z - 0.1f);
        }
        else
        {
            transform.localScale = scale;
        }
    }

    // 回転と位置の目標を設定するメソッド
    public void rotateSet(Quaternion rotation, Vector3 position)
    {
        targetRotation = rotation;
        targetPosition = position;
        rotating = true;
        rotationTime = 0f; // 経過時間をリセット
        movementTime = 0f;
    }
}
