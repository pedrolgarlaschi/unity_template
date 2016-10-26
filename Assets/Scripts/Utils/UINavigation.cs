using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Coesia.events;

namespace com.pedroleonardo.ui
{
	public class UINavigation : MonoBehaviour {

		private static UINavigation instance;
		public static UINavigation Instance{
			get{

				if(instance == null){
					
					m_canvas = Resources.LoadAll<BaseUICanvas>("UI");
				
					m_listOfCanvas = new List<string>();

					GameObject go = new GameObject();
					
					go.name = "UINavigation";
					instance = go.AddComponent<UINavigation>();

					GameObject controllers =  GameObject.Find("controlers") as GameObject;

					foreach(BaseUICanvas c in m_canvas)
					{
						if(Screen.width >= 2048)
							c.GetScaler().matchWidthOrHeight = 1;
						else
							c.GetScaler().matchWidthOrHeight = 0;
					}

					
					if(controllers != null)
						go.transform.SetParent(controllers.transform);

				}


				return instance;
			}
			set{instance = value;}
		}

		private static BaseUICanvas[] m_canvas;
		private static List<string> m_listOfCanvas;
		private bool m_transition;	
		private bool m_inTransition;

		private BaseUICanvas m_nextCanvas;
		private BaseUICanvas m_ref;
		private System.Object m_nextCanvasParams;
		private BaseUICanvas m_uiCanvas;
		public BaseUICanvas MyUICanvas
		{
			get{return m_uiCanvas;}
		}


		void Awake()
		{
			if(!instance)
				DontDestroyOnLoad(gameObject);


			EventManager.Instance.AddListener<UiEvent> (UiEventHandler);
		}

		private void UiEventHandler(UiEvent evt)
		{
			if (evt.State == BaseUICanvas.CanvasState.close)
				OnUICanvasClose (evt.UiCanvas);
			else if (evt.State == BaseUICanvas.CanvasState.open)
				OnUICanvasOpen (evt.UiCanvas);
		}


		public string LastCanvasName
		{
			get{

				if(m_listOfCanvas.Count > 1)
					return m_listOfCanvas[m_listOfCanvas.Count - 2];
				else
					return "";
			}
		}

		void OnLevelWasLoaded(int level) {

			m_uiCanvas = null;
			m_nextCanvas = null;
		}

		public BaseUICanvas OpenUICanvas<T>(string name , System.Object param){
			
			if (m_uiCanvas != null && m_uiCanvas.Name == name)
				return m_uiCanvas;

			if (m_transition && m_ref != null && m_ref.Name == name && m_uiCanvas != null)
				return m_uiCanvas;

			if (m_transition){
			
				if(m_nextCanvas != null)
				{
					Destroy(m_nextCanvas.gameObject);
					m_nextCanvas  = null;
				}

				if(m_uiCanvas != null)
				{
					Destroy(m_uiCanvas.gameObject);
					m_uiCanvas = null;
				}
			}

			m_transition = true;

			m_ref = GetUICanvasRefByName (name);
				
			if (m_uiCanvas != null){
				
				m_nextCanvas = Instantiate (m_ref) as BaseUICanvas;
				m_nextCanvas.GetCanvas().enabled = false;
				m_nextCanvasParams = param;

				m_uiCanvas.Close();


				return m_nextCanvas;
			}
			else if(m_nextCanvas == null)
				m_nextCanvas = Instantiate (m_ref) as BaseUICanvas;


			m_uiCanvas = m_nextCanvas;
			
			m_uiCanvas.GetCanvas().enabled = true;

			m_nextCanvasParams = null;

			m_uiCanvas.Init ();
			m_uiCanvas.onUICanvasClose = OnUICanvasClose;
			m_uiCanvas.onUICanvasOpen = OnUICanvasOpen;


			m_uiCanvas.name = m_ref.name;


			m_listOfCanvas.Add (m_uiCanvas.Name);
			m_uiCanvas.Open (param);

			m_nextCanvas = null;

			return m_uiCanvas;

		}


		public void CloseUICanvas(){
			if (m_uiCanvas == null)
				return;

			m_transition = true;

			m_uiCanvas.Close ();
		}

		private void OnUICanvasOpen(BaseUICanvas uiCanvas){
			m_transition = false;
		}
		
		private void OnUICanvasClose(BaseUICanvas uiCanvas){	

			Destroy (m_uiCanvas.gameObject);
			m_uiCanvas = null;

			m_transition = false;

			if (m_nextCanvas != null) {
			
				m_nextCanvas.GetCanvas().enabled = true;

				OpenUICanvas <BaseUICanvas>(m_nextCanvas.Name, m_nextCanvasParams);
			}
		}

		private BaseUICanvas GetUICanvasRefByName(string name){
			foreach (BaseUICanvas c in m_canvas) 
			{
				if(c.Name == name)
					return c;
			}

			return null;
		}


	}
}