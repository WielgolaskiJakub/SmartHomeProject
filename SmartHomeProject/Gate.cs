namespace SmartHomeProject
{
    internal class Gate
    {
        private bool gate;

        public Gate(bool gate)
        {
            this.gate = gate;
        }

        public bool isGate()
        {
            if (gate)
            {
                Console.WriteLine("Brama jest otwarta");
            }
            else
            {
                Console.WriteLine("Brama jest zamknięta");
            }

            return gate;
        }
    }
}





