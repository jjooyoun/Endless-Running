using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour {

    public List<GameObject> characterList;
    public int index = 0;
    public Text charName;
    public string[] nameList = { "SnowBall", "UCLABall", "USCBall" };

    void Start () {
        GameObject[] character = Resources.LoadAll<GameObject>("Prefabs/Character");
        foreach (GameObject c in character) {
            GameObject _char = Instantiate(c) as GameObject;
            _char.transform.SetParent(GameObject.Find("CharacterList").transform);
            characterList.Add(_char);
            _char.SetActive(false);
            characterList[index].SetActive(true);
        }
    }

    public void Next() {
        characterList[index].SetActive(false);
        if (index == characterList.Count - 1) {
            index = 0;
        } else {
            index++;
        }
        characterList[index].SetActive(true);
        setCharInfo();
    }

    public void Previous() {
        characterList[index].SetActive(false);
        if (index == 0) {
            index = characterList.Count - 1;
        } else {
            index--;
        }
        characterList[index].SetActive(true);
        setCharInfo();
    }

    public void Select(){
        string ball_mat_path = "Prefabs/Character" + "/" + nameList[index];
        PlayerPrefs.SetString("BALL_MAT_PATH", ball_mat_path);
        Debug.Log("mat_path:" + PlayerPrefs.GetString("BALL_MAT_PATH"));
        PlayerPrefs.Save();
    }

    private void setCharInfo() {
        charName.GetComponent<Text>().text = nameList[index];
    }
}
