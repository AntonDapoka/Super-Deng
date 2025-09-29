using UnityEngine;

public class controlScene : MonoBehaviour {
    public GameObject g1, g2, g3, g4;
    public AudioSource _audiosource;
    public AudioPeer audiopeer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Time.timeScale = 1f + audiopeer._AmplitudeBuffer;

		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            g1.SetActive(!g1.activeSelf);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            g2.SetActive(!g2.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            g3.SetActive(!g3.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            g4.SetActive(!g4.activeSelf);
        }
  
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _audiosource.mute = !_audiosource.mute;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _audiosource.time = 0;
        }
    }
}
