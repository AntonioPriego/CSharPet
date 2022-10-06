
namespace CSharPet
{
    internal class MainProgram
    {
        static void Main(string[] args)
        {
            Cat pet = new();

            Console.WriteLine("WELCOME TO C#Pet");
            Console.WriteLine("Press enter to begin");
            Console.ReadLine();
            Console.Clear();

            pet.Play();

            Console.WriteLine("YOUR C#Pet IS NOT ALIVE ANYMORE :_(");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }

}
