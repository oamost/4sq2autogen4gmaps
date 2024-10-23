using FsqAutogen;

class Program
{
    static async Task Main(string[] args)
    {
            // Process checkins
            //
            var checkins = new KmlGen();
            bool checkinsexported = await checkins.ProcessCheckins();

            if  (checkinsexported)
                Console.WriteLine("\nFinished with processing user checkins. ");

            Console.ReadKey();
    }
}
