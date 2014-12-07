using System.Drawing;
using Wexman.Design;

namespace Example
{
    public class Red : DefaultProvider<Color>
    {
        public override Color GetDefault(DefaultUsage usage)
        {
            // we want red as default color!
            return Color.Red;
        }
    }
}
