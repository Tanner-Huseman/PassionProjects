using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
   public class PurchaseMenu //feed money/ select produc / finish transaction / current money provided
    {
        //PROPERTY
        VendingMachine Machine { get; set; }
        string Selection { get; set; }
        bool selectionOk { get; set; }

        //CONSTRUCTOR
        public PurchaseMenu(VendingMachine vendingMachine)
        {
            this.Machine = vendingMachine;
        }

        //METHODS

        public void PurchaseMenuDisplay()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(" |*************************************|");
            Console.WriteLine(" | * *                             * * |");
            Console.WriteLine(" |* *        Vendo-Matic 800        * *|");
            Console.WriteLine(" | * *                             * * |");
            Console.WriteLine(" |*************************************|");
            Console.WriteLine( );
            Console.WriteLine("    (1) Feed Money");
            Console.WriteLine();
            Console.WriteLine("    (2) Select Product");
            Console.WriteLine();
            //display full items
            //select item
            //logic to check valid slot number and enough money
            //If Valid, vend item and adjust balance.
            Console.WriteLine("    (3) Finish Transaction");
            Console.WriteLine();
            Console.WriteLine($"    Balance: ${Machine.Balance}");
            Console.WriteLine();
            Console.Write("    Please make a *Selection: ");

            
            //string selection = Console.ReadLine();

            string myOptions2 = Console.ReadLine();

            switch(myOptions2)
            {
                case "1":                    
                    decimal depositAmount = 0;                   
                    bool isDepositingMoney = true;
                    while (isDepositingMoney)
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine($"    Current Money Provided: ${Machine.Balance}");
                        Console.WriteLine();
                        Console.Write("    Insert Money: (0 to return) $ ");

                        try
                        {
                            depositAmount = decimal.Parse(Console.ReadLine());
                        }
                        catch (ArgumentNullException ex)
                        {
                            Console.WriteLine($"    please enter a valid number. {ex.Message}");
                            Console.ReadLine();
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"    please enter a valid number. {ex.Message}");
                            Console.ReadLine();
                        }

                        if (depositAmount == 0)
                        {
                            isDepositingMoney = false;
                        }
                        else if(depositAmount > 0)
                        {
                        DepositMoney depositMoney = new DepositMoney(depositAmount, Machine);
                        depositMoney.Deposit();
                        //Console.WriteLine($"    Current Money Provided: ${Machine.Balance}");
                        }
                       
                    }
                    PurchaseMenuDisplay();
                    break;
                case "2":
                    DisplayVendingMachineItems();
                    CheckSlotNumber checkSlotNumber = new CheckSlotNumber(Selection, Machine);
                    selectionOk = checkSlotNumber.MakeItemSelection();
                    CompletePurchase();
                    break;
                case "3":
                    Exit();
                    break;
                default:
                    PurchaseMenuDisplay();
                    break;
            }
        }
            //OPTION 1 FEED MONEY
            //ADD LOG
        
        //// Enter item price create a variable 
        //public int CostOfItem = Machine.Item[].Price;
        //public int FeedMoney { get; set; }
        //public void VendingMachine()
        //{
        //    FeedMoney = 0;

        //}

        //public void DepositMoney()
        //{
        //    Console.Write("Insert Money: $");
        //    decimal money = decimal.Parse(Console.ReadLine());

        //    // the only valid money entries $1, $2, $5, or $10.
        //    switch (money)
        //    {
        //        case (1):
        //            FeedMoney = 1;
        //            break;
        //        case (2):
        //            FeedMoney = 2;
        //            break;
        //        case (5):
        //            FeedMoney = 5;
        //            break;
        //        case (10):
        //            FeedMoney = 10;
        //            break;
        //        default:
        //            Console.WriteLine("Invalid Entry");
        //            Console.ReadLine();
        //            PurchaseMenuDisplay();
        //            break;
        //    }
        //    Machine.Balance += FeedMoney;
        //    LogClass logClass = new LogClass();
        //    logClass.FeedMoneyLog(FeedMoney, Machine.Balance);
        //    PurchaseMenuDisplay();
        //}



        //OPTION 2
        //ADD LOG
        
        void DisplayVendingMachineItems()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(" |*************************************|");
            Console.WriteLine(" | * *                             * * |");
            Console.WriteLine(" |* *     Vendo-Matic 800 : Menu    * *|");
            Console.WriteLine(" | * *                             * * |");
            Console.WriteLine(" |*************************************|");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Quantity  | Slot Number |   Price   |  Item");
            //STEP 5
            foreach (Item item in Machine.Items)
            {
                if (item.Remaining == 0)
                {
                    Console.WriteLine($" {item.Name} SOLD OUT");
                }
                else
                {
                    Console.WriteLine($"   {item.Remaining}      |     {item.SlotNumber}      |   ${item.Price}   |  {item.Name}");
                }
            }
            Console.WriteLine();
            Console.Write("    Please Enter Slot Number: ");
            Selection = Console.ReadLine().ToUpper();
        }


        //private void MakeItemSelection(string selection)
        //{// create a booling to check if the customer selection is okay
        //    bool selectionOk = false;
        //    foreach (Item item in Machine.Items)
        //    {
        //        if (selection == item.SlotNumber)
        //        {
        //            selectionOk = true;
        //        }
        //    }
        public void CompletePurchase()
        {

            if(selectionOk == false)
            {
                Console.WriteLine("    Invalid Selection");
                Console.ReadLine();
                PurchaseMenuDisplay();
            }

           // if selection is correct, connect to item through index.
            int selectedItemIndex = -1;
                if (selectionOk)
                {
                    for (int i = 0; i < Machine.Items.Count; i++)
                    {
                        if (Machine.Items[i].SlotNumber == Selection)
                        {
                            selectedItemIndex = i;
                        }
                    }
                }


                Item selectedItem = Machine.Items[selectedItemIndex];
                // Check Machine balance
                bool checkTotal = false;

                if (Machine.Balance >= selectedItem.Price)
                {
                    checkTotal = true;
                }
                else
                {
                    checkTotal = false;
                    Console.WriteLine();
                    Console.WriteLine("    Insufficient Funds");
                    Console.ReadLine();
                    PurchaseMenuDisplay();
                }


                bool isInStock = false;
                if (selectedItem.Remaining > 0)
                {
                    isInStock = true;
                }
                else
                {
                    Console.WriteLine($"    {selectedItem.Name} SOLD OUT");
                    Console.ReadLine();
                    PurchaseMenuDisplay();
                }

                
                // Comment
                string message="";
                switch (selectedItem.Type)
                {
                    case "Chip":
                        message = ("crunch crunch, yum!");
                        break;
                    case "Candy":
                        message = "munch munch, yum!";
                        break;
                    case "Drink":
                        message = "glug glug, yum!";
                        break;
                    case "Gum":
                        message = "chew chew, yum!";
                        break;
                }
                        if (isInStock == true && checkTotal == true)
                {
                    Machine.Balance -= selectedItem.Price;
                    selectedItem.Remaining--;
                    Console.WriteLine();
                    Console.WriteLine($"    {selectedItem.Name} ${selectedItem.Price}");
                    Console.WriteLine($"    {message}");
                    Console.WriteLine();
                    Console.WriteLine($"    Remaining Balance: ${Machine.Balance}");
                    LogClass logClass = new LogClass();
                    logClass.PurchaseLog(selectedItem.Name, selectedItem.SlotNumber, (Machine.Balance + selectedItem.Price), Machine.Balance);
                    Console.ReadLine();
                    PurchaseMenuDisplay();
                }
            }
        


            //Option 3 EXIT
            //ADD LOG
            public void Exit()
            {
            Console.WriteLine();
            Console.WriteLine("    Successful Transaction");

            decimal finalChange = Machine.Balance * 100;
            //logic to make Quarters Dimes Nickles

            if(Machine.Balance > 0.00M)
            {

                decimal quarter = 25M;
                int quarterCount = 0;
                decimal dime = 10M;
                int dimeCount = 0;
                decimal nickel = 5M;
                int nickelCount = 0;


                if (finalChange >= quarter)
                {
                    quarterCount = (int)(finalChange / quarter);
                    dimeCount = (int)((finalChange % quarter) / dime);
                    nickelCount = (int)(((finalChange % quarter) % dime) / nickel);
                }
                else if (finalChange >= dime)
                {
                    dimeCount = (int)(finalChange / dime);
                    nickelCount = (int)((finalChange % dime) / nickel);
                }
                else if (finalChange > 0)
                {
                    nickelCount = (int)(finalChange / nickel);
                }
                Console.WriteLine($"    Your change: {quarterCount} Quarters, {dimeCount} Dimes, {nickelCount} Nickels.");
                Machine.Balance = 0;
                Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine(" |*************************************|");
                Console.WriteLine(" | * *                             * * |");
                Console.WriteLine(" |* *           Good-Bye!           * *|");
                Console.WriteLine(" | * *                             * * |");
                Console.WriteLine(" |*************************************|");
                Console.ReadLine();
                LogClass logClass = new LogClass();
                logClass.ExitLog(finalChange/ 100M, Machine.Balance);
                MainMenu mainMenu = new MainMenu(Machine);
                mainMenu.MainMenuDisplay();
             }
            else
            {
                Console.WriteLine(" |*************************************|");
                Console.WriteLine(" | * *                             * * |");
                Console.WriteLine(" |* *           Good-Bye!           * *|");
                Console.WriteLine(" | * *                             * * |");
                Console.WriteLine(" |*************************************|");
                Console.ReadLine();
                MainMenu mainMenu = new MainMenu(Machine);
                mainMenu.MainMenuDisplay();
            }
        }
    }
}

