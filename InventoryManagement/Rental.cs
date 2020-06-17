using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagement
{
    class Rental
    {
        public string ToolName { get; private set; }
        public string ToolCategory { get; private set; }
        public string RenterName { get; private set; }
        public string ContactPhone { get; private set; }

        public Rental (string toolName, string toolCategory, string renterName, string phone)
        {
            ToolName = toolName;
            ToolCategory = toolCategory;
            RenterName = renterName;
            ContactPhone = phone;
        }
    }
}
