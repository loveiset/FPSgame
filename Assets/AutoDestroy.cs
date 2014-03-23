using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
    public float m_timer = 1.0f;


	// Use this for initialization
	
	// Update is called once per frame
	void Update () {

        m_timer -= Time.deltaTime;
        if (m_timer <= 0)
        {
            Destroy(this.gameObject);
        }
	
	}
}
