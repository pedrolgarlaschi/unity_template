using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Uniduino;

using System;
using System.Threading;
using System.IO.Ports;

namespace com.pedroleonardo.serial
{
	public class SerialReader : MonoBehaviour {

		private static SerialReader instance;
		public static SerialReader 	Instance
		{
			get{
				if (instance)
					return instance;

				GameObject go = new GameObject ();
				go.name = "SerialReader";
				instance = go.AddComponent<SerialReader> ();

				m_listaners = new List<ISerialListener> ();

				return instance;
			}
		}

		private static List<ISerialListener> m_listaners;

		public static void RegisterListener(ISerialListener listener)
		{
			if (Instance) {}

			m_listaners.Add (listener);
		}


		public static void RemoveListener(ISerialListener listener)
		{
			if (Instance) {}

			m_listaners.Remove (listener);
		}
			
		public bool SerialConnected
		{
			get{

				if(m_serial == null)
					return false;

				return m_serial.IsOpen;

			}
		}

		private string 		m_message;
		private SerialPort 	m_serial;


		void Start()
		{
			Connect (Uniduino.Arduino.guessPortName ());	
		}

		public void Connect(string comName)
		{
			try
			{
				m_serial = new SerialPort (comName, 9600);
				m_serial.Open ();
				Thread readThread = new Thread(Read);
				readThread.Start();
			}
			catch {
			
			}
		}

		public void Read()
		{
			while (true)
			{
				try
				{
					m_message = m_serial.ReadLine();
				}
				catch (TimeoutException) 
				{
					Debug.Log("catch");

				}
			}
		}

		void OnApplicationQuit() {

			if(m_serial != null && m_serial.IsOpen)
				m_serial.Close ();
		}


		void Update()
		{
			if (m_message != "")
				BroadcastMessage ();
				
		}

		private void BroadcastMessage()
		{
			foreach (ISerialListener l in m_listaners)
				l.OnSerialMessage (m_message);

			m_message = "";
		}

	}


	public interface ISerialListener  {
		void OnSerialMessage (string msg);
	}
}
	