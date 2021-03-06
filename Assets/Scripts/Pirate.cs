using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pirate : MonoBehaviour
{
	public NavMeshAgent agent;

	public int hunger;
	public int thirst;
	public int morale;

	private int maxHunger;
	private int maxThirst;
	private int maxMorale;

	public Vector3 origLocation;
	public Vector3 curLocation;
	public Vector3 lastLocation;

	public bool selected = false;
	public bool doneJob = false;
	public bool returning = false;

	public Job lastJob;
	public double jobStartTime;

	public AudioClip[] pirateSpeechClips;

	public Animator anim;

	//Initialization
	void Start ()
	{
		maxHunger = DataValues.instance.getMaxHunger ();
		maxThirst = DataValues.instance.getMaxThirst ();
		maxMorale = DataValues.instance.getMaxMorale ();
		hunger = maxHunger;
		thirst = maxThirst;
		morale = maxMorale;
		agent = gameObject.GetComponent<NavMeshAgent> ();
		origLocation = gameObject.transform.position;
		curLocation = gameObject.transform.position;
		lastLocation = gameObject.transform.position;
		anim = gameObject.GetComponent<Animator> ();
	}

	void Update ()
	{
		curLocation = gameObject.transform.position;

		if (Vector3.Distance (curLocation, agent.destination) <= 1f) {
			agent.updatePosition = false;
			agent.updateRotation = false;
			anim.SetBool ("walk", false);
		} else {
			//agent.enabled = true;
			anim.SetBool ("walk", true);
		} 

		if (selected) {
			updateUI ();
		}

		if (curLocation == origLocation)
			returning = false;

		HungerAndThirst ();

		lastLocation = curLocation;
	}

	private void HungerAndThirst() {
		if (DayNightController.minutes == 59 && DayNightController.worldTimeHour % 3 == 0)
			updateValues(true, true, false, false, 1);
	}

	public void say (int audioIndex)
	{
		if (!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().clip = pirateSpeechClips [audioIndex];
			GetComponent<AudioSource>().pitch = Random.Range (1.0F, 1.15F);
			GetComponent<AudioSource>().Play ();
		}
	}

	public void updateUI ()
	{
		GameObject.Find ("StatBars").GetComponent<StatBarAnimator> ().changeHunger (hunger);
		GameObject.Find ("StatBars").GetComponent<StatBarAnimator> ().changeThirst (thirst);
		GameObject.Find ("StatBars").GetComponent<StatBarAnimator> ().changeMorale (morale);
		Sprite [] pirateFaces = GameObject.Find ("PirateManager").GetComponent<PirateManager> ().pirateFaces;
		GameObject.Find ("StatBars").GetComponent<StatBarAnimator> ().changeFaces (pirateFaces);
	}

	public void updateValues (bool h, bool t, bool m, bool reset, int delta)
	{
		if (h) {
			if (hunger + delta <= 0)
				hunger = 0;
			else if (hunger + delta >= maxHunger)
				hunger = maxHunger;
			else if (reset)
				hunger = maxHunger;
			else
				hunger = hunger + delta;
		}
		
		if (t) {
			if (thirst + delta <= 0)
				thirst = 0;
			else if (thirst + delta >= maxThirst)
				thirst = maxThirst;
			else if (reset)
				thirst = maxThirst;
			else
				thirst = thirst + delta;
		}
		
		if (m) {
			if (morale + delta <= 0)
				morale = 0;
			else if (morale + delta >= maxMorale)
				morale = maxMorale;
			else if (reset)
				morale = maxMorale;
			else
				morale = morale + delta;
		}
	}
}