namespace PracticeCSharp
{
    internal interface IChain
    {
        int Length {get;}
        IChain MakeChain(IChain chain1, IChain chain2, int n);
        bool IsCorrect();
        int Rating();
    } 
}