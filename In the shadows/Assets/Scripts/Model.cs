using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    #region Variables
    //Public Variables
    public Material normalMat;
    public Material highligtedMat;

    //Private Variables
    private new MeshRenderer renderer;
    private Material[] materials;
    #endregion

    #region Toggle Between selections of materials
    public void SelectionToggle(bool selection)
    {
        if (renderer == null)
        {
            renderer = GetComponent<MeshRenderer>();
        }

        if (selection)
        {
            materials[0] = highligtedMat;
            renderer.sharedMaterials = materials;
        }
        else
        {
            materials[0] = normalMat;
            renderer.sharedMaterials = materials;
        }
    }
    #endregion

    void Start()
	{
		renderer = GetComponent<MeshRenderer>();
		materials = renderer.sharedMaterials;
	}

}
