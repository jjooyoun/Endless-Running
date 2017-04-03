using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

	public SceneFader fader;

	public Button[] levelButtons;

	void Start ()
	{
        //PlayerPrefs.SetInt("levelReached", 2);
        int levelReached = PlayerPrefs.GetInt("levelReached", 2);

		for (int i = 0; i < levelButtons.Length; i++)
		{
			if (i + 1 > levelReached)
				levelButtons[i].interactable = false;
		}
	}

	public void Select (int sceneIndex)
	{
		fader.FadeTo(sceneIndex);
	}
}
