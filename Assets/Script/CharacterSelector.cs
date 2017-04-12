using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour {

    public List<GameObject> characterList;

    void Start () {
        GameObject character = Resources.Load("Prefabs/Snowball") as GameObject;
        GameObject _char = Instantiate(character) as GameObject;
        _char.transform.SetParent(GameObject.Find("CharacterList").transform);
        characterList.Add(_char);
        _char.SetActive(false);
        characterList[0].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
