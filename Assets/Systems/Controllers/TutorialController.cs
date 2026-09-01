using System;
using System.Collections;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private Transform tutorialPanel;
    [SerializeField] private Animator animator;

    private TutorialPhase currentPhase;

    public bool IsActive => currentPhase != TutorialPhase.Unactive && currentPhase != TutorialPhase.Completed;

    private void Awake()
    {
        currentPhase = TutorialPhase.Unactive;
    }

    public void ShowMovementInstruction()
    {
        SetMovementActive(true);
    }

    public void SetMovementActive(bool newActive)
    {
        tutorialPanel.gameObject.SetActive(newActive);

        if(newActive == true)
        {
            if (currentPhase == TutorialPhase.Unactive)
            {
                ShowContent(1);
            }
        }
    }

    public void ShowFallInstructionCoroutine()
    {
        if (currentPhase == TutorialPhase.Movement || currentPhase == TutorialPhase.Fall)
        {
            ShowFallInstruction();
            StartCoroutine(nameof(FallIntructionCoroutine));
            Debug.Log("[Tutorial Controller] Show Fall Instruction Coroutine");
        }
    }

    private void ShowFallInstruction()
    {
        if (currentPhase == TutorialPhase.Movement || currentPhase == TutorialPhase.Fall)
        {
            ShowContent(2);
            Debug.Log("[Tutorial Controller] Show Fall Instruction");
        }
    }

    public void ShowFallInstruction(float delay)
    {
        Invoke(nameof(ShowFallInstruction), delay);
    }

    public void HideTutorial()
    {
        tutorialPanel.gameObject.SetActive(false);
    }

    private IEnumerator FallIntructionCoroutine()
    {
        Time.timeScale = 0.2f;
        animator.speed = 3f;

        yield return new WaitForSecondsRealtime(2f);

        ShowContent();
        Time.timeScale = 1f;
        animator.speed = 1f;

        currentPhase = TutorialPhase.Completed;
    }

    private void ShowContent(int index)
    { 
        ShowContent(true);

        tutorialPanel.GetChild(index).gameObject.SetActive(true);

        switch (index)
        {
            case 1:
                currentPhase = TutorialPhase.Movement;
                animator.SetTrigger("Movement");
                break;

            case 2:
                currentPhase = TutorialPhase.Fall;
                animator.SetTrigger("Fall");
                break;

            default:
                Debug.Log("[Tutorial Controller] Unknown content index: " + index);
                break;
        }

        Debug.Log("[Tutorial Controller] Show Content (index): " + index + " name: " + tutorialPanel.GetChild(index).name);
    }

    private void ShowContent(bool panelActive = false)
    {
        int childCount = tutorialPanel.childCount;
        Debug.Log("[Tutorial Controller] Show Content (child count): " + childCount);

        tutorialPanel.gameObject.SetActive(true);

        for (int i = childCount - 1; i > 0; i--)
        {
            Transform content = tutorialPanel.GetChild(i);
            content.gameObject.SetActive(false);
        }

        tutorialPanel.gameObject.SetActive(panelActive);
    }
}

public enum TutorialPhase { Unactive, Movement, Fall, Completed }