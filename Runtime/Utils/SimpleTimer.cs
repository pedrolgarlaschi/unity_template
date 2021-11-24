using UnityEngine;
using System.Collections;

public class SimpleTimer : MonoBehaviour {

    private static int timerId;
    public delegate void OnTimerCompleteDelegate();
    public delegate void OnTimerUpdateDelegate(float time);

    public static SimpleTimer StartTimer(float time, OnTimerCompleteDelegate onComplete, OnTimerUpdateDelegate onUpdate = null , bool autoStart = true)
    {
        timerId++;
        SimpleTimer timer = new GameObject().AddComponent<SimpleTimer>();
        timer.name = "SimpleTimer_" + timerId;

        timer.Init(time, onComplete , onUpdate);

        timer.DestroyOnComplete = true;

        if(autoStart)
            timer.StartTimer();
            

        return timer;
    }

    private float                       m_startTime;
    private float                       m_time;
    private float                       m_cTime;
    private OnTimerCompleteDelegate     m_completeHandler;
    private OnTimerUpdateDelegate       m_onUpdate;


    public float CurrentTime
    {
        get { return m_cTime; }
    }
    


    public bool DestroyOnComplete { set; get; }
    public bool IsRunning { private set; get; }
    public bool Loop { set; get; }

    public void Init(float time, OnTimerCompleteDelegate onComplete , OnTimerUpdateDelegate onUpdate)
    {
        m_time = time;
        m_completeHandler = onComplete;
        m_onUpdate = onUpdate;

        IsRunning = false;
    }

    public void StartTimer()
    {
        m_startTime = Time.time;
        m_cTime = m_time;
        IsRunning = true;
    }

    public void StopTimer()
    {
        m_cTime = m_time;
        IsRunning = false;

        if (Loop)
            StartTimer();
    }

    void Update()
    {
        if (!IsRunning) return;

        m_cTime =  (m_startTime + m_time) - Time.time;

        if (m_cTime <= 0)
        {
            if(m_completeHandler != null)
            {
                if (m_onUpdate != null)
                    m_onUpdate(m_cTime);
                m_completeHandler();
                StopTimer();
            }

            if(DestroyOnComplete)
                Destroy(gameObject);

            return;
        }


        if (m_onUpdate != null)
            m_onUpdate(m_cTime);


    }


}
