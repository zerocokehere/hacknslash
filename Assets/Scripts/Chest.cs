using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
[RequireComponent (typeof(AudioSource))]
public class Chest : MonoBehaviour {
	
	public enum State {
		open,
		close,
		inbetween
	}
	
	public AudioClip openSound;
	public AudioClip closeSound;
	
	public GameObject particleEffect;
	
	public GameObject[] parts;
	private Color[] _defaultColors;
	
	public State state;
	
	public float maxDistance = 2;		//the max distance the player can be to open this chest
	
	// Use this for initialization
	void Start () {
		state = Chest.State.close;
		
		particleEffect.SetActive (false);
		
		_defaultColors = new Color[parts.Length];
		
		if(parts.Length > 0)
			for(int cnt = 0; cnt < _defaultColors.Length; cnt++)
				_defaultColors[cnt] = parts[cnt].renderer.material.GetColor("_Color");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void OnMouseEnter() {
		Debug.Log("Enter");	
		Highlight(true);
	}
	
	public void OnMouseExit() {
		Debug.Log("Exit");	
		Highlight(false);
	}
	public void OnMouseUp() {
		Debug.Log("Up");	
		
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		
		if(go == null)
			return;
		if(Vector3.Distance(transform.position, go.transform.position) > maxDistance)
			return;
		
		switch(state) {
		case State.open:
			state = Chest.State.inbetween;
			StartCoroutine("Close");
			break;
		case State.close:
			state = Chest.State.inbetween;
			StartCoroutine("Open");
			break;
		}
//		if(state == Chest.State.close)
//			Open();
//		else 
//			Close();
	}
	
	private IEnumerator Open() {
		
		animation.Play("open");
		
		particleEffect.SetActive (true);
		audio.PlayOneShot(openSound);
		yield return new WaitForSeconds(animation["open"].length);
		state = Chest.State.open;
		Messenger<int, GameObject>.Broadcast("PopulateChest", 5, gameObject, MessengerMode.DONT_REQUIRE_LISTENER);

	}
	
	private IEnumerator Close() {
		animation.Play("close");
		particleEffect.SetActive (false);
		audio.PlayOneShot(closeSound);	
		yield return new WaitForSeconds(animation["close"].length);
		state = Chest.State.close;
	}
	
	private void Highlight(bool glow) {
		if(glow) {
			if(parts.Length > 0)
				for(int cnt = 0; cnt < _defaultColors.Length; cnt++)
					parts[cnt].renderer.material.SetColor("_Color",Color.yellow);			
		}
		else { 
			if(parts.Length > 0)
			for(int cnt = 0; cnt < _defaultColors.Length; cnt++)
				parts[cnt].renderer.material.SetColor("_Color", _defaultColors[cnt]);
		}
	}
	
}
