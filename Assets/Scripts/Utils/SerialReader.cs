using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Threading;
using System.IO.Ports;
using System.IO;

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
			GetPortNames ();
			Connect (guessPortName());
		}

		// Static Helpers	
		private string guessPortName()
		{		
			switch (Application.platform)
			{
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXDashboardPlayer:
			case RuntimePlatform.LinuxPlayer:
				return guessPortNameUnix();

			default: 
				return guessPortNameWindows();
			}

			//return guessPortNameUnix();
		}

		private string guessPortNameWindows()
		{
			var devices = System.IO.Ports.SerialPort.GetPortNames();

			if (devices.Length == 0) // 
			{
				return "COM3"; // probably right 50% of the time		
			} else
				return devices[0];				
		}

		private string guessPortNameUnix()
		{			
			var devices = System.IO.Ports.SerialPort.GetPortNames();

			if (devices.Length ==0) // try manual enumeration
			{
				devices = System.IO.Directory.GetFiles("/dev/");		
			}
			string dev = ""; ;			
			foreach (var d in devices)
			{				
				if (d.StartsWith("/dev/tty.usb") || d.StartsWith("/dev/ttyUSB"))
				{
					dev = d;
					//Debug.Log("Guessing that arduino is device " + dev);
					break;
				}
			}		
			return dev;		
		}


		private string[] GetPortNames ()
		{
			int p = (int)Environment.OSVersion.Platform;
			List<string> serial_ports = new List<string> ();

			// Are we on Unix?
			if (p == 4 || p == 128 || p == 6) {
				string[] ttys = Directory.GetFiles ("/dev/", "tty.*");
				foreach (string dev in ttys) {
					if (dev.StartsWith ("/dev/tty.*"))
						serial_ports.Add (dev);
					//Debug.Log (String.Format (dev));
				}
			}

			return serial_ports.ToArray ();
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
				Debug.Log ("error");
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

		public void Disconnect()
		{
			m_serial.Close ();
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
