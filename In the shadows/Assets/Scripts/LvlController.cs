using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class LvlController : ScriptableObject
{
    #region Variables
    public ProgressController proCont;
    public LevelData data;
    public SoundController soundCont;
    public bool testMode = false;
    [HideInInspector] public ApplicationLifeCycle applicationLifeCycle;
    #endregion

    #region Level Loader
    public void LevelLoader(LevelData data)
    {
        //Add a Debug.log("") before scene manager to see if this method is called via testing/ debugging
        soundCont.PlayGoToScene();
        this.data = data;
        SceneManager.LoadScene(2);
    }
    #endregion

    #region Level Completed Logic
    public void LevelDone(float time)
    {
        soundCont.PlayWin();
        float priv_time;
        if (proCont.records.doneLevels.TryGetValue(data.name, out priv_time))
        {
            if (time < priv_time)
            {
                proCont.records.doneLevels[data.name] = time;
            }
        }
        else
        {
            proCont.records.doneLevels.Add(data.name, time);
        }
    }
    #endregion

    #region Menu Navigation
    public void GoToMenu()
    {
        soundCont.PlayGoToScene();
        SceneManager.LoadScene(1);
    }

    public void GoToStartMenu()
    {
        testMode = false;
        soundCont.PlayGoToScene();
        SceneManager.LoadScene(0);
    }

    #endregion


    /// <summary>
    /// The below event listener is only for debug purposes.
    /// Uncomment to se the debug info written to console
    /// </summary>
    #region On Enable Event Listener
    //void OnEnable()
    //{
    //    Debug.Log("LvlController Enabled");
    //} 
    #endregion

    #region Test Mode On
    public void SetTestMode()
    {
        testMode = true;
    } 
    #endregion
}
