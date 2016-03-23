using UnityEngine;
using System.Collections;


public class SkillVisuals : MonoBehaviour 
{

	[SerializeField] private GameObject hoverobj;
	[SerializeField] private Effects dropFx;

	void Awake ()
	{
		hoverobj.SetActive(false);
		dropFx.gameObject.SetActive(false);
	}

	public void ShowSkill (Vector3 pos)
	{
		SetPosition(pos);
		this.gameObject.SetActive(true);
		hoverobj.SetActive(true);
		dropFx.gameObject.SetActive(false);
	}

	public void ShowFx (Vector3 pos)
	{
		SetPosition(pos);
		this.gameObject.SetActive(true);
		hoverobj.SetActive(false);
		dropFx.gameObject.SetActive(true);
		dropFx.Play();
		dropFx.onEndAction = OnFxEnd;
	}

	private void SetPosition(Vector3 pos)
	{
		pos.z = this.transform.position.z;
		this.transform.position = pos;
	}

	public void OnFxEnd ()
	{
		this.gameObject.SetActive(false);
		UnityEngine.GameObject.Destroy(this.gameObject);
	}

}
