using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationLifeCycle : MonoBehaviour
{
    [SerializeField] GameObject clsPanel;
    public LvlController levelController;

    void Start ()
	{
		if (levelController.applicationLifeCycle != null)
		{
			if (levelController.applicationLifeCycle != this)
            {
                Destroy(gameObject);
            }
		}
		else
		{
            levelController.applicationLifeCycle = this;
		}
		DontDestroyOnLoad(this);
	}
	
	void Update ()
	{
		if (Input.GetKey("escape"))
        {
            clsPanel.SetActive(true);
        }
	}

	public void Exit()
	{
        clsPanel.SetActive(true);
	}
}
