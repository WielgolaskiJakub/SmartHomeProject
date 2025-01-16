using System.Diagnostics;
using System.Timers;

namespace SmartHomeProject
{
    internal class SmartHomeTimer
    {
        private System.Timers.Timer timer;
        private DateTime simulatedTime;

        public SmartHomeTimer()
        {

            simulatedTime = DateTime.Now;

            //ustawianie pętli do wyświetlania timera
            timer = new System.Timers.Timer(1000.0);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //dodawanie 1h co sekunde w symulowanym timerze
            simulatedTime = simulatedTime.AddHours(1);
            
        }
        // udostępnienie Timera
        public DateTime SimulatedTime
        {
            get { return simulatedTime; }
        }

        public void Stop()
        {
            timer.Stop();
        }
        public void Start()
        {
            timer.Start();
        }


    }
}