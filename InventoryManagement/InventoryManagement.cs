using System;
using static System.Console;

namespace InventoryManagement
{
    class Program
    {
        static Tool toolStorage;
        static Tool category;
        const int MAX_RENTALS = 100;
        static Rental[] rentals = new Rental[MAX_RENTALS];
        const string SEPARATOR = "--------------------------------------------------------------------------------";

        static void Main()
        {
            InitialiseProgram();
            RunProgram();
        }

        static void InitialiseProgram()
        {
            InitialiseToolStorage();
            InitialiseTools();
        }

        static void RunProgram()
        {
            while (true)
            {
                DisplayMainMenu();
            }
        }

        static void DisplayMainMenu()
        {
            Clear();

            // Main menu options
            WriteLine("Welcome to the Inventory Management System.");
            WriteLine();
            WriteLine("Please select an option below.");
            WriteLine(SEPARATOR);
            WriteLine("{0,-8}{1}", "1.", "View tools");
            WriteLine(SEPARATOR);
            WriteLine("{0,-8}{1}", "2.", "Add new tool");
            WriteLine(SEPARATOR);
            WriteLine("{0,-8}{1}", "3.", "Rent tool");
            WriteLine(SEPARATOR);
            WriteLine("{0,-8}{1}", "4.", "Return rented tool");
            WriteLine(SEPARATOR);
            WriteLine("{0,-8}{1}", "ESC.", "Exit");

            // Detecting the option selected by the user and sending them on their way
            ConsoleKeyInfo c = ReadKey();
            if (c.Key == ConsoleKey.D1 || c.Key == ConsoleKey.NumPad1)
                ViewCategories();
            else if (c.Key == ConsoleKey.D2 || c.Key == ConsoleKey.NumPad2)

                // Calling the method with an optional argument to visit the correct screen
                ViewCategories(2);
            else if (c.Key == ConsoleKey.D3 || c.Key == ConsoleKey.NumPad3)

                // Calling the method with an optional argument to visit the correct screen
                ViewCategories(3);
            else if (c.Key == ConsoleKey.D4 || c.Key == ConsoleKey.NumPad4)
                ReturnTool();
            else if (c.Key == ConsoleKey.Escape)

                // Ending the program
                Environment.Exit(0);
        }

        // Optional argument will send the user down different routes after this screen
        static void ViewCategories(int option = 1)
        {
            Clear(); 
            WriteLine("Please select an option below.");
            category = toolStorage.FirstChild;
            int counter = 1;

            // Writing each category with the corresponding key to press to select
            while (category != null)
            {
                WriteLine(SEPARATOR);
                WriteLine("{0,-8}{1}", counter, CapitaliseWords(category.Name) + " Tools");
                category = category.NextSibling;
                ++counter;
            }
            // Key 0 (and any other non-valid key) returns to the main menu
            WriteLine(SEPARATOR);
            WriteLine("{0,-8}{1}", "0", "Return");

            // Checking for a valid key option to pass to the next method
            ConsoleKeyInfo c = ReadKey();
            if (char.IsDigit(c.KeyChar))

                // If the user is trying to get back to the main menu
                if (int.Parse(c.KeyChar.ToString()) == 0)
                    return;
                else
                    switch (option)
                    {
                        case 1:
                            ViewTools(int.Parse(c.KeyChar.ToString()));
                            break;

                        case 2:
                            AddNewTool(int.Parse(c.KeyChar.ToString()));
                            break;

                        case 3:
                            RentTool(int.Parse(c.KeyChar.ToString()));
                            break;
                    }
        }

        // Helper method returns a string with the all words' first characters capitalised
        static string CapitaliseWords(string s)
        {
            string[] words = s.Split(' ');
            for (int i = 0; i < words.Length; ++i)
                if (words[i].Length > 1)
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                else if (words[i].Length == 1)
                    words[i] = words[i].ToUpper();
            return string.Join(' ', words);
        }

        // Printing all tools in the category to the screen
        static void ViewTools(int option)
        {
            Clear();
            GetCategory(option);
            WriteLine("{0} Tools", CapitaliseWords(category.Name));
            Tool tool = category.FirstChild;
            int counter = 1;
            while (tool != null)
            {
                WriteLine(SEPARATOR);
                WriteLine("{0,-8}{1,-15}{2}", "", "Name: ", tool.Name);
                WriteLine("{0,-8}{1,-15}{2}", counter, "Description: ", tool.Description);
                WriteLine("{0,-8}{1,-15}{2}", "", "Quantity: ", tool.Quantity);
                tool = tool.NextSibling;
                ++counter;
            }
            // Key 0 (and any other non-valid key) returns to the main menu
            WriteLine(SEPARATOR);
            WriteLine();
            WriteLine("{0,-8}{1}", "0", "Return");

            ReadKey();
        }

