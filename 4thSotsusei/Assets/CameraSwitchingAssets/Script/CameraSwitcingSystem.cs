using System;
using System.Collections.Generic;
using UnityEngine;

// �J�����̐؂�ւ�������\�[�X�R�[�h

public class CameraSwitcingSystem : MonoBehaviour
{
    #region --- Enum ---

    /// <summary>
    /// �}�E�X�̃{�^���p�̗񋓌^
    /// </summary>
    private enum MouseButton
    {
        // ���N���b�N
        Left,
        // �E�N���b�N
        Right,
        // �z�C�[��
        Wheel,
    }

    #endregion --- Enum ---

    #region --- Fields ---

    /// <summary>
    /// �f���ʒu�̃��X�g 
    /// </summary>
    [SerializeField]
    private List<ProjectionInfo> projectionPos = new List<ProjectionInfo>();

    [SerializeField]
    private List<ProjectionInfo> projectionPos_Front= new List<ProjectionInfo>();
    [HideInInspector]public bool Change_CameraRaw = false;
    [HideInInspector]public int currentPos_Front = 0;

    /// <summary>
    /// ���C���Ŏg�p����J�����̃I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private GameObject mainCamera;

    /// <summary>
    /// ���C���Ŏg�p����J������Camera�R���|�[�l���g
    /// </summary>
    [SerializeField]
    private Camera cameraCom;

    /// <summary>
    /// ���݂̓��e���Ă���ʒu�̔ԍ�
    /// </summary>
    [HideInInspector] public int currentPos = 0;

    /// <summary>
    /// �^�[�Q�b�g�̕����������Ă������̔���
    /// </summary>
    [SerializeField]
    private bool isLookTarget;

    /// <summary>
    /// �J���������_�ړ����鑬��
    /// </summary>
    [SerializeField]
    private float moveCameraSpeed = 0.1f;

    /// <summary>
    /// �J�����̎���p��ۑ�����ϐ�
    /// </summary>
    private float saveFOV = 0;

    #region --- Const Value ---

    /// <summary>
    /// For���̗v�f���̏����l => 0
    /// </summary>
    private const int FOR_INITIAL_INDEX = 0;

    /// <summary>
    /// projectionPos�̗v�f���̍ŏ��l => 0
    /// </summary>
    private const int MIN_VIEWERPOS_INDEX = 0;

    /// <summary>
    /// projectionPos�̗v�f���̍ő�l�����߂邽�߂̐��l => -1
    /// </summary>
    private const int MAX_VIEWERPOS_INDEX = 1;

    #endregion ---Const Value---

    #endregion --- Fields ---

    #region --- Methods ---

    void Start()
    {
        //cameraCom = mainCamera.GetComponent<Camera>();
        //saveFOV = cameraCom.fieldOfView;

        // ���X�g�̗v�f�����������������鏈��
        for (int index = FOR_INITIAL_INDEX; index < projectionPos.Count; index++)
        {
            // ���X�g�̒��ŏ������e�ʒu�̃g���K�[��True�ł������ꍇ
            if (projectionPos[index].initiProjectionPos)
            {
                // �J�����̓��e�ʒu��ݒ肷��
                ChangePosCamera(index,currentPos_Front,Change_CameraRaw);

                currentPos = index;
            }
        }
    }

    void Update()
    {
        // �L�[�{�[�h����̃J�����؂�ւ��̏���
        InputKeyBoard();

        mainCamera.transform.position = projectionPos[currentPos].projector.transform.position;
        mainCamera.transform.LookAt(projectionPos[currentPos].targetHeadObj);

        if (Change_CameraRaw)
        {
            mainCamera.transform.position = projectionPos_Front[currentPos_Front].projector.transform.position;
            mainCamera.transform.LookAt(projectionPos_Front[currentPos_Front].targetHeadObj);
        }

        // �J�����𓮂�������
        //MoveCamera();
    }

    /// <summary>
    /// �J�����؂�ւ����L�[�{�[�h�ƃ}�E�X�ő��삷��p�̊֐�
    /// </summary>
    private void InputKeyBoard()
    {
        // �J������؂�ւ��邩�𔻒肷��ϐ�
        bool isChange = false;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Change_CameraRaw = !Change_CameraRaw;
        }

        if (!Change_CameraRaw)
        {
            // �E���L�[�ƃ}�E�X�̉E�N���b�N�������ꂽ�ꍇ
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetMouseButtonDown((int)MouseButton.Right) ||
               OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                // ���݂̓��e�ʒu�̐��l�����X�g�̗v�f����菭�Ȃ��ꍇ
                if (currentPos < (projectionPos.Count - MAX_VIEWERPOS_INDEX))
                {
                    // ���e�ʒu�̏��Ԃ��P���₷
                    currentPos++;
                    //ChangePosCamera(currentPos);

                    // �J�����̐؂�ւ������锻���true�ɂ���
                    isChange = true;
                }
            }

