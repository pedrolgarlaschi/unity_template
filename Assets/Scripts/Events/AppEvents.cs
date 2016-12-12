using UnityEngine;
using System.Collections;
using io.studiocomma.ui;

namespace io.studiocomma.events
{
	public abstract class AppEvent  {}


	public class UiEvent : AppEvent{

		public BaseUICanvas 				UiCanvas{ private set; get;}
		public BaseUICanvas.CanvasState 	State{ private set; get;}

		public UiEvent(BaseUICanvas canvas ,BaseUICanvas.CanvasState state)
		{
			UiCanvas = canvas;
			State = state;
		}
	}
		
}

