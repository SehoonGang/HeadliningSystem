using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadliningSystem.Services
{
    public class CameraService
    {
        private bool _isConnected = false;
        public bool IsCameraConnected
        {
            get => _isConnected;
            private set => _isConnected = value;
        }
    }
}
