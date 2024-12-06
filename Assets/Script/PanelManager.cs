using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] CameraMove cameraMove;
    [SerializeField] int rlCount = 0;
    private List<Panel> panels;
    int rlCountMax = 0;

    float haveTime = 0.0f;
    int haveNum = 0;
    [SerializeField] Vector3 nowPanel1Pos = Vector3.zero;
    [SerializeField] Vector3 nowPanel2Pos = Vector3.zero;

    private Panel holdingPanel;
    private Panel targetPanel;

    // Start is called before the first frame update
    void Start()
    {
        // list�C���X�^���X����
        panels = new List<Panel>();
        // tag�uPanel�v�����I�u�W�F�N�g�����ׂĎ擾
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Panel");
        foreach (GameObject obj in objects)
        {
            Panel p;
            // Panel�R���|�[�l���g���擾�o�����烊�X�g�֒ǉ�
            if (obj.TryGetComponent<Panel>(out p))
            {
                panels.Add(p);
            }
        }
        panels.Sort((x, y) => string.Compare(x.name, y.name));

        rlCountMax = panels.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        Choose();
    }

    void Choose()
    {
        if (cameraMove.isSwitch)
        {
            arrow.SetActive(true);

            // ���E�I��
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (rlCount < rlCountMax)
                {
                    rlCount++;
                    arrow.transform.position = new Vector3(panels[rlCount].transform.position.x, arrow.transform.position.y, arrow.transform.position.z);
                    SwapPanels();
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (rlCount > 0)
                {
                    rlCount--;
                    arrow.transform.position = new Vector3(panels[rlCount].transform.position.x, arrow.transform.position.y, arrow.transform.position.z);
                    SwapPanels();
                }
            }

            // ��]
            if (Input.GetKeyUp(KeyCode.Space) &&
                !panels[rlCount].rotating &&
                !panels[rlCount].isHave &&
                haveTime <= 1.0f)
            {
                panels[rlCount].rotateSet(panels[rlCount].transform.rotation * Quaternion.Euler(180, 0, 0)); // X������180�x��]��ǉ�
                panels[rlCount].rotating = true; // ��]�J�n
            }

            // �����ǂ����̔���
            if (Input.GetKey(KeyCode.Space))
            {
                haveTime += Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.RightArrow) ||
                Input.GetKeyDown(KeyCode.D) ||
                Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.A))
            {
                haveTime = 0.0f;
                panels[rlCount].isHave = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                haveNum = rlCount;
                nowPanel1Pos = panels[haveNum].transform.position;
                holdingPanel = panels[haveNum];
            }

            // ����
            if (Input.GetKey(KeyCode.Space) &&
                haveTime > 1.0f)
            {
                panels[haveNum].isHave = true;

                nowPanel2Pos = panels[rlCount].transform.position;

               /* panels[rlCount].transform.position = nowPanel1Pos;
                panels[haveNum].transform.position = nowPanel2Pos;*/
            }
        }
        else
        {
            arrow.SetActive(false);
        }
    }

    void SwapPanels()
    {
        if (holdingPanel != null && holdingPanel.isHave)
        {
            targetPanel = panels[rlCount];
            Vector3 tempPos = targetPanel.transform.position;
            targetPanel.transform.parent.position = holdingPanel.transform.position;
            holdingPanel.transform.parent.position = tempPos;

            // �p�l���̃��X�g�̃C���f�b�N�X�����ւ���
            int holdingIndex = panels.IndexOf(holdingPanel);
            int targetIndex = panels.IndexOf(targetPanel);
            panels[holdingIndex] = targetPanel;
            panels[targetIndex] = holdingPanel;
        }
    }
}
