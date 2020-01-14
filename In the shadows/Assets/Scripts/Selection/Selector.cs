using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu()]
public class Selector : ScriptableObject
{
	public ISelectiable current;

	[HideInInspector] public UnityEvent onObjectSelectedEvent;
	[HideInInspector] public UnityEvent onObjectDeselectedEvent;

	/// <summary>
	/// Setting new selected object and update selection state of priv and current objects
	/// </summary>
	/// <param name="newSelected"></param>
	public void RegisterSelection (ISelectiable newSelected)
	{
		if (current != null)
					current.SelectorToggle(false);
		newSelected.SelectorToggle(true);
		current = newSelected;
		if (onObjectSelectedEvent != null)
			onObjectSelectedEvent.Invoke ();
	}

	/// <summary>
	/// Will deselect current selected object
	/// </summary>
	/// <param name="touchedObj"></param>
	public void RemoveSelection (ISelectiable deselectMe)
	{
		deselectMe.SelectorToggle(false);
		current = null;
		if (onObjectDeselectedEvent != null)
			onObjectDeselectedEvent.Invoke ();
	}

	

}