        // Setting the category variable to point at the correct tree node
        static void GetCategory(int option)
        {
            category = toolStorage.FirstChild;
            int counter = 1;
            while (category != null)
            {
                // If we reach the option the user selected, category will be set correctly
                if (counter == option)
                    return;
                category = category.NextSibling;
                ++counter;
            }
        }

        static void AddNewTool(int categoryOption)
        {
            Clear();
            GetCategory(categoryOption);
            Write("Please enter the name of the new {0} tool: ", category.Name);
            string newName = CapitaliseWords(ReadLine());
            WriteLine();

            Tool tool = category.FirstChild;
            int quantity = -1;
            while (tool != null)
            {
                // If the tool to add already exists in the inventory
                if (newName == tool.Name)
                {
                    WriteLine("This tool currently exists in the inventory.");
                    WriteLine();
                    do
                    {
                        Write("Please enter the quantity to add (0+): ");
                        if (int.TryParse(ReadLine(), out int input))
                            quantity = input;
                        WriteLine();
                    } while (quantity < 0);
                    // Providing an option for 0 in case the user made a mistake
                    if (quantity == 0)
                    {
                        WriteLine("Tool not added (quantity is zero)");
                        WriteLine("Please press 0 to return to the main menu");
                        ReadKey();
                        return;
                    }
                    break;
                }
                tool = tool.NextSibling;
            }
            // If the tool doesn't currently exist in the system
            if (quantity == -1)
            {
                Write("Please enter a brief description for this tool: ");
                string newDescription = ReadLine();
                WriteLine();
                do
                {
                    Write("Please enter a quantity for this tool (0+): ");
                    if (int.TryParse(ReadLine(), out int input))
                        quantity = input;
                    WriteLine();
                } while (quantity < 0);
                // Providing an option for 0 in case the user made a mistake
                if (quantity == 0)
                {
                    WriteLine("Tool not added (quantity is zero)");
                    WriteLine("Please press 0 to return to the main menu");
                    ReadKey();
                    return;
                }
                category.AddTool(new Tool(newName, newDescription, quantity));
            }
            else
            {
                tool.Quantity += quantity;
            }
            WriteLine("Tool added successfuly.");
            WriteLine("Please press 0 to return to the main menu");
            ReadKey();
        }

        static void RentTool(int categoryOption)
        {
            Clear();
            GetCategory(categoryOption);
            WriteLine("{0} Tools", CapitaliseWords(category.Name));
            Tool tool = category.FirstChild;
            int counter = 0;
            while (tool != null)
            {
                ++counter;
                WriteLine(SEPARATOR);
                WriteLine("{0,-8}{1,-15}{2}", "", "Name: ", tool.Name);
                WriteLine("{0,-8}{1,-15}{2}", counter, "Description: ", tool.Description);
                WriteLine("{0,-8}{1,-15}{2}", "", "Quantity: ", tool.Quantity);
                tool = tool.NextSibling;
            }
            WriteLine(SEPARATOR);
            WriteLine();
            WriteLine("{0,-8}{1}", "0", "Return");
            WriteLine();
            int id = -1;
            do
            {
                Write("Please input the id of the tool to rent (1-{0}): ", counter);
                if (int.TryParse(ReadLine(), out int input))
                    id = input;
                WriteLine();
            } while (id < 0 || id > counter);

            // Searching for the tool to rent and updating its quantity if available
            tool = category.FirstChild;
            counter = 0;
            while (tool != null)
            {
                ++counter;
                if (counter == id)
                {
                    if (tool.Quantity > 0)
                    {
                        --tool.Quantity;
                        break;
                    }
                    // When the tool is not in stock
                    else
                    {
                        WriteLine("Tool not currently available.");
                        WriteLine("Please press 0 to return to the main menu.");
                        ReadKey();
                        return;
                    }
                }
                tool = tool.NextSibling;
            }

            // Attempting to find an empty spot in the rentals array
            for (int i = 0; i < rentals.Length; ++i)
            {
                // If we have found an empty spot
                if (rentals[i] == null)
                {
                    Clear();
                    Write("Please enter the renter's full name: ");
                    string renterName = ReadLine();
                    WriteLine();
                    Write("Please enter the renter's contact phone number: ");
                    string phone = ReadLine();
                    WriteLine();

                    // Logging the rental
                    rentals[i] = new Rental(tool.Name, category.Name, renterName, phone);
                    WriteLine("Tool rented successfully.");
                    WriteLine("Please press 0 to return to the main menu.");
                    ReadKey();
                    return;
                }
                // Else if no empty spots remain
                else if (i == rentals.Length - 1)
                {
                    WriteLine("Maximum rentals reached.");
                    WriteLine("Please return a tool first.");
                    WriteLine("Press 0 to return to the main menu");
                    ReadKey();
                    return;
                }
            }
        }

