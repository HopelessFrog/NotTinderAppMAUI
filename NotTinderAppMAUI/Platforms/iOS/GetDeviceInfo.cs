﻿using UIKit;

namespace DeviceDetails
{
    public partial class GetDeviceInfo
    {
        public partial string GetDeviceID()
        {
            string deviceID = UIDevice.CurrentDevice.IdentifierForVendor.ToString();
            return deviceID;
        }
    }
}