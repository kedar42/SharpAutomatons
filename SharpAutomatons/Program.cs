namespace SharpAutomatons;

static class Program
{

    public static int Main()
    {

        var automaton = new AutomatonBuilder("Test")
            .AddState("S")
            .AddState("A")
            .AddState("B")
            .AddTransition("S", "A", 'a')
            .AddTransition("S", "B", 'b')
            .AddTransition("A", "B", 'b')
            .AddTransition("B", "B", 'b')
            .MakeFinal("B")
            .InitialState("S")
            .Build();
        
        Console.WriteLine(automaton.Test("abb"));
        Console.WriteLine(automaton.Test("b"));
        Console.WriteLine(automaton.Test("a"));
        Console.WriteLine(automaton.Test("ba"));
        
        return 0;
    }
    
}

