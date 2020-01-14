using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum LevelMode {ONE_MODEL, TWO_MODELS}

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 1)]
public class LevelData : ScriptableObject
{
	public string name;
	public LevelMode mode = LevelMode.ONE_MODEL;
	
	[Header("Model1")]
	public GameObject model0;
	public float precision0;
	public Vector3 startRotation0;
	public Vector3 wRot0;
	public Vector3 wRot0_1;

	[Header("Respective models rotation")]
	public float precisionAll;
	public Vector3 wRotAll;
	public Vector3 wRotAll_1;

    [Header("Model1")]
    public GameObject model1;
    public float precision1;
    public Vector3 startRotation1;
    public Vector3 wRot1;
    public Vector3 wRot1_1;
}

