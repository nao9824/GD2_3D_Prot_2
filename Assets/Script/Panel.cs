using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    //回転
    float rotationSpeed = 100.0f; // 回転速度
    private Quaternion targetRotation; // 目標の回転
    public bool rotating = false; // 回転中かどうかを判定するフラグ
    public bool isInversion = false; // 反転してるか

    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.rotation; // 初期の回転を保存
    }

    // Update is called once per frame
    void Update()
    {
        // 回転中の場合
        if (rotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 目標の回転に到達したかどうかをチェック
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation; // 目標の回転を正確に設定
                Transform player = transform.Find("Player");

                // 反転状態を更新
                isInversion = !isInversion;
                if (player != null)
                {
                    Player playerScript = player.GetComponent<Player>();
                    playerScript.InvertGravity();
                }
                rotating = false; // 回転終了
            }
        }
    }

    public void rotateSet(Quaternion rotation)
    {
        targetRotation = rotation;
    }
}
