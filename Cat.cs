// Timer Stuff from: https://learn.microsoft.com/es-es/dotnet/api/system.timers.timer?view=net-6.0

using System.Timers;
using System.IO;
using System.Diagnostics;


namespace CSharPet
{
    // The only one C#Pet at this time.
    public class Cat
    {
        // Timers.
        private static System.Timers.Timer freqTimer;
        private static System.Timers.Timer wearTimer;
        private static bool newInfoToShow  = true;
        private static bool newWearToApply = false;

        // Properties.
        private string command = string.Empty;
        CSharPetStatus currentStatus = CSharPetStatus.Regular;

        private int food; 
        public int Food // Food filling status.
        {
            get => food;
            set
            {
                if (value >= MyConstants.MINVALUE && value <= MyConstants.MAXVALUE)
                    food = value;
                else if (value < MyConstants.MINVALUE)
                    food = 0;
                else
                    food = MyConstants.MAXVALUE;
            }
        }
        private int clean; 
        public int Clean // Clean filling status.
        {
            get => clean;
            set
            {
                if (value >= MyConstants.MINVALUE && value <= MyConstants.MAXVALUE)
                    clean = value;
                else if (value < MyConstants.MINVALUE)
                    clean = 0;
                else
                clean = MyConstants.MAXVALUE;
            }
        }
        private int happy; 
        public int Happy // Happy filling status.
        {
            get => happy;
            set
            {
                if (value >= MyConstants.MINVALUE && value <= MyConstants.MAXVALUE)
                    happy = value;
                else if (value < MyConstants.MINVALUE)
                    happy = 0;
                else
                    happy = MyConstants.MAXVALUE;
            }
        }

        // Methods.

        // The alive loop for the C#Pet.
        public void Play()
        {
            // Set some initial parametres
            Food = 8;
            Clean = 5;
            Happy = 3;
            SetTimers();

            /*
            CODE TO IMPLEMENT COMMAND PLAY
            (With the C#Pet)(Probably a simple snake game).

            var proc = Process.Start("./HolaMundo.exe");
            proc.WaitForExit();
            var exitCode = proc.ExitCode;
            Console.WriteLine(exitCode);
            Console.ReadLine();
            */

            while (IsAlive())
            {
                if (newWearToApply)
                {
                    ApplyWear();
                    newWearToApply = false;
                }

                if (newInfoToShow)
                {
                    RunCommand();

                    PrintAll();

                    TryReadCommand();
                }
            }

            // Print a last time to show why the C#Pet has died.
            currentStatus = CSharPetStatus.Dead;
            PrintAll();
            // Stop the timer before exit from the alive loop.
            StopTimers();
        }

        // Print a visual status for the property indicated
        private void PrintPropStatus(string prop)
        {
            int propCount;
            
            switch (prop)
            {
                case ("Food"):
                    propCount = Food;
                    break;
                case ("Clean"):
                    propCount = Clean;
                    break;
                case ("Happy"):
                    propCount = Happy;
                    break;
                default:
                    propCount = 0;
                    break;
            }

            Console.Write("\t|\t\t\t\t" + prop + "\t");

            for (int i = 0; i < MyConstants.MAXVALUE; i++)
            {
                Console.Write((propCount > i) ? "▓" : "░");
            }
            Console.Write("\t\t\t\t  |");
            Console.WriteLine();
        }