            // �����L�[�ƃ}�E�X�̍��N���b�N�������ꂽ�ꍇ
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetMouseButtonDown((int)MouseButton.Left) ||
                OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
            {
                // ���݂̓��e�ʒu�̐��l�����X�g�̐擪�̗v�f����葽���ꍇ
                if (currentPos > MIN_VIEWERPOS_INDEX)
                {
                    // ���e�ʒu�̏��Ԃ��P���炷
                    currentPos--;
                    //ChangePosCamera(currentPos);

                    // �J�����̐؂�ւ������锻���true�ɂ���
                    isChange = true;
                }
            }
        }
        else
        {
            // �E���L�[�ƃ}�E�X�̉E�N���b�N�������ꂽ�ꍇ
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetMouseButtonDown((int)MouseButton.Right) ||
               OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                // ���݂̓��e�ʒu�̐��l�����X�g�̗v�f����菭�Ȃ��ꍇ
                if (currentPos_Front < (projectionPos_Front.Count - MAX_VIEWERPOS_INDEX))
                {
                    // ���e�ʒu�̏��Ԃ��P���₷
                    currentPos_Front++;
                    //ChangePosCamera(currentPos);

                    // �J�����̐؂�ւ������锻���true�ɂ���
                    isChange = true;
                }
            }

            // �����L�[�ƃ}�E�X�̍��N���b�N�������ꂽ�ꍇ
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetMouseButtonDown((int)MouseButton.Left) ||
                OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
            {
                // ���݂̓��e�ʒu�̐��l�����X�g�̐擪�̗v�f����葽���ꍇ
                if (currentPos_Front > MIN_VIEWERPOS_INDEX)
                {
                    // ���e�ʒu�̏��Ԃ��P���炷
                    currentPos_Front--;
                    //ChangePosCamera(currentPos);

                    // �J�����̐؂�ւ������锻���true�ɂ���
                    isChange = true;
                }
            }
        }

        // �J�����̐؂�ւ������锻�肪true�̏ꍇ
        if (isChange)
        {
            // �J�����̈ʒu���w�肵���ʒu�ɐ؂�ւ��鏈��
            ChangePosCamera(currentPos,currentPos_Front,Change_CameraRaw);
        }
    }

    /// <summary>
    /// �J�����̈ʒu���w�肵���ʒu�Ɉړ�������֐�
    /// </summary>
    /// <param name="index"> �ړ�����ʒu(�v�f��)���w�肷��l </param>
    private void ChangePosCamera(int index,int index_Front,bool Flag)
    {
        // �^�[�Q�b�g�̕����Ɍ������邩�̃g���K�[�̔��育�Ƃɏ�����ς���
        switch (isLookTarget)
        {
            // �^�[�Q�b�g�̕����Ɍ�������ꍇ
            case true:
                if (!Flag)
                {
                    // �J�����̈ʒu���w�肵���v�f�̈ʒu�ɐݒ肷��
                    mainCamera.transform.position = projectionPos[index].projector.transform.position;

                    // �J�����̊p�x���w�肵���v�f�̈ʒu�ɐݒ肷��
                    mainCamera.transform.LookAt(projectionPos[index].targetHeadObj);
                }
                else
                {
                    mainCamera.transform.position = projectionPos_Front[index_Front].projector.transform.position;

                    mainCamera.transform.LookAt(projectionPos_Front[index_Front].targetHeadObj);
                }
                break;
            // �^�[�Q�b�g�̕����Ɍ������Ȃ��ꍇ
            case false:
                // �J�����̈ʒu���w�肵���v�f�̈ʒu�ɐݒ肷��
                mainCamera.transform.position = projectionPos[index].projector.transform.position;

                // �J�����̊p�x���w�肵���v�f�̈ʒu�ɐݒ肷��
                mainCamera.transform.rotation = projectionPos[index].projector.transform.rotation;
                break;
        }
    }

    /// <summary>
    /// �J�����̎��_���ړ������邽�߂̊֐�
    /// </summary>
    private void MoveCamera()
    {
        // �J��������������
        float moveSpeed = moveCameraSpeed * Time.deltaTime;

        // �}�E�X��X����Y���̓������擾����
        float moveX = Input.GetAxis("Mouse X") * moveSpeed;
        float moveY = Input.GetAxis("Mouse Y") * -moveSpeed;

        // �X�y�[�X�L�[�������Ă���ꍇ
        if (Input.GetKey(KeyCode.Space))
        {
            // �}�E�X�̃z�C�[�����������ꍇ
            if (Input.GetMouseButtonDown((int)MouseButton.Wheel))
            {
                // �J�����̎���p�������ɖ߂�
                cameraCom.fieldOfView = saveFOV;
            }

            // �J�����̎���p��ύX����i�Y�[���C��/�Y�[���A�E�g�@�\�j
            cameraCom.fieldOfView += moveY;
        }
        // �L�[�������Ă��Ȃ��ꍇ
        else
        {
            // �J��������]������
            mainCamera.transform.Rotate(moveY, moveX, 0);
        }
    }

    #endregion --- Methods ---
}

#region --- Class ---

/// <summary>
/// ���e�ʒu�̏����Ǘ�����N���X
/// </summary>
[System.Serializable]
public class ProjectionInfo
{
    /// <summary>
    /// ���e�ʒu�̐���
    /// </summary>
    [SerializeField]
    private string description;

    /// <summary>
    /// �J�����������^�[�Q�b�g�̓��̈ʒu
    /// </summary>
    public Transform targetHeadObj;

    /// <summary>
    /// ���e�ʒu�̏��
    /// </summary>
    public Transform projector;

    /// <summary>
    /// �����ɉf�����e�ʒu��ݒ肷��g���K�[
    /// </summary>
    public bool initiProjectionPos;
}

#endregion --- Class ---