//while (!selectionOk)
    //{
    //    switch (selection)
    //    {
    //        case "A1":
    //            selectionOk = true;
    //            //ReturnChange();
    //            break;
    //        case "A2":
    //            selectionOk = true;
    //            //ReturnChange();
    //            break;
    //        case "A3":
    //            selectionOk = true;
    //            //ReturnChange();
    //            break;
    //        case "A4":
    //            selectionOk = true;
    //           // ReturnChange();
    //            break;
    //        case "B1":
    //            selectionOk = true;
    //            //ReturnChange();
    //            break;
    //        case "B2":
    //            selectionOk = true;
    //            //ReturnChange();
    //            break;
    //        case "B3":
    //            selectionOk = true;
    //           // ReturnChange();
    //            break;
    //        case "B4":
    //            selectionOk = true;
    //            //ReturnChange();
    //            break;
    //        case "C1":
    //            selectionOk = true;
    //           // ReturnChange();
    //            break;
    //        case "C2":
    //            selectionOk = true;
    //           // ReturnChange();
    //            break;
    //        case "C3":
    //            selectionOk = true;
    //           // ReturnChange();
    //            break;
    //        case "C4":
    //            selectionOk = true;
    //           // ReturnChange();
    //            break;
    //        case "D1":
    //            selectionOk = true;
    //          //  ReturnChange();
    //            break;
    //        case "D2":
    //            selectionOk = true;
    //            //ReturnChange();
    //            break;
    //        case "D3":
    //            selectionOk = true;
    //           // ReturnChange();
    //            break;
    //        case "D4":
    //            selectionOk = true;
    //           // ReturnChange();
    //            break;
    //        default:
    //            Console.WriteLine("Invalid Selection");
    //            Console.ReadLine();
    //            //selection =return to purchase menu;
    //            //selectionOk = false;
    //            PurchaseMenuDisplay();
    //            break;
    //    }