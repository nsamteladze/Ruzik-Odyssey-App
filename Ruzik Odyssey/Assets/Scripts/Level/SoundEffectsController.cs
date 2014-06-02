using UnityEngine;
using System.Collections;

public class SoundEffectsController : MonoBehaviour
{
	public AudioClip laserShot;
	public AudioClip missileShot;
	public AudioClip playerExplosion;
	public AudioClip[] playerTaunts;

	private void Awake()
	{
		if (Instance != null) throw new UnityException("Multiple sound effects controllers detected");
		Instance = this;
	}

	public static SoundEffectsController Instance { get; private set; }

	public void PlayLaserShot()
	{
		PlayAudioClip(laserShot);
	}

	public void PlayMissileShot()
	{
		PlayAudioClip(missileShot);
	}

	public void PlayPlayerExplosion()
	{
		PlayAudioClip(playerExplosion);
	}

	public void PlayPlayerTaunt()
	{
		if (Random.Range(0, 3) < 1)
		{
			PlayAudioClip(playerTaunts[Random.Range(0, playerTaunts.Length)]);
		}
	}

	private void PlayAudioClip(AudioClip audioClip)
	{
		AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);
	}

}
