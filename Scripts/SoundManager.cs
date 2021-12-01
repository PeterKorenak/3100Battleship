using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip[] clips;

    private AudioSource player;

    private void Start()
    {
        player = GetComponent<AudioSource>();
    }

    public void PlaySound(int index)
    {
        if (player)
        {
            player.clip = clips[index];
            player.Play();
        }
    }

    public bool CheckIfPlaying()
    {
        return player.isPlaying;
    }
}