        // Print the cat depending on the current status
        private void PrintCat()
        {
            Console.WriteLine("\t--------------------------------------- -------------------------------------------");
            Console.WriteLine("\t| ,_     _                             |                                          |");
            Console.WriteLine("\t| |\\_,-~/                              |    YOU CAN INTERACT WITH ME USING THIS:  |");
            if (currentStatus == CSharPetStatus.Happy)
                Console.WriteLine("\t| / _  _ |    ,--.       ♥   ♥      ♥  | >> 1 | Feed  | feed                      |");
            else if (currentStatus == CSharPetStatus.Dead)
                Console.WriteLine("\t| / -  - |         _._     _._     _._ | >> 1 | Feed  | feed                      |");
            else
                Console.WriteLine("\t| / _  _ |    ,--.                     | >> 1 | Feed  | feed                      |");
            if (currentStatus == CSharPetStatus.Happy)
                Console.WriteLine("\t|(  ~  ~ )   / ,-'    ♥      ♥  ♥      | >> 2 | Clean | clean                     |");
            else if (currentStatus == CSharPetStatus.Dead)
                Console.WriteLine("\t|(  *  * )           _._        _._    | >> 2 | Clean | clean                     |");
            else
                Console.WriteLine("\t|(  @  @ )   / ,-'                     | >> 2 | Clean | clean                     |");
            if (currentStatus == CSharPetStatus.Happy)
                Console.WriteLine("\t| \\  _t_/-._( (          ♥ ♥     ♥     | >> 3 | Pet   | pet                       |");
            else if (currentStatus == CSharPetStatus.Dead)
                Console.WriteLine("\t| \\  _T_/-._,-.            _._   _._   | >> 3 | Pet   | pet                       |");
            else
                Console.WriteLine("\t| \\  _T_/-._( (                        | >> 3 | Pet   | pet                       |");
            Console.WriteLine("\t| /         `. \\                       |                                          |");
            Console.WriteLine("\t| |        _  \\ |                      |                                          |");
            Console.WriteLine("\t| \\ \\ ,  /      |                      |                                          |");
            Console.WriteLine("\t|  || |-_\\__   /                       |                                          |");
            Console.WriteLine("\t| ((_/`(____,-'                        |                                          |");
            Console.WriteLine("\t--------------------------------------- -------------------------------------------");

            currentStatus = CSharPetStatus.Regular;
        }

        // Return alive status. Dead when any status property reaches to 0.
        private bool IsAlive()
        {
            return (Food > 0 && Clean > 0 && Happy > 0);
        }

        // Manage the all on screen info
        private void PrintAll()
        {
            Console.Clear();
            PrintCat();
            PrintPropStatus("Food");
            PrintPropStatus("Clean");
            PrintPropStatus("Happy");
            Console.WriteLine("\t-----------------------------------------------------------------------------------");
            Console.Write(">> ");

            newInfoToShow = false;
        }

        // Initial config for the timers.
        private static void SetTimers()
        {
            // Create timers with their intervals.
            freqTimer = new System.Timers.Timer(MyConstants.REFRESH_MSECS);
            wearTimer = new System.Timers.Timer(MyConstants.REFRESH_MSECS);

            // Hook up the Elapsed event for each timer. 
            freqTimer.Elapsed   += OnTimedFreqEvent;
            freqTimer.AutoReset =  true;
            freqTimer.Enabled   =  true;
            wearTimer.Elapsed   += OnTimedWearEvent;
            wearTimer.AutoReset =  true;
            wearTimer.Enabled   =  true;

        }

        // Closing timers.
        private static void StopTimers()
        {
            freqTimer.Stop();
            freqTimer.Dispose();
            wearTimer.Stop();
            wearTimer.Dispose();
        }

        // Event when refresh screen timer is activated.
        private static void OnTimedFreqEvent(Object source, ElapsedEventArgs e)
        {
            newInfoToShow = true;
        }

        // Event when C#Sharp wear timer is activated.
        private static void OnTimedWearEvent(Object source, ElapsedEventArgs e)
        {
            newWearToApply = true;
        }

        // Read command from user considering refresh screen.
        private async Task TryReadCommand()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromSeconds(MyConstants.REFRESH_SECS));
            Task<int> task = (Task<int>)Task.Run(() => ReadCommand(), source.Token);
        }

        // Reading command from user event.
        private async Task ReadCommand()
        {
            command = await Console.In.ReadLineAsync();
            newInfoToShow = true;
        }

        // Apply command readed
        private void RunCommand()
        {
            Random random = new Random();

            switch (command)
            {
                case "1": case "Feed": case "feed":
                    Feed();
                    break;
                case "2": case "CleanUp": case "cleanup":
                    CleanUp();
                    break;
                case "3": case "Pet": case "pet":
                    Pet();
                    break;
                default:
                    break;
            }

            command = string.Empty;
        }

        // Feed C#Pet with random results.
        private void Feed()
        {
            Random random = new Random();

            Food += random.Next(1, 3);
        }

        // Clean C#Pet with random results.
        private void CleanUp()
        {
            Random random = new Random();

            Clean += random.Next(1, 3);
        }

        // Pet C#Pet with random results.
        private void Pet()
        {
            Random random = new Random();

            Happy += random.Next(1, 3);

            currentStatus = CSharPetStatus.Happy;
        }

        // Apply the wear to the status property.
        private void ApplyWear()
        {
            Random random = new Random();

            Food  -= random.Next(1, 4);
            Clean -= random.Next(1, 4);
            Happy -= random.Next(1, 4);
        }
    }
}
