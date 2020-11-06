using UnityEngine;

namespace RPG.Core
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] AudioClip punchSound;
        [SerializeField] AudioClip punchSound1;
        [SerializeField] AudioClip punchSound2;
        int punchIndex = 0;

        // Start is called before the first frame update

        public void PunchSound()
        {
            punchIndex++;
            if (punchIndex == 0) transform.GetComponent<AudioSource>().PlayOneShot(punchSound);        
            if(punchIndex == 2) transform.GetComponent<AudioSource>().PlayOneShot(punchSound1);
            if (punchIndex == 4)
            {
                transform.GetComponent<AudioSource>().PlayOneShot(punchSound2);
                punchIndex = 0;
            }

        }

    }
}