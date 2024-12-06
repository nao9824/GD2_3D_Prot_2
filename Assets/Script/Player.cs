using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //移動
    Rigidbody rb;
    [SerializeField] CameraMove cameraMove;
    Vector3 direction;
    float speed = 5.0f;

    //ジャンプ
    float jumpForce = 20.0f; // ジャンプ力
    [SerializeField] bool isGrounded = true; // プレイヤーが地面に接しているかどうかを判定

    //今いるパネルを取得
    RaycastHit hit;
    Panel panel;
    [SerializeField]private bool isUpsideDown = false; // さかさまの状態を判定するフラグ

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        newvelo.x = Input.GetAxis("Horizontal") * speed;
        if (Input.GetAxis("Jump") != 0 && isGrounded)
        {
            newvelo.y = Input.GetAxis("Jump") * jumpForce;
            isGrounded = false;
        }
        rb.velocity = newvelo;
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
            Debug.Log("さかさまになったよ");
        }
    }

    public void InvertGravity()
    {
        isUpsideDown = !isUpsideDown;
        if (isUpsideDown)
        {
            Physics.gravity *= -1;
            //   rb.mass *= -1;//.AddForce(-Physics.gravity * rb.mass, ForceMode.Acceleration);
            Debug.Log("重力反転: さかさま");
        }
        else
        {
            Physics.gravity *= -1;
            //rb.mass *= -1;//rb.AddForce(Physics.gravity * rb.mass, ForceMode.Acceleration);
            Debug.Log("重力反転: 元に戻った");
        }
    }

    void Jump()
    {
        rb.velocity += Vector3.up * jumpForce;
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            isGrounded = true;
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
