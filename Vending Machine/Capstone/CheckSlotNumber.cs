using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class CheckSlotNumber
    {
        //PROPERTY
        string Selection { get; set; }
        VendingMachine Machine { get; set; }

        //CONSTRUCTOR
        public CheckSlotNumber(string selection, VendingMachine vendingMachine)
        {
            Selection = selection.ToUpper();
            Machine = vendingMachine;
        }

        //METHOD
        public bool MakeItemSelection()
        {
            bool selectionOk = false;
            foreach (Item item in Machine.Items)
            {
                if (Selection == item.SlotNumber)
                {
                    selectionOk = true;
                }
            }
            return selectionOk;
        }
    }
}

//        private void 
//        {// create a booling to check if the customer selection is okay
//            bool selectionOk = false;
//            foreach (Item item in Machine.Items)
//            {
//                if (selection == item.SlotNumber)
//                {
//                    selectionOk = true;
//                }
//            }
//            if (selectionOk == false)
//            {
//                Console.WriteLine("Invalid Selection");
//                Console.ReadLine();
//                PurchaseMenuDisplay();
//            }

//        }
//}
