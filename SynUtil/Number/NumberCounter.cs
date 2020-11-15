namespace SynUtil.Number
{
    public class NumberCounter
    {
        private int Ctr = 0;

        public NumberCounter()
        {
        }

        public int GetNext()
        {
            Ctr++;
            return Ctr - 1;
        }
    }
}