        static void ReturnTool()
        {
            int i = 0;
            ConsoleKeyInfo c;
            do
            {
                // If there is an active rental in this slot
                if (rentals[i] != null)
                {
                    Clear();
                    WriteLine("Rental {0}", i + 1);
                    WriteLine(SEPARATOR);
                    WriteLine("{0,-12}{1}", "Category:", CapitaliseWords(rentals[i].ToolCategory));
                    WriteLine("{0,-12}{1}", "Tool:", rentals[i].ToolName);
                    WriteLine("{0,-12}{1}", "Renter:", rentals[i].RenterName);
                    WriteLine("{0,-12}{1}", "Phone:", rentals[i].ContactPhone);

                    WriteLine();
                    WriteLine("Use the left and right arrow keys to navigate, or 0 to return.");
                    WriteLine("To mark this rental as returned, press Enter.");
                    c = ReadKey();
                    if (c.Key == ConsoleKey.LeftArrow && i > 0)
                        --i;
                    else if (c.Key == ConsoleKey.RightArrow && i < (MAX_RENTALS - 1))
                        ++i;
                    else if (c.Key == ConsoleKey.Enter)
                    {
                        WriteLine();
                        WriteLine("To confirm, please press Y");
                        c = ReadKey();
                        if (c.Key == ConsoleKey.Y)
                        {
                            // Getting the category based on the stored tool category name
                            category = toolStorage.FirstChild;
                            while (category != null)
                            {
                                // If we reach the option the user selected, category will be set correctly
                                if (category.Name == rentals[i].ToolCategory)
                                    break;
                                category = category.NextSibling;
                            }
                            category.AddTool(new Tool(rentals[i].ToolName, "", 1));
                            rentals[i] = null;
                        }
                    }
                }
                else
                {
                    Clear();
                    WriteLine("Rental {0}", i + 1);
                    WriteLine(SEPARATOR);
                    WriteLine("Empty");
                    WriteLine();
                    WriteLine();
                    WriteLine();

                    WriteLine();
                    WriteLine("Use the left and right arrow keys to navigate, or 0 to return.");
                    c = ReadKey();
                    if (c.Key == ConsoleKey.LeftArrow && i > 0)
                        --i;
                    else if (c.Key == ConsoleKey.RightArrow && i < (MAX_RENTALS - 1))
                        ++i;
                }
            } while (c.Key != ConsoleKey.D0 && c.Key != ConsoleKey.NumPad0);
        }

        // Setting up the toolStorage as a tree structure containing preset tool categories
        static void InitialiseToolStorage()
        {
            // An array of initialCategories to assist with initialising the toolStorage
            string[] initialCategories = { "gardening", "flooring", "fencing", "measuring", "cleaning",
                "painting", "electronic", "electricity", "automotive" };

            // Creating the root of the tree
            toolStorage = new Tool("root", null, -1);

            // Setting the children of root to be the different tool categories
            toolStorage.FirstChild = new Tool(initialCategories[0], null, -1);
            category = toolStorage.FirstChild;
            for (int i = 1; i < initialCategories.Length; ++i)
            {
                category.NextSibling = new Tool(initialCategories[i], null, -1);
                category = category.NextSibling;
            }
        }

