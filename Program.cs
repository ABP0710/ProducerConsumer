namespace ProducerConsumer
{


    internal class Program
    {
        static Queue<Item> _shopDisplay = new Queue<Item>();

        static void Main(string[] args)
        {

            Thread prod = new Thread(() => refillDisplay());
            prod.Start();

            Thread kunde = new Thread(() => GetItem());
            kunde.Start();


            prod.Join();
            kunde.Join();
        }

        public static Item GetItem()
        {
            while (true)
            {
                lock (_shopDisplay)
                {
                    try
                    {
                        while (_shopDisplay.Count == 0)
                        {
                            Console.WriteLine("kunden venter på vare");
                            Monitor.Wait(_shopDisplay);
                            Thread.Sleep(500);
                        }
                    }
                    finally
                    {
                        Console.WriteLine("Kunden har taget en vare");
                        Thread.Sleep(500);
                        _shopDisplay.Dequeue();
                    }
                }
            }
        }

        public static void refillDisplay()
        {
            while (true)
            {
                lock (_shopDisplay)
                {
                    try
                    {
                        if (_shopDisplay.Count < 3)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                Item item = new Item();
                                Thread.Sleep(500);
                                _shopDisplay.Enqueue(item);
                            }
                            Console.WriteLine("Medarbejderen har stillet 5 nye vare frem");
                            Monitor.PulseAll(_shopDisplay);
                        }
                    }
                    finally
                    {
                        Console.WriteLine("ellers andet?");
                        Thread.Sleep(500);
                    }
                }
            }
        }
    }
}

public class Item
{

}