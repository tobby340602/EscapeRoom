using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public Animator closeTutorialButton;
    private bool isMenuOpen = false;

    public void ToggleMenu()
    {
        if (closeTutorialButton.GetCurrentAnimatorStateInfo(0).IsName("MenuSlideIn"))
            return;

        isMenuOpen = !isMenuOpen;
        closeTutorialButton.SetBool("closeScreen", !isMenuOpen);
    }

    public void ToggleTutorialAnimationComplete()
    {
        if (!isMenuOpen)
            closeTutorialButton.gameObject.SetActive(false);
    }
}