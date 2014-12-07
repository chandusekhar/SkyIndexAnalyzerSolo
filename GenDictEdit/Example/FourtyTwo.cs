using Wexman.Design;

namespace Example
{
    public class FourtyTwo : DefaultProvider<int>
    {
        public override int GetDefault(DefaultUsage usage)
        {
            // The mother of all numbers
            return 42;
        }
    }
}
