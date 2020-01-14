using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopWatch : MonoBehaviour
{
	private Text text;
    private float _time = 0f;
    private bool stop = false;

    private IEnumerator Counting()
    {
        while (!stop)
        {
            text.text = "Time : " + _time;
            yield return new WaitForSeconds(1f);
            _time++;
        }
    }

    public float time
	{
		get
		{
			return _time;
		}
	}

	void Start()
	{
		text = GetComponent<Text>();
	}

	public void StartStopWatch()
	{
		StartCoroutine(Counting());
	}

	public float Stop()
	{
		stop = true;
		return _time;
	}

	

}
