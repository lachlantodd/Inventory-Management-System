using System;

namespace InventoryManagement
{
    class Tool : IComparable
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Quantity { get; set; }
        public Tool FirstChild { get; set; } = null;
        public Tool NextSibling { get; set; } = null;

        public Tool(string name, string description, int quantity)
        {
            Name = name;
            Description = description;
            Quantity = quantity;
        }

        public void AddTool(Tool newTool)
        {
            // If there are currently no tools in this category, insert newTool and exit the method
            if (this.FirstChild == null)
            {
                this.FirstChild = newTool;
                return;
            }
            Tool prevTool = null;
            Tool tool = this.FirstChild;

            // For all tools in the category
            do
            {
                // If the newTool name comes alphabetically before the current tool, insert here
                int result = newTool.CompareTo(tool);
                if (result < 0)
                {
                    newTool.NextSibling = tool;

                    // If we are dealing with the first tool in the category
                    if (this.FirstChild == tool)
                        this.FirstChild = newTool;
                    else
                        prevTool.NextSibling = newTool;
                    return;
                }
                // Else if the newTool and current tool are identical, increase the quantity by 1 and exit the method
                else if (result == 0)
                {
                    ++tool.Quantity;
                    return;
                }
                // Else, we move onto the next tool or exit the loop if one doesn't exist
                prevTool = tool;
                tool = tool.NextSibling;
            } while (tool != null);

            // If we have reached this point, prevTool.NextSibling is null as we are at the end of the category
            prevTool.NextSibling = newTool;
        }

        public void RentTool(Tool tool)
        {

        }

        // Allows for the new tool to be inserted alphabetically by name
        public int CompareTo(object obj)
        {
            Tool other = (Tool)obj;
            return this.Name.CompareTo(other.Name);
        }
    }
}
