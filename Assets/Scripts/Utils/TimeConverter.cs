using UnityEngine;
using System.Collections;

public class TimeConverter  {

	public enum ConvertTimeType
	{
		hmsc,
		hms,
		msc,
		ms,
		sc,
		s
	}

	public static string ConvertTime(float time, ConvertTimeType format)
	{

		int horas = Mathf.FloorToInt(time/3600);
		int minutos = Mathf.FloorToInt((time - horas * 3600) / 60);
		int segundos = Mathf.FloorToInt((time - horas * 3600 - minutos * 60));
		float centesimos = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100);
		string reth=horas.ToString();
		string retm = minutos.ToString();
		string rets = segundos.ToString();
		string retc = centesimos.ToString();

		if (horas<10) reth="0"+reth;
		if (minutos<10) retm="0"+retm;
		if (segundos<10) rets="0"+rets;
		if (centesimos<10) retc="0"+retc;

		string ret="";

		if (format == ConvertTimeType.hmsc) ret = reth + ":" + retm + ":" + rets + ":" + retc;
		else if (format == ConvertTimeType.hms) ret = reth + ":" + retm + ":" + rets;
		else if (format == ConvertTimeType.msc) ret = retm + ":" + rets + ":" + retc;
		else if (format == ConvertTimeType.ms) ret = retm + ":" + rets;
		else if (format == ConvertTimeType.sc) ret = rets + ":" + retc;
		else if (format == ConvertTimeType.s) ret = rets;
		return ret;
	}



}
