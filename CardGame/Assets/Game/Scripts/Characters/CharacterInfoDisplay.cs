using UnityEngine;
using System.Collections;



public class CharacterInfoDisplay : MonoBehaviour 
{
	[SerializeField] private TextMesh attackText;
	[SerializeField] private TextMesh lifeText;

	void Awake ()
	{
		attackText.gameObject.GetComponent<MeshRenderer> ().sortingLayerName = "CharactersUI";
		lifeText.gameObject.GetComponent<MeshRenderer> ().sortingLayerName = "CharactersUI";
	}

	public void SetStats (CharacterStats stats)
	{
		attackText.text = stats.attack.ToString();
		lifeText.text = stats.life.ToString();
	}

}
