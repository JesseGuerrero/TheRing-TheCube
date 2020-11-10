using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PlayerCamController : MonoBehaviour
    {
        public GameObject crosshairUI;
        public GameObject cameraThird;
        public GameObject cameraFirst;
        bool isFirst;

        // Start is called before the first frame update
        void Start()
        {
            SetCamThirdPerson();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("f"))
            {
                if (isFirst)
                {
                    SetCamThirdPerson();
                }
                else
                {
                    SetCamFirstPerson();
                }
            }
        }

        private void LateUpdate()
        {
            //Needs to be in late update or jitters.
            PosThirdPerson();
        }

        private void PosThirdPerson()
        {
            /*
             Assign camera to player pos
             */
            cameraThird.transform.position = GameObject.FindWithTag("Player").transform.position;
        }

        public bool IsFirst() { return isFirst; }

        private void SetCamFirstPerson()
        {
            //We are in 3rd so switch to first
            cameraFirst.SetActive(true);
            cameraThird.SetActive(false);
            isFirst = true;

            //Hide mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void SetCamThirdPerson()
        {
            //We are in 1st so switch to 3rd
            cameraFirst.SetActive(false);
            cameraThird.SetActive(true);
            isFirst = false;

            //Show mouse
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        //Our ray casting
        public Ray GetCenterRay()
        {
            //raycast from center of screen, https://bit.ly/3pd584D
            return cameraFirst.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        }

        public Ray GetMouseRay()
        {
            return cameraThird.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        }
    }
}