using UnityEngine;

public static class ExtensionMethods
{
	public static bool IsPlayingAnimation(this Animator animator)
	{
		return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 || animator.IsInTransition(0);
	}
}