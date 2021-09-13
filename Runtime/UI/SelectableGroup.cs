using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityTools.UI
{
	[RequireComponent(typeof(Selectable))]
	public class SelectableGroup : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
	{
		/// <summary>
		/// An enumeration of selected states of objects
		/// </summary>
		private enum SelectionState
		{
			/// <summary>
			/// The UI object can be selected.
			/// </summary>
			Normal,

			/// <summary>
			/// The UI object is highlighted.
			/// </summary>
			Highlighted,

			/// <summary>
			/// The UI object is pressed.
			/// </summary>
			Pressed,

			/// <summary>
			/// The UI object is selected
			/// </summary>
			Selected,

			/// <summary>
			/// The UI object cannot be selected.
			/// </summary>
			Disabled,
		}
		
		public List<TargetInfo> targets;

		private Selectable _selectable;
		private bool _isPointerDown;
		private bool _hasSelection;
		private bool _isPointerInside;
		
		private void Awake()
		{
			_selectable = GetComponent<Selectable>();
		}

		private void OnEnable()
		{
			if (EventSystem.current && EventSystem.current.currentSelectedGameObject != gameObject)
				OnDeselect(null);
		}

		public void OnSelect(BaseEventData eventData)
		{
			_hasSelection = true;

			foreach (var target in targets)
				target.Select();
		}

		public void OnDeselect(BaseEventData eventData)
		{
			_hasSelection = false;
			
			foreach (var target in targets)
				target.Normal();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_isPointerDown = true;
			
			foreach (var target in targets)
				target.Pressed();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			_isPointerDown = false;

			foreach (var target in targets)
			{
				if (currentSelectionState == SelectionState.Selected)
					target.Select();
				else
					target.Normal();
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_isPointerInside = true;
			
			foreach (var target in targets)
				target.Highlight();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_isPointerInside = false;
			
			foreach (var target in targets)
			{
				if (currentSelectionState == SelectionState.Selected)
					target.Select();
				else
					target.Normal();
			}
		}
		
		private SelectionState currentSelectionState
		{
			get
			{
				if (!_selectable.IsInteractable())
					return SelectionState.Disabled;
				if (_isPointerDown)
					return SelectionState.Pressed;
				if (_hasSelection)
					return SelectionState.Selected;
				if (_isPointerInside)
					return SelectionState.Highlighted;
				return SelectionState.Normal;
			}
		}

		//TODO: create a PropertyDrawer
		[Serializable]
		public class TargetInfo
		{
			public Graphic targetGraphic;
			
			public Selectable.Transition transition = Selectable.Transition.ColorTint;
			
			[Header("ColorTint Only")]
			public ColorBlock colorBlock = ColorBlock.defaultColorBlock;

			[Header("SpriteSwap Only")]
			public Sprite normalSprite;
			public SpriteState spriteState;
			
			public void Normal()
			{
				switch (transition)
				{
					case Selectable.Transition.None:
						break;
					case Selectable.Transition.ColorTint:
						targetGraphic.color = colorBlock.normalColor;
						break;
					case Selectable.Transition.SpriteSwap:
						if (targetGraphic is Image image)
							image.sprite = normalSprite;
						break;
					case Selectable.Transition.Animation:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			
			public void Select()
			{
				switch (transition)
				{
					case Selectable.Transition.None:
						break;
					case Selectable.Transition.ColorTint:
						targetGraphic.color = colorBlock.selectedColor;
						break;
					case Selectable.Transition.SpriteSwap:
						if (targetGraphic is Image image)
							image.sprite = spriteState.selectedSprite;
						break;
					case Selectable.Transition.Animation:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			public void Highlight()
			{
				switch (transition)
				{
					case Selectable.Transition.None:
						break;
					case Selectable.Transition.ColorTint:
						targetGraphic.color = colorBlock.highlightedColor;
						break;
					case Selectable.Transition.SpriteSwap:
						if (targetGraphic is Image image)
							image.sprite = spriteState.highlightedSprite;
						break;
					case Selectable.Transition.Animation:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			public void Pressed()
			{
				switch (transition)
				{
					case Selectable.Transition.None:
						break;
					case Selectable.Transition.ColorTint:
						targetGraphic.color = colorBlock.pressedColor;
						break;
					case Selectable.Transition.SpriteSwap:
						if (targetGraphic is Image image)
							image.sprite = spriteState.pressedSprite;
						break;
					case Selectable.Transition.Animation:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			public void Disable()
			{
				switch (transition)
				{
					case Selectable.Transition.None:
						break;
					case Selectable.Transition.ColorTint:
						targetGraphic.color = colorBlock.disabledColor;
						break;
					case Selectable.Transition.SpriteSwap:
						if (targetGraphic is Image image)
							image.sprite = spriteState.disabledSprite;
						break;
					case Selectable.Transition.Animation:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}