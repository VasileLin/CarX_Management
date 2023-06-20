using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarX.Classes
{
    public class RoundControls
    {
        public Region CreateRoundedRegion(Rectangle bounds, int cornerRadius)
        {
            GraphicsPath roundedPath = new GraphicsPath();
            roundedPath.AddArc(bounds.X, bounds.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedPath.AddArc(bounds.Right - cornerRadius * 2, bounds.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedPath.AddArc(bounds.Right - cornerRadius * 2, bounds.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedPath.AddArc(bounds.X, bounds.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedPath.CloseFigure();

            return new Region(roundedPath);
        }
    }
}
