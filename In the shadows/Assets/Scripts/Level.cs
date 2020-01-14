using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public enum LevelState {LOCKED, UNLOCKED, DONE}
public class Level : MonoBehaviour, ISelectiable
{
    #region Variables
    Vector3 pos;

    //Public Variables
    public int numb;
    public bool isDragged = false;
    public TextMesh lvlNameTxt;
    public TextMesh lvlTimeTxt;
    public LevelState dfltState;
    public LevelData lvlData;
    public LvlController lvlController;
    public Material nrmlMtrl;
    public Material hilightMtrl;
    public Selector selector;
    public UnityEvent onLvlStateChng;
    public Level nxtLvl;
    public float rotateSpd;

    //Private Variables
    private Vector3 startPosition;
    private Rigidbody rgdbdy;
    private bool selected = false;
    private MeshRenderer renderer;
    private Material[] mats;
    private int selectionCount = 0;
    private LevelState _state;
    #endregion

    #region Constructor
    public LevelState state
    {
        get
        {
            return _state;
        }
    }
    #endregion

    #region Level Starter
    IEnumerator Start()
    {
        if (lvlData == null)
        {
            Destroy(this);
        }
        startPosition = transform.position;
        name = lvlData.name;
        rgdbdy = GetComponent<Rigidbody>();
        renderer = GetComponent<MeshRenderer>();
        _state = dfltState;
        mats = renderer.sharedMaterials;
        lvlNameTxt.text = lvlData.name;
        yield return null;

        RecordChecker();

        if (state == LevelState.LOCKED && lvlController.testMode)
        {
            StateChanger(LevelState.UNLOCKED);
        }
    }
    #endregion

    #region Level Selector
    public void LevelSelector()
    {
        lvlController.LevelLoader(lvlData);
    }
    #endregion

    #region State Changer
    public void StateChanger(LevelState newState)
    {
        if (_state != newState && lvlData != null)
        {
            _state = newState;

            if (onLvlStateChng != null)
            {
                onLvlStateChng.Invoke();
            }
        }
    } 
    #endregion

    #region Record Checker
    void RecordChecker()
    {
        if (lvlData != null && lvlController.proCont.records.doneLevels.ContainsKey(lvlData.name))
        {
            lvlTimeTxt.text = "Time : " + lvlController.proCont.records.doneLevels[lvlData.name];
            StateChanger(LevelState.DONE);

            if (nxtLvl != null)
            {
                if (nxtLvl.state == LevelState.LOCKED)
                {
                    nxtLvl.StateChanger(LevelState.UNLOCKED);
                }
            }
        }
    }
    #endregion

    #region Selector Toggle
    public void SelectorToggle(bool selection)
    {
        if (renderer == null)
        {
            renderer = GetComponent<MeshRenderer>();
        }

        selected = selection;

        if (selected)
        {
            mats[0] = hilightMtrl;
            renderer.sharedMaterials = mats;
            lvlNameTxt.gameObject.SetActive(true);

            if (state == LevelState.DONE)
            {
                lvlTimeTxt.gameObject.SetActive(true);
            }
        }
        else
        {
            mats[0] = nrmlMtrl;
            renderer.sharedMaterials = mats;
            lvlNameTxt.gameObject.SetActive(false);

            if (state == LevelState.DONE)
            {
                lvlTimeTxt.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region On Mouse Interaction Event Listeners
    private void OnMouseDown()
    {
        pos = Input.mousePosition;
        selector.RegisterSelection(this);
    }

    void OnMouseDrag()
    {
        isDragged = true;
        float x = Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpd * 10;
        rgdbdy.angularVelocity = Vector3.zero;
        rgdbdy.AddTorque(x, 0, 0);
    }

    void OnMouseUp()
    {
        if (Vector3.Distance(pos, Input.mousePosition) <= 2)
        {
            if (selected && selectionCount > 0 && state != LevelState.LOCKED)
            {
                LevelSelector();
                selectionCount = 0;
            }
            else
            {
                selector.RegisterSelection(this);
                selectionCount++;
            }
        }
        else if (isDragged)
        {
            if (selected)
            {
                selector.RemoveSelection(this);
            }
            isDragged = false;
        }
    } 
    #endregion
}
