using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : TestInteractable
{
    private bool isOpen = false;
    private bool canBeInteractedWith = true;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public override void OnFocus()
        {
            
        }

        public override void OnInteract()
        {
            if (canBeInteractedWith)
            {
                isOpen = !isOpen;
                Vector3 doorTransformDirection = transform.TransformDirection(Vector3.forward);
                Vector3 playerTransformDirection = FirstPersonController.instance.transform.position - transform.position;
                float dot = Vector3.Dot(doorTransformDirection,playerTransformDirection);

                anim.SetFloat("dot",dot);
                anim.SetBool("isOpen",isOpen);

                StartCoroutine(AutoClose());
            }
        }

        public override void OnLoseFocus()
        {
            
        }

        private IEnumerator AutoClose()
        {
            while (isOpen)
            {
                yield return new WaitForSeconds(3);

                if(Vector3.Distance(transform.position,FirstPersonController.instance.transform.position) > 3)
                {
                    isOpen = false;
                    anim.SetFloat("dot",0);
                    anim.SetBool("isOpen",isOpen);
                }
            }            
        }

    private void Animator_LockInteraction()
    {
        canBeInteractedWith = false;
    }

    private void Animator_UnlockInteraction()
    {
        canBeInteractedWith = true;
    }     



}
