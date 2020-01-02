using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
	[SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
	[SerializeField] private AudioSource audSource;
	private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();


	private void Start()
	{
		audSource = GetComponent<AudioSource>();
		foreach(AudioClip clip in audioClips)
		{
			if(clip != null)
			{
				clips.Add(clip.name.Trim().ToLower(), clip);
			}
		}
	}

	/// <summary>
	/// Plays a sound 
	/// </summary>
	/// <param name="soundName"></param>
	public void PlaySound(string soundName)
	{
		if(clips.ContainsKey(soundName.ToLower().Trim()))
		{
			audSource.PlayOneShot(clips[soundName.ToLower().Trim()]);
		}
		else  //If the sound is not found
		{
			throw new System.Exception("The audio clip " + soundName.ToLower() + " is not added to the sound controller script on " + this.name +" !");
		}
		
	}
}

