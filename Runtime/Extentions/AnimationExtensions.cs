using System.Linq;
using UnityEngine;

namespace UnityTools.Extensions
{
	public static class AnimationExtensions
	{
		public static bool IsAnimationRunning(this Animator animator, string animationName)
		{
			return animator.runtimeAnimatorController.animationClips.Any(x => x.name == animationName);
		}

		public static AnimationClip GetAnimationClip(this Animator animator, string animationName)
		{
			return animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == animationName);
		}
	}
}