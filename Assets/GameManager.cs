using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    int m_score = 0;

    static int m_hiscore = 0;

    int m_ammo = 0;

    Player m_player;

    GUIText text_ammo;
    GUIText text_score;
    GUIText text_hiscore;
    GUIText text_life;



	// Use this for initialization
	void Start () {
        Instance = this;
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        text_ammo = this.transform.FindChild("text_ammo").GetComponent<GUIText>();
        text_hiscore = this.transform.FindChild("text_hiscore").GetComponent<GUIText>();
        text_score = this.transform.FindChild("text_score").GetComponent<GUIText>();
        text_life = this.transform.FindChild("text_life").GetComponent<GUIText>();

	
	}
	
	// Update is called once per frame
	
	

    public void SetScore(int score)
    {
        m_score += score;
        Debug.Log(m_score);
        if (m_score > m_hiscore)
        {
            m_hiscore = m_score;
        }
        text_score.text = "score: " + m_score;
        text_hiscore.text = "high score: " + m_hiscore;

    }
    public void SetAmmo(int ammo)
    {
        m_ammo -= ammo;
        if (ammo <= 0)
        {
            m_ammo = 100 - m_ammo;

        }
        text_ammo.text = m_ammo.ToString() + "/100";
    }
    public void SetLife(int life)
    {
        text_life.text = life.ToString();
    }

    void OnGUI()
    {
        if (m_player.m_life <= 0)
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontSize = 40;
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "game over");

            GUI.skin.label.fontSize = 20;
            if (GUI.Button(new Rect(Screen.width * 0.5f - 150, Screen.height * 0.7f, 300, 40), "try again"))
            {
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
    }
}
