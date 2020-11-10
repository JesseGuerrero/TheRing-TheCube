using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Core
{
    public class PlayerAudio : MonoBehaviour, IAudio
    {
        public List<AudioClip> punchSounds;        
        [SerializeField] AudioClip deathSound;

        int punchIndex = 0;

        // Start is called before the first frame update

        public void PunchSound()
        {
            int len = punchSounds.Count;
            int index = Random.Range(0, len * 2);
            if (len > 0 && index < len) { transform.GetComponent<AudioSource>().PlayOneShot(punchSounds.ElementAt(index)); }
        }

        public void DeathSound()
        {
            transform.GetComponent<AudioSource>().PlayOneShot(deathSound);
        }
        public void StartAggroSound()
        {

        }

    }
}