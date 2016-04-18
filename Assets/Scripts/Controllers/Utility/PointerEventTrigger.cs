using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems {
	[AddComponentMenu("Event/Pointer Event Trigger")]
	public class PointerEventTrigger :
	MonoBehaviour,
	IPointerEnterHandler,
	IPointerExitHandler,
	IPointerDownHandler,
	IPointerUpHandler,
	IPointerClickHandler,
	IInitializePotentialDragHandler,
	IBeginDragHandler,
	IDragHandler,
	IEndDragHandler,
	IDropHandler,
	IScrollHandler {
		[Serializable]
		public class TriggerEvent : UnityEvent<PointerEventData> {

		}

		[Serializable]
		public class Entry {
			public EventTriggerType eventType = EventTriggerType.PointerClick;
			public TriggerEvent callback = new TriggerEvent();
		}

		[FormerlySerializedAs("delegates")]
		[SerializeField]
		List<Entry> m_Delegates;


		public List<Entry> triggers {
			get {
				if (m_Delegates == null)
					m_Delegates = new List<Entry>();
				return m_Delegates;
			}
			set { m_Delegates = value; }
		}

		void Execute (EventTriggerType eventType, PointerEventData eventData) {
			foreach (var entry in triggers)
				if (entry.eventType == eventType && entry.callback != null)
					entry.callback.Invoke(eventData);
		}

		public virtual void OnPointerEnter (PointerEventData eventData) {
			Execute(EventTriggerType.PointerEnter, eventData);
		}

		public virtual void OnPointerExit (PointerEventData eventData) {
			Execute(EventTriggerType.PointerExit, eventData);
		}

		public virtual void OnDrag (PointerEventData eventData) {
			Execute(EventTriggerType.Drag, eventData);
		}

		public virtual void OnDrop (PointerEventData eventData) {
			Execute(EventTriggerType.Drop, eventData);
		}

		public virtual void OnPointerDown (PointerEventData eventData) {
			Execute(EventTriggerType.PointerDown, eventData);
		}

		public virtual void OnPointerUp (PointerEventData eventData) {
			Execute(EventTriggerType.PointerUp, eventData);
		}

		public virtual void OnPointerClick (PointerEventData eventData) {
			Execute(EventTriggerType.PointerClick, eventData);
		}

		public virtual void OnScroll (PointerEventData eventData) {
			Execute(EventTriggerType.Scroll, eventData);
		}

		public virtual void OnInitializePotentialDrag (PointerEventData eventData) {
			Execute(EventTriggerType.InitializePotentialDrag, eventData);
		}

		public virtual void OnBeginDrag (PointerEventData eventData) {
			Execute(EventTriggerType.BeginDrag, eventData);
		}

		public virtual void OnEndDrag (PointerEventData eventData) {
			Execute(EventTriggerType.EndDrag, eventData);
		}
	}
}