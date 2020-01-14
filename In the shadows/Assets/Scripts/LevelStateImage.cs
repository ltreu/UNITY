using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateImage : MonoBehaviour
{
    #region Variables
    public Level level;
    public Material MaterialLocked;
    public Material MaterialUnlocked;
    public Material MaterialDone;

    private readonly LevelState currentState = LevelState.UNLOCKED;
    private new MeshRenderer renderer;
    private Material[] materials;
    #endregion

    /// <summary>
    /// The switch case below is triggered on the level select menu
    /// It decied the material for use when a level is in one of three states:
    /// UNLOCKED, LOCKED and COMPLETED
    /// </summary>
    #region Material Setting Switch Case 
    public void SetMaterial()
    {
        switch (level.state)
        {
            case LevelState.UNLOCKED:
                {
                    materials[0] = MaterialUnlocked;
                    break;
                }
            case LevelState.LOCKED:
                {
                    materials[0] = MaterialLocked;
                    break;
                }
            case LevelState.DONE:
                {
                    materials[0] = MaterialDone;
                    break;
                }
        }

        renderer.sharedMaterials = materials;
    } 
    #endregion

    IEnumerator Start()
	{
		renderer = GetComponent<MeshRenderer>();
		materials = renderer.sharedMaterials;
		
		level.onLvlStateChng.AddListener(SetMaterial);
		yield return null;
		if (currentState != level.state)
        {
            SetMaterial();
        }
	}
}
