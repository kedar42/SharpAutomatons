namespace SharpAutomatons;

static class Program
{
    public static int Main()
    {
        Automaton test;
        
        
        var builder1 = new AutomatonBuilder("Test", "S");
        builder1.AddTransition("S", "a", "S");
        builder1.AddFinalState("S");
        test = builder1.Build();
        
        var builder2 = new AutomatonBuilder("Test", "S");
        builder2.AddTransition("S", "a", "A");
        builder2.AddTransition("A", "a", "A");
        builder2.AddFinalState("A");
        test = builder2.Build();
        
        var builder3 = new AutomatonBuilder("Test", "S");
        builder3.AddTransition("S", "a", "A");
        builder3.AddTransition("A", "b", "S");
        builder3.AddTransition("S", "b", "S");
        builder3.AddFinalState("A");
        test = builder3.Build();

        var builder4 = new AutomatonBuilder("Test", "S");
        builder4.AddTransition("S", "a", "A");
        builder4.AddTransition("S", "b", "B");
        builder4.AddTransition("A", "b", "B");
        builder4.AddTransition("B", "b", "B");
        builder4.AddFinalState("B");
        builder4.AddFinalState("A");
        test = builder4.Build();
        
        var builderNondeterministic = new AutomatonBuilder("Test", "S");
        builderNondeterministic.AddTransition("S", "a", "A");
        builderNondeterministic.AddTransition("S", "a", "B");
        builderNondeterministic.AddTransition("A", "b", "C");
        builderNondeterministic.AddTransition("B", "b", "C");
        builderNondeterministic.AddFinalState("C");
        test = builderNondeterministic.Build();
        
        var builderNondeterministic2 = new AutomatonBuilder("Test", "S");
        builderNondeterministic2.AddTransition("S", "a", "A");
        builderNondeterministic2.AddTransition("S", "a", "B");
        builderNondeterministic2.AddTransition("A", "b", "C");
        builderNondeterministic2.AddTransition("B", "b", "C");
        builderNondeterministic2.AddTransition("B", "b", "B");
        builderNondeterministic2.AddFinalState("C");
        test = builderNondeterministic2.Build();
        
        var redundant = new AutomatonBuilder("Test", "S");
        redundant.AddTransition("S", "a", "A");
        redundant.AddTransition("S", "a", "B");
        redundant.AddTransition("A", "b", "C");
        test = redundant.Build();
        
        var harderRedundant = new AutomatonBuilder("Test", "S");
        harderRedundant.AddTransition("S", "a", "A");
        harderRedundant.AddTransition("S", "a", "B");
        harderRedundant.AddTransition("A", "b", "C");
        harderRedundant.AddFinalState("C");
        test = harderRedundant.Build();

        return 0;
    }
}