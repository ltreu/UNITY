using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    #region Variables
    //Public variables
    public LvlController lvlController;
    public ItemsStackCounter tipsDisplayer;
    public GameObject winPanel;
    public StopWatch stopWatch;
    public GameObject modelsHolder;
    public float rotationSpeed;
    public LevelData data;
    public new Transform light;
    public Transform anchor0;
    public Transform anchor1;
    public float smoothRotateTime;
    public bool blockWhenWin = false;

    //Private Variables
    private bool pause = false;
    private bool win = false;
    private bool oneModelMode = true;
    private bool firstActive = true;
    private float time;
    private Quaternion wRot0, wRot1, wRotAll, wRot0_1, wRot1_1, wRotAll_1;
    private Rigidbody rgdbdy0, rgdbdy1, rgdbdyCur, rgdbdyAll;
    private Transform trans0, trans1, transCurrent, transAll;
    #endregion

    void Awake()
	{
		if (lvlController.data != null)
        {
            data = lvlController.data;
        }
	}

	void Start()
	{
        SceneSetup();

		StartCoroutine(RotationManager());
		stopWatch.StartStopWatch();
	}

    #region Scene Setup Method
    private void SceneSetup()
    {
        if (data == null)
        {
            Destroy(gameObject);
        }

        oneModelMode = data.mode == LevelMode.ONE_MODEL;

        if (oneModelMode)
        {
            GameObject GameObj0 = Instantiate(data.model0, modelsHolder.transform, false);
            transCurrent = GameObj0.transform;
            transCurrent.rotation = Quaternion.Euler(data.startRotation0);
            rgdbdyCur = GameObj0.GetComponent<Rigidbody>();
            wRot0 = Quaternion.Euler(data.wRot0);
            wRot0_1 = Quaternion.Euler(data.wRot0_1);
        }
        else
        {
            GameObject GameObj0 = Instantiate(data.model0, anchor0.position, Quaternion.Euler(data.startRotation0), modelsHolder.transform);
            GameObject GameObj1 = Instantiate(data.model1, anchor1.position, Quaternion.Euler(data.startRotation0), modelsHolder.transform);

            trans0 = GameObj0.transform;
            rgdbdy0 = GameObj0.GetComponent<Rigidbody>();
            trans1 = GameObj1.transform;
            rgdbdy1 = GameObj1.GetComponent<Rigidbody>();
            transCurrent = GameObj0.transform;
            rgdbdyCur = GameObj0.GetComponent<Rigidbody>();
            rgdbdyAll = modelsHolder.GetComponent<Rigidbody>();
            transAll = modelsHolder.transform;

            wRot0 = Quaternion.Euler(data.wRot0);
            wRot1 = Quaternion.Euler(data.wRot1);
            wRotAll = Quaternion.Euler(data.wRotAll);
            wRot0_1 = Quaternion.Euler(data.wRot0_1);
            wRot1_1 = Quaternion.Euler(data.wRot1_1);
            wRotAll_1 = Quaternion.Euler(data.wRotAll_1);
        }
    }
    #endregion

    #region Rotate Smoothly
    private IEnumerator RotateSmoothly()
    {
        bool wasBlocked = blockWhenWin;
        blockWhenWin = true;
        float rate = 1.0f / smoothRotateTime;
        float t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime * rate;
            if (!oneModelMode)
            {
                Quaternion temp0 = trans0.rotation;
                Quaternion temp1 = trans1.rotation;
                transAll.rotation = Quaternion.Slerp(transAll.rotation, wRotAll, t);
                trans0.rotation = Quaternion.Slerp(temp0, wRot0, t);
                trans1.rotation = Quaternion.Slerp(temp1, wRot1, t);
            }
            else
            {
                transCurrent.rotation = Quaternion.Slerp(transCurrent.rotation, wRot0, t);
            }
            yield return null;
        }

        blockWhenWin = wasBlocked;
    }
    #endregion

    #region Rotation Manager
    IEnumerator RotationManager()
    {
        while (true)
        {
            if (!pause)
            {
                if (!win || !blockWhenWin)
                {
                    InputChecker();
                }
                if (!win)
                {
                    RotationChecker();
                }
                if (Input.GetKeyUp(KeyCode.P))
                {
                    if (!oneModelMode)
                    {
                        Debug.Log("trans0 :" + trans0.rotation.eulerAngles + " trans1: " + trans1.rotation.eulerAngles + " transAll: " + transAll.rotation.eulerAngles);
                    }
                    else
                    {
                        Debug.Log("trans0 :" + transCurrent.rotation.eulerAngles);
                    }
                }
            }
            yield return null;
        }
    }
    #endregion

    #region Rotation Checker
    private void RotationChecker()
    {
        if (oneModelMode)
        {
            float angle = Quaternion.Angle(transCurrent.rotation, wRot0);
            float angle_1 = Quaternion.Angle(transCurrent.rotation, wRot0_1);

            angle = (angle < angle_1) ? angle : angle_1;

            if (angle < 90)
            {
                float percent = ((180 - angle) / (180 - data.precision0)) * 100;
                tipsDisplayer.Show(percent);
            }
            if (angle <= data.precision0)
            {
                Winner();
            }
        }
        else
        {
            float c0 = Quaternion.Angle(trans0.rotation, wRot0);
            float c1 = Quaternion.Angle(trans1.rotation, wRot1);
            float c2 = Quaternion.Angle(transAll.rotation, wRotAll);

            float c0_1 = Quaternion.Angle(trans0.rotation, wRot0_1);
            float c1_1 = Quaternion.Angle(trans1.rotation, wRot1_1);
            float c2_1 = Quaternion.Angle(transAll.rotation, wRotAll_1);

            c0 = (c0 < c0_1) ? c0 : c0_1;
            c1 = (c1 < c1_1) ? c1 : c1_1;
            c2 = (c2 < c2_1) ? c2 : c2_1;

            float percent = 0;
            if (c0 <= data.precision0 + 20) percent += 20;
            if (c0 <= data.precision0) percent += 20;
            if (c1 <= data.precision1 + 20) percent += 20;
            if (c1 <= data.precision1) percent += 20;
            if (c2 <= data.precisionAll + 20) percent += 10;
            if (c2 <= data.precisionAll) percent += 10;

            tipsDisplayer.Show(percent);

            if (percent >= 98)
            {
                Winner();
            }
        }
    }
    #endregion

    #region Input Checker
    private void InputChecker()
    {
        bool GeneralRotation = Input.GetMouseButton(0);
        bool SWPRotation = !oneModelMode && (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl));
        bool RotateZ = Input.GetKey(KeyCode.Z) && Input.GetMouseButton(0);
        bool RotateBothZ = !oneModelMode && Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));

        if (RotateZ)
        {
            float x = Input.GetAxis("Mouse X");
            transCurrent.Rotate(light.forward, x / 2);
        }
        else if (RotateBothZ)
        {
            Quaternion temp0 = trans0.rotation;
            Quaternion temp1 = trans1.rotation;
            float x = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
            float y = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;

            if (!Input.GetKey("space"))
            {
                transAll.Rotate(y, x, 0, Space.World);
            }
            else if (Input.GetKey("space"))
            {
                transAll.Rotate(light.forward, x / 10);
            }
            trans0.rotation = temp0;
            trans1.rotation = temp1;
        }
        else if (SWPRotation)
        {
            ModelSwapper();
        }
        else if (GeneralRotation)
        {
            float x = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
            float y = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;

            transCurrent.Rotate(y, x, 0, Space.World);

            rgdbdyCur.angularVelocity = Vector3.zero;
            rgdbdyCur.AddTorque(y, x, 0);
        }
    }
    #endregion

    #region Model Swapper
    private void ModelSwapper()
    {
        firstActive = !firstActive;
        transCurrent.GetComponent<Model>().SelectionToggle(false);
        //trying to implement if statement for the  below
        //toDo: Test if statements below:
        //transCurrent = (firstActive) ? trans0 : trans1;
        if(transCurrent.Equals(firstActive))
        {
            transCurrent = trans0;
        }
        else
        {
            transCurrent = trans1;
        }
        //rgdbdyCur = (firstActive) ? rgdbdy0 : rgdbdy1;
        if(rgdbdyCur.Equals(firstActive))
        {
            rgdbdyCur = rgdbdy0;
        }
        else
        {
            rgdbdyCur = rgdbdy1;
        }
        transCurrent.GetComponent<Model>().SelectionToggle(true);
        lvlController.soundCont.PlayChangeModel();
    }
    #endregion

    #region Winner
    private void Winner()
    {
        winPanel.SetActive(true);

        win = true;
        lvlController.LevelDone(stopWatch.Stop());
        StartCoroutine(RotateSmoothly());
    }
    #endregion

}
