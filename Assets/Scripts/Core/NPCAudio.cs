using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Core
{
    public class NPCAudio : MonoBehaviour, IAudio
    {
        [SerializeField] AudioClip punchSound;
        [SerializeField] AudioClip punchSound1;
        [SerializeField] AudioClip punchSound2;
        [SerializeField] AudioClip deathSound;
        public List<AudioClip> startAggroSound;

        int punchIndex = 0;

        // Start is called before the first frame update

        public void PunchSound()
        {
            return;
        }

        public void DeathSound()
        {
            transform.GetComponent<AudioSource>().PlayOneShot(deathSound);
        }

        public void StartAggroSound()
        {
            int len = startAggroSound.Count;
            if (len > 0) { transform.GetComponent<AudioSource>().PlayOneShot(startAggroSound.ElementAt(Random.Range(0, len))); }
        }
    }
}
