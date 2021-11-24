using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace com.pedrogarlaschi.UI
{
	public class UINavigation : MonoBehaviour
	{

		public enum AreaState
		{
			startOpen,
			endOpen,
			startClose,
			endClose
		}

		public delegate void OnNavigationEventDelegate(AreaState state, BaseArea area);

		public class NavigationObject
		{

			public string AreaName { private set; get; }
			public ArrayList Params;


			public NavigationObject(string areaName)
			{
				AreaName = areaName;
				Params = new ArrayList();
			}

			public void AddParam<T>(T o)
			{

				Params.Add(o);
			}

		}

		private static UINavigation instance;
		public static UINavigation Instance
		{

			get
			{
				if (instance != null)
					return instance;

				history = new List<NavigationObject>();
				nextAreas = new List<NavigationObject>();
				Areas = Resources.LoadAll<BaseArea>("UI/");


				areaTarget = GameObject.Find("areaTarget").GetComponent<Image>();

				GameObject go = new GameObject("UINavigation");
				instance = go.AddComponent<UINavigation>();

				foreach (BaseArea a in GameObject.FindObjectsOfType<BaseArea>())
					GameObject.Destroy(a.gameObject);


				return instance;
			}
		}

		private static BaseArea[] Areas;

		public static event OnNavigationEventDelegate OnNavigationEvent;

		private static List<NavigationObject> history;
		private static List<NavigationObject> nextAreas;
		private static Image areaTarget;

		public static BaseArea CurrentArea { private set; get; }
		public static BaseArea NextArea { private set; get; }

		public void Open(NavigationObject obj)
		{

			if (CurrentArea != null && CurrentArea.AreaName == obj.AreaName)
				return;

			if (CurrentArea != null)
			{

				nextAreas.Add(obj);

				NextArea = GameObject.Instantiate(GetAreaReferenceFromName(obj.AreaName));

				NextArea.Init(obj.Params);

				Close();
				return;
			}

			history.Add(obj);


			if (NextArea != null)
				CurrentArea = NextArea;
			else
			{
				CurrentArea = GameObject.Instantiate(GetAreaReferenceFromName(obj.AreaName));
				CurrentArea.Init(obj.Params);
			}

			NextArea = null;

			//SimpleTimer.StartTimer (0.1f, delegate() {

			CurrentArea.transform.SetParent(areaTarget.transform);
			CurrentArea.transform.position = Vector3.zero;
			
			CurrentArea.GetComponent<Image>().rectTransform.offsetMin = new Vector2(0, 0);
			CurrentArea.GetComponent<Image>().rectTransform.offsetMax = new Vector2(0, 0);
			CurrentArea.OnNavigationEven += CurrentArea_OnNavigationEven;

			CurrentArea.transform.localScale = Vector3.one;

			CurrentArea.Open();
			//});


		}

		void CurrentArea_OnNavigationEven(AreaState state, BaseArea area)
		{

			switch (state)
			{

				case AreaState.startOpen:

					break;
				case AreaState.endOpen:

					break;
				case AreaState.startClose:

					break;
				case AreaState.endClose:

					CurrentArea.OnNavigationEven -= CurrentArea_OnNavigationEven;
					Destroy(CurrentArea.gameObject);

					CurrentArea = null;

					if (nextAreas.Count > 0)
					{
						NavigationObject next = nextAreas[0];
						nextAreas.RemoveAt(0);
						Open(next);
					}
					break;

			}

			if (OnNavigationEvent != null)
				OnNavigationEvent(state, area);

		}

		public void Back()
		{

			if (history.Count < 1)
			{
				Debug.LogWarning("There's no history to get back");
				return;
			}

			Open(history[history.Count - 2]);
		}

		public BaseArea GetAreaReferenceFromName(string areaName)
		{

			foreach (BaseArea a in Areas)
			{

				if (areaName == a.AreaName)
					return a;
			}

			return null;
		}


		public void Close()
		{
			CurrentArea.Close();
		}

	}
}