using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmsAnimationController : MonoBehaviour
{
	[SerializeField] private Animator playerArmsAnimator;

	public void SwingTool()
	{
		playerArmsAnimator.SetTrigger("SwungTool");
	}


	public void SetHoldItem(bool isHoldingItem)
	{
		playerArmsAnimator.SetBool("HoldingItem", isHoldingItem);
	}

	public void HoldTool(bool isHoldingTool)
	{
		playerArmsAnimator.SetBool("HoldingTool", isHoldingTool);
	}
}
