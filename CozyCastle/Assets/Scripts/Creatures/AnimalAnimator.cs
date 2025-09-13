using UnityEngine;

public class AnimalAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayIdle() => animator.SetBool("isWalking", false);
    public void PlayWalk() => animator.SetBool("isWalking", true);
    public void PlayReact() => animator.SetTrigger("react");
    public void PlaySleep(bool sleeping) => animator.SetBool("isSleeping", sleeping);
}
