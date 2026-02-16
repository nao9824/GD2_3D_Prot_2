using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //移動
    Rigidbody rb;
    [SerializeField] CameraMove cameraMove;
    Vector3 direction;
    float speed = 5.0f;

    //ジャンプ
    float jumpForce = 12.0f; // ジャンプ力
    [SerializeField] bool isGrounded = true; // プレイヤーが地面に接しているかどうかを判定
    [SerializeField] AudioClip jumpSE; // ジャンプのサウンドエフェクト
    private AudioSource audioSource; // AudioSourceコンポーネント

    //今いるパネルを取得
    RaycastHit hit;
    Panel panel;
    [SerializeField] public bool isUpsideDown = false; // さかさまの状態を判定するフラグ

    //リスポーン
    Vector3 startPos;
    Vector3 gravity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isUpsideDown = false;

        startPos = transform.position;

        if (Physics.gravity.y >= 0)
        {
            Physics.gravity *= -1;
        }
        gravity = Physics.gravity;

        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();

        // オーディオクリップが正しく設定されているか確認
        if (jumpSE == null)
        {
            Debug.LogError("JumpSEオーディオクリップが設定されていません！");
        }
        if (audioSource == null)
        {
            Debug.LogError("AudioSourceコンポーネントが見つかりません！");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!cameraMove.isSwitch)
        {
            Move();
            transform.SetParent(null);
            rb.useGravity = true;
        }
        else
        {
            Ray ray = new Ray(transform.position, Vector3.forward);
            Ray ray2 = new Ray(transform.position, Vector3.back);
            if (Physics.Raycast(ray, out hit, 10) || Physics.Raycast(ray2, out hit, 10))
            {
                if (hit.collider.CompareTag("Panel"))
                {
                    transform.SetParent(hit.transform.parent, true);
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    panel = hit.collider.GetComponent<Panel>();

                }
            }
        }
    }

    private void Move()
    {
        Vector3 newvelo = new Vector3(0, rb.velocity.y, 0);

        // 水平方向の移動
        newvelo.x = Input.GetAxis("Horizontal") * speed;

        if (Input.GetAxis("Jump") != 0 && isGrounded)
        {
            // 重力反転に応じてジャンプ力の方向を調整
            newvelo.y = isUpsideDown ? -jumpForce : jumpForce;
            isGrounded = false;

            // ジャンプ時にSEを再生
            Debug.Log("ジャンプサウンド再生");
            audioSource.PlayOneShot(jumpSE);
        }

        rb.velocity = newvelo;

        // 進行方向を向く（重力対応版）
        Vector3 lookDir = new Vector3(rb.velocity.x, 0f, 0f);

        if (lookDir.sqrMagnitude > 0.001f)
        {
            // 重力の逆方向がキャラの上方向
            Vector3 characterUp = -Physics.gravity.normalized;

            Quaternion targetRot = Quaternion.LookRotation(lookDir, characterUp);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, 15f * Time.deltaTime));
        }


        if (newvelo.x != 0)
        {
            Debug.Log(newvelo);
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(newvelo.normalized.x, 0, 0), Vector3.up);
            //rb.rotation = Quaternion.RotateTowards(rb.rotation, toRotation, 720 * Time.deltaTime);
        }

        Ray ray = new Ray(transform.position, Vector3.forward);
        Ray ray2 = new Ray(transform.position, Vector3.back);
        if (Physics.Raycast(ray, out hit, 10) || Physics.Raycast(ray2, out hit, 10))
        {
            if (hit.collider.CompareTag("Panel"))
            {
                panel = hit.collider.GetComponent<Panel>();
            }
        }

        if (panel != null && panel.isInversion)
        {
            // rb.AddForce(-Physics.gravity * rb.mass);
            //Debug.Log("さかさまになったよ");
        }
    }


    public void InvertGravity()
    {
        isUpsideDown = !isUpsideDown;
        if (isUpsideDown)
        {
            Physics.gravity *= -1;
            //   rb.mass *= -1;//.AddForce(-Physics.gravity * rb.mass, ForceMode.Acceleration);
            //Debug.Log("重力反転: さかさま");
        }
        else
        {
            Physics.gravity *= -1;
            //rb.mass *= -1;//rb.AddForce(Physics.gravity * rb.mass, ForceMode.Acceleration);
            //Debug.Log("重力反転: 元に戻った");
        }
    }

    /*void Jump()
    {
        rb.velocity += Vector3.up * jumpForce;
        isGrounded = false;
    }
*/
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Death"))
        {
            transform.position = startPos;
            isUpsideDown = false;
            isGrounded = true;
            Physics.gravity = gravity;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void InvertYRotation()
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion invertedRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y + 180, currentRotation.eulerAngles.z);
        transform.rotation = invertedRotation;
        Debug.Log("プレイヤーが180度反転しました");
    }
}
