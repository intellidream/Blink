using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.Sterling.Core.Exceptions;

namespace Blink.Shared.Engine.SkyDrive
{
    public class SterlingSkyDriveException : SterlingException 
    {
        public SterlingSkyDriveException(Exception ex)
            : base(string.Format("An exception occurred accessing SkyDrive: {0}", ex), ex)
        {
            
        }
    }
}
