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
            if(obj.TryGetComponent<Panel>(out p))
            {
                panels.Add(p);
            }
        }
        panels.Sort((x,y)=> string.Compare(x.name,y.name));

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

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (rlCount < rlCountMax)
                {
                    rlCount++;
                    arrow.transform.position = new Vector3(panels[rlCount].transform.position.x, arrow.transform.position.y, arrow.transform.position.z);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (rlCount > 0)
                {
                    rlCount--;
                    arrow.transform.position = new Vector3(panels[rlCount].transform.position.x, arrow.transform.position.y, arrow.transform.position.z);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && !panels[rlCount].rotating)
            {
                panels[rlCount].rotateSet(panels[rlCount].transform.rotation * Quaternion.Euler(180, 0, 0)); // X������180�x��]��ǉ�
                panels[rlCount].rotating = true; // ��]�J�n
            }

        }
        else
        {
            arrow.SetActive(false);
        }
    }
}
