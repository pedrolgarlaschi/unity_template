using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using io.studiocomma.events;

namespace io.studiocomma.ui
{
	/// <summary>
	/// UI canvas event delegate
	/// </summary>
	public delegate void OnBaseUICanvasEvent(BaseUICanvas uiCanvas);

	/// <summary>
	/// Base class for all of the UI canvas, use to be an abstract class but i had some problems dispatching events from the Unity animtion
	/// </summary>
	public class BaseUICanvas : MonoBehaviour {


		///Current canvas state
		public enum CanvasState
		{
			opening,
			open,
			closing,
			close
		}

		public OnBaseUICanvasEvent onUICanvasOpen;
		public OnBaseUICanvasEvent onUICanvasClose;

		[SerializeField]
		protected string m_name;
		public string Name
		{
			get{return m_name;}
		}

		protected bool m_isOpen = false;
		public bool IsOpen
		{
			get{return m_isOpen;}
		}

		protected Canvas m_canvas;
		public Canvas GetCanvas()
		{
			if(m_canvas == null)
				m_canvas = gameObject.GetComponent<Canvas> ();

			return m_canvas;
		}

		protected CanvasState 	m_state;
		protected Animation 	m_animation;
		protected Text[] 		m_textFields; 
		protected System.Object m_params;
		public CanvasScaler GetScaler()
		{
			return gameObject.GetComponent<CanvasScaler> ();
		}


		/// <summary>
		/// I do not use the start or awake to init the canvas i call taht fundtion from the Navigation controller.
		/// </summary>
		public virtual void Init()
		{

			m_animation = gameObject.GetComponent<Animation> ();
			m_textFields = gameObject.GetComponentsInChildren<Text> ();
			m_state = CanvasState.close;

			if (m_animation != null) {
				m_animation.Stop ();
			}
		}


		void Update()
		{
			OnUpdate ();

			if (!m_animation)
				return;

			if (!m_animation.isPlaying && m_state == CanvasState.opening)
				OnAnimationIntroEnd ();
			else if (!m_animation.isPlaying && m_state == CanvasState.closing)
				OnAnimationEndEnd ();




		}

		protected virtual void OnUpdate()
		{
			
		}


		/// <summary>
		/// Open ui canvas
		/// </summary>
		/// <param name="param">Parameter.</param>
		public virtual void Open(System.Object param)
		{

			m_state = CanvasState.opening;

			EventManager.Instance.TriggerEvent (new UiEvent (this,m_state));

			if (m_animation != null) {
				
				m_animation.Play ("Intro");
			}
			else
				OnAnimationIntroEnd ();

		}
			
		/// <summary>
		/// Close UI canvas
		/// </summary>
		public virtual void Close()
		{
			m_state = CanvasState.closing;

			EventManager.Instance.TriggerEvent (new UiEvent (this,m_state));

			if (m_animation != null)
				m_animation.Play ("Outro");
			else
				OnAnimationEndEnd ();
		}

		/// <summary>
		/// Triggered when the animation intro ends
		/// </summary>
		public virtual void OnAnimationIntroEnd()
		{
			m_state = CanvasState.open;
			m_isOpen = true;

			EventManager.Instance.TriggerEvent (new UiEvent (this,m_state));

			//if(onUICanvasOpen != null)
			//	onUICanvasOpen (this);
		}

		/// <summary>
		/// Triggered when  the animation outro ends
		/// </summary>
		public virtual void OnAnimationEndEnd()
		{

			m_state = CanvasState.close;
			m_isOpen = false;


			EventManager.Instance.TriggerEvent (new UiEvent (this,m_state));
			//if(onUICanvasClose != null)
			//	onUICanvasClose (this);
		}
	
	}
}
