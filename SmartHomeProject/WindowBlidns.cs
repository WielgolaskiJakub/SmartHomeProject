namespace SmartHomeProject
{
    internal class WindowBlinds
    {
        private bool windowBlindEnabled;
        private int windowBlindsStart;
        private int windowBlindsEnd;
        SmartHomeTimer timer;
        public WindowBlinds(SmartHomeTimer timer)
        {
            this.timer = timer;

            windowBlindsStart = Convert.ToInt32(DataBaseInitializer.GetColumnValue("AutoBlindsStart"));
            windowBlindsEnd = Convert.ToInt32(DataBaseInitializer.GetColumnValue("AutoBlindsEnd"));

        }


        public bool IsWindowBlindsEnabled()
        {
            int hour = timer.SimulatedTime.Hour;



            if (windowBlindsStart <= windowBlindsEnd)
            {
                // Dzienny zakres (np. 7:00–18:00)
                windowBlindEnabled= hour >= windowBlindsStart && hour <= windowBlindsEnd;
            }
            else
            {
                // Nocny zakres (np. 18:00–7:00)
                windowBlindEnabled = hour >= windowBlindsStart || hour <= windowBlindsEnd;
            }


            return windowBlindEnabled;
        }

    }
    }