        // Setting up the toolStorage tree to contain preset tool data
        static void InitialiseTools()
        {
            category = toolStorage.FirstChild;
            while (category != null)
            {
                switch (category.Name)
                {
                    case "gardening":
                        category.AddTool(new Tool("Trojan Fibreglass D Handle Garden Fork",
                            "Sharp tines, perfect for your heavy gardening project.", 3));
                        category.AddTool(new Tool("Fiskars 330 mm Fixed Blade Garden Saw",
                            "Cuts on both the push and pull stroke.", 1));
                        category.AddTool(new Tool("Sherlock 80 L Garden Poly Tray Wheelbarrow",
                            "Built to last a lifetime.", 3));
                        category.AddTool(new Tool("Saxon Square Mouth Concrete Shovel",
                            "Ideal for any hole.", 5));
                        category.AddTool(new Tool("Trojan 900 g Timber Hatchet",
                            "Hardwood handle, great for light work.", 4));
                        break;

                    case "flooring":
                        category.AddTool(new Tool("DTA 200 mm Vacuum Suction Cup",
                            "Great for glass, tiles and porcelain.", 2));
                        category.AddTool(new Tool("DTA 2000 Piece Wedge Levelling Spacer",
                            "Great for lippage free surfaces.", 4));
                        category.AddTool(new Tool("DTA 750 mm Sigma Tile Cutter",
                            "Accurate cutting action with a removable measuring bar.", 1));
                        category.AddTool(new Tool("QEP Adhesive Grass Trowel",
                            "Comes with a comfortable soft-grip polymer handle.", 5));
                        category.AddTool(new Tool("DTA Grout Clean Up System",
                            "For messes, big and small.", 2));
                        break;

                    case "fencing":
                        category.AddTool(new Tool("Star Post Driver 80 NB",
                            "Pipe body of 80 mm nominal bore.", 1));
                        category.AddTool(new Tool("Star Post Lifter",
                            "Single pump action handle.", 1));
                        category.AddTool(new Tool("Ringmaster Pneumatic Clip Gun",
                            "Clips wires together in a faster and easier way.", 2));
                        break;

                    case "measuring":
                        category.AddTool(new Tool("Stanley 5 m 16' Tylon Tape",
                            "Tough rubber and plastic case, 19 mm wide blade.", 5));
                        category.AddTool(new Tool("Stanley 60 m Open Reel Fibreglass Measuring Tape",
                            "Durable case with a 1/2\" wide blade.", 4));
                        category.AddTool(new Tool("Craftright Lightweight Measuring Wheel",
                            "Twin wheels with an Extendable handle.", 2));
                        break;

                    case "cleaning":
                        category.AddTool(new Tool("Kinetic Black Rubber Plunger",
                            "Lightweight and suitable for any toilet.", 5));
                        category.AddTool(new Tool("Oates 1 m Nippers Reacher Pick Up Tool",
                            "Extra long handle great for picking up sharp objects.", 4));
                        category.AddTool(new Tool("Kinetic 180 cm Pipe Drain Cleaning Tool",
                            "Quickly clears any blocked drains.", 2));
                        category.AddTool(new Tool("Cyclone Gutter Cleaning Tool",
                            "450 mm long, removes leaves and other fire hazards.", 5));

                        break;

                    case "painting":
                        category.AddTool(new Tool("Trojan 100 mm Knife Wall Stripper",
                            "Corrosion resistant with an aluminium alloy body.", 3));
                        category.AddTool(new Tool("Trojan Pail Opener",
                            "Composed of ABS plastic.", 4));
                        category.AddTool(new Tool("Trojan Paint Mixer",
                            "Quickly clears any blocked drains.", 2));
                        category.AddTool(new Tool("Paint Partner 63 mm Synthetic Paint Brush",
                            "Suitable for all general painting.", 5));
                        break;

                    case "electronic":
                        category.AddTool(new Tool("Hakko FX-888D Digital Soldering Station",
                            "26 V AC output, 1.2 m soldering iron cable.", 3));
                        category.AddTool(new Tool("Solder sucker",
                            "Quick suck action to remove solder.", 3));
                        category.AddTool(new Tool("Fine-tip curved tweezers",
                            "ESD safe, 120 mm long, 9 mm gap while open.", 5));
                        break;

                    case "electricity":
                        category.AddTool(new Tool("Klein Tools 600 V 10 A Manual Ranging Multimeter",
                            "Durable, can withstand drops of 1 m.", 3));
                        category.AddTool(new Tool("Deta Power Outlet Tester Plug",
                            "Safe, compact and great for detecting wiring faults.", 2));
                        category.AddTool(new Tool("Projecta 12 v Battery Tester",
                            "Bright LED screen, safe and simple to use.", 1));
                        break;

                    case "automotive":
                        category.AddTool(new Tool("Projecta 12 v Battery Charger Pro",
                            "6 stage charger which can charge a variety of batteries.", 1));
                        category.AddTool(new Tool("Projecta 200 A Surge Protected Jumper Leads",
                            "Polarity protected with insulated clamps.", 2));
                        category.AddTool(new Tool("Kincrome Digital Smart Tyre Gauge",
                            "Has a built-in tyre deflating function.", 1));
                        category.AddTool(new Tool("Kincrome 1850kg Axle Stand Twin Pack Set",
                            "Holds up to 1850 kg.", 2));
                        break;
                }
                category = category.NextSibling;
            }
        }
    }
}
