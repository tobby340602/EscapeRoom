using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public Animator menuPanelAnimator;
    private bool isMenuOpen = false;

    public void ToggleMenu()
    {
        if (menuPanelAnimator.GetCurrentAnimatorStateInfo(0).IsName("MenuSlideIn"))
            return;

        isMenuOpen = !isMenuOpen;
        menuPanelAnimator.SetBool("isOffScreen", !isMenuOpen);
    }

    public void ToggleMenuAnimationComplete()
    {
        if (!isMenuOpen)
            menuPanelAnimator.gameObject.SetActive(false);
    }
}
