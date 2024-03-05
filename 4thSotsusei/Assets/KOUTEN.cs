using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOUTEN : MonoBehaviour
{
    [Header("���]�̎��ƂȂ�I�u�W�F�N�g")]
    [SerializeField] private GameObject TargetObj;
    [Header("��]������J��������(���)")]
    [SerializeField] private GameObject[] Cameras_Back;
    [Header("��]������J��������(�O)")]
    [SerializeField] private GameObject[] Cameras_Front;
    [Header("��]���x")]
    public float speed = 100f;
    [Header("CameraSwitchSystem���A�^�b�`")]
    [SerializeField]private CameraSwitcingSystem switcingSystem;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (!switcingSystem.Change_CameraRaw)
            {
                Cameras_Back[switcingSystem.currentPos].transform.RotateAround(TargetObj.transform.position, Vector3.up, speed * Time.deltaTime);
            }
            else
            {
                Cameras_Front[switcingSystem.currentPos_Front].transform.RotateAround(TargetObj.transform.position, Vector3.up, speed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (!switcingSystem.Change_CameraRaw)
            {
                Cameras_Back[switcingSystem.currentPos].transform.RotateAround(TargetObj.transform.position, Vector3.up, -speed * Time.deltaTime);
            }
            else
            {
                Cameras_Front[switcingSystem.currentPos_Front].transform.RotateAround(TargetObj.transform.position, Vector3.up, -speed * Time.deltaTime);
            }
        }
    }
}
