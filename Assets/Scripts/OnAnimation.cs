using UnityEngine;
using UnityEngine.Events;

public class OnAnimation : StateMachineBehaviour
{
    [SerializeField] private UnityEvent onEnterStateAnimation; // Calls when animation starts/enters
    [SerializeField] private UnityEvent onExitStateAnimation; // Calls when exit animation state, not when animation is finished
    [SerializeField] private UnityEvent onFinishedAnimation; // Call when animation is finished

    private bool isAnimationDone;
    private int animationLength = 1;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onEnterStateAnimation?.Invoke();
        isAnimationDone = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (!isAnimationDone && stateInfo.normalizedTime >= animationLength)
       {
            onFinishedAnimation?.Invoke();
            isAnimationDone = true;
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onExitStateAnimation?.Invoke();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
