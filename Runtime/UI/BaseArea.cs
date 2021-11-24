using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.pedrogarlaschi.UI
{

	public abstract class BaseArea : MonoBehaviour
	{

		public event UINavigation.OnNavigationEventDelegate OnNavigationEven;

		[SerializeField]
		private string areaName;
		public string AreaName
		{

			get
			{
				return areaName;
			}

		}

		protected Dictionary<Transform, Vector3> startPositions;
		protected Dictionary<Transform, Vector3> startRotations;


		public virtual void Init(ArrayList param)
		{
			startPositions = new Dictionary<Transform, Vector3>();
			startRotations = new Dictionary<Transform, Vector3>();

			Transform[] arr = GetComponentsInChildren<Transform>();
            foreach(Transform t in arr)
            {
				startPositions.Add(t, t.localPosition);
				startRotations.Add(t, t.localEulerAngles);
			}
		}

		public virtual void Open()
		{

			if (OnNavigationEven != null)
				OnNavigationEven(UINavigation.AreaState.startOpen, this);

			OnOpenEnded();

		}

		protected virtual void OnOpenEnded()
		{

			if (OnNavigationEven != null)
				OnNavigationEven(UINavigation.AreaState.endOpen, this);
		}

		public virtual void Close()
		{

			if (OnNavigationEven != null)
				OnNavigationEven(UINavigation.AreaState.startClose, this);

			OnCloseEnded();
		}

		protected virtual void OnCloseEnded()
		{

			if (OnNavigationEven != null)
				OnNavigationEven(UINavigation.AreaState.endClose, this);
		}

	}
}