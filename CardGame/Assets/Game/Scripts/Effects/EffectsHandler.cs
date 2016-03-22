using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class EffectsHandler : MonoBehaviour 
{

	private static EffectsHandler instance;
	public static EffectsHandler Instance
	{
		get { return instance; }
	}

	[SerializeField] private GameObject[] referenceEffects;

	private Dictionary<EffectsType, Effects> effectsList;
	private int fxCounter = 0;

	void Awake ()
	{
		instance = this;
		fxCounter = 0;

		effectsList = new Dictionary<EffectsType, Effects>();
		for(int i=0; i < referenceEffects.Length; i++)
		{
			Effects refEffect = referenceEffects[i].gameObject.GetComponent<Effects>();
			if(refEffect == null)
				continue;
			if(!effectsList.ContainsKey(refEffect.Type))
				effectsList.Add(refEffect.Type, refEffect);
		}
	}

	public void PlayEffectsAt (EffectsType fxType, Vector3 fxPos)
	{
		Effects refFx = GetEffects(fxType);
		if(refFx == null)
			return;

		fxCounter++;

		GameObject refObj = GameObject.Instantiate(refFx.gameObject) as GameObject;
		refObj.name = "fx_" + fxCounter;
		refObj.transform.SetParent(this.transform);

		Effects playFx = refObj.GetComponent<Effects>();
		if(playFx != null)
		{
			refObj.SetActive(true);
			playFx.PlayAt(fxPos);
		}
	}

	public Effects GetEffects(EffectsType fxType)
	{
		if(effectsList.ContainsKey(fxType))
		{
			return effectsList[fxType];
		}
		return null;
	}


}
