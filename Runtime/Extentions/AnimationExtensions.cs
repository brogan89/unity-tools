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
	}
}