using System.Diagnostics;

namespace SmartHomeProject
{
    internal class Lights
    {
        private SmartHomeTimer timer;
        private bool lightsEnabled;
        private int lightsStart;
        private int lightsEnd;

        public Lights(SmartHomeTimer timer)
        {
            this.timer = timer;

            lightsStart = Convert.ToInt32(DataBaseInitializer.GetColumnValue("AutoLightsStart"));
            lightsEnd = Convert.ToInt32(DataBaseInitializer.GetColumnValue("AutoLightsEnd"));

        }

        public bool IsLightsEnabled()
        {
            int hour = timer.SimulatedTime.Hour;

      

            if (lightsStart <= lightsEnd)
            {
               
                lightsEnabled = hour >= lightsStart && hour <= lightsEnd;
            }
            else
            {
                
                lightsEnabled = hour >= lightsStart || hour <= lightsEnd;
            }

       
            return lightsEnabled;
        }
    }
}