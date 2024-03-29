﻿using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;
using Colors.Net;

namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;

        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {
                // View your current balance
                Account account = tenmoApiService.GetBalance();
                console.PrintAccountBalance(account.balance);
                console.Pause();
            }

            if (menuSelection == 2)
            {   
                // View your past transfers
                Option2();   
            }

            if (menuSelection == 3)
            {
                // View your pending requests
                Option3();
            }

            if (menuSelection == 4)
            {
                // Send TE bucks
                //Option4();
                bool isSuccessful = Option4();
                while (!isSuccessful)
                {
                    isSuccessful = Option4();
                }
            }

            if (menuSelection == 5)
            {
                // Request TE bucks
                bool isSuccessful = Option5();
                while (!isSuccessful)
                {
                    isSuccessful = Option5();
                }
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }


        // impementation for option 2: "View your past transfers"
        private void Option2()
        {
            Account currentUserAccount = tenmoApiService.GetBalance();

            List <Transfer> transfers = tenmoApiService.GetTransfers();
            console.PrintTransfers(transfers, currentUserAccount.user_id, 2);

            int selectedTransferId = console.PromptForInteger("Please enter Transfer Id to view details (0 to cancel)", 0);
            bool isSelectedIdValid = false;

            if (selectedTransferId == 0)
            {
                RunAuthenticated();
            }
            else 
            {
                foreach(Transfer transfer in transfers)
                {
                    if(selectedTransferId == transfer.TransferId)
                    {
                        isSelectedIdValid = true;
                    }
                }
            }
            if (isSelectedIdValid)
            {
                try
                {
                    
                    Transfer transfer = tenmoApiService.GetTransferDetails(selectedTransferId); // Make request for transfer details
                    console.PrintTransferDetails(transfer); // Display transfer details
                    console.Pause();
                }
                catch (Exception ex)
                {
                    console.PrintError($" {selectedTransferId} is an invalid Transfer Id: {ex.Message}");

                }
            }
            else
            {
                console.PrintError($" {selectedTransferId} is an invalid Transfer Id");
                console.Pause();
            }
            

        }


        // implementation for Option 3: "View your pending requests"
        private void Option3()
        {
            Account currentUserAccount = tenmoApiService.GetBalance();

            List<Transfer> transfers = tenmoApiService.GetPendingTransfers();
            console.PrintTransfers(transfers, currentUserAccount.user_id, 1);

            if(transfers.Count == 0)
            {
                console.PrintError("No Pending Transfers Currently");
                console.Pause();
            }
            else
            {
                int selectedTransferId = console.PromptForInteger("Please enter Transfer Id to approve/reject (0 to cancel)", 0);
                bool isSelectedIdValid = false;


                foreach (Transfer transfer in transfers)
                {
                    if (selectedTransferId == transfer.TransferId)
                    {
                        isSelectedIdValid = true;
                    }
                }
                    

                if (isSelectedIdValid)
                {
                    console.PrintApproveRejectMenu();
                    int approveRejectMenuSelection = console.PromptForInteger("Please choose an option (0 to cancel)", 0);

                    switch (approveRejectMenuSelection)
                    {

                        // == don't approve or reject pending transfer == //
                        case 0:
                            break;

                        // == approves pending transfer == //
                        case 1:
                            // get single transfer details
                            Transfer transferToApprove = tenmoApiService.GetTransferDetails(selectedTransferId);

                            // update transfer status id
                            transferToApprove.TransferStatusId = 2;

                            // call api method to update transfer status id
                            tenmoApiService.UpdateStatusId(transferToApprove);

                            // update from and to user account balances
                            Account fromUserAccount = tenmoApiService.GetAccount(transferToApprove.FromUserId);
                            Account toUserAccount = tenmoApiService.GetAccount(transferToApprove.ToUserId);

                            fromUserAccount.balance -= transferToApprove.Amount;
                            toUserAccount.balance += transferToApprove.Amount;

                            tenmoApiService.UpdateBalance(fromUserAccount);
                            tenmoApiService.UpdateBalance(toUserAccount);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Transfer Approved!");
                            Console.ResetColor();
                            console.Pause();

                            break;



                        // == rejects pending transfer == //
                        case 2:
                            // get single transfer details
                            Transfer transferToReject = tenmoApiService.GetTransferDetails(selectedTransferId);

                            // update transfer status id
                            transferToReject.TransferStatusId = 3;

                            // call api method to update transfer status id
                            tenmoApiService.UpdateStatusId(transferToReject);

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Transfer Rejected");
                            Console.ResetColor();
                            console.Pause();

                            break;

                    }

                }
                else if(selectedTransferId == 0)
                {

                }
                else
                {
                    console.PrintError("Invalid Id");
                    console.Pause();
                }

            }

        }   


        // implementation for option 4: "Send TE bucks"
        private bool Option4()
        {

            // Send TE bucks
            List<User> users = tenmoApiService.GetUsers();
            console.PrintUsers(users);
            int toUserId = console.PromptForInteger("Id of the user you are sending to (0 to cancel) ", 0);
            Account account = tenmoApiService.GetBalance();

            if(toUserId == 0)
            {
                return true;
            }

            bool isUser = false;
            foreach (User user in users)
            {
                if (user.UserId == toUserId)
                {
                    isUser = true;
                }
            }

            if (isUser == false)
            {
                console.PrintError("Invalid user Id");
                console.Pause();
                return false;
            }
            if (toUserId == account.user_id)
            {
                console.PrintError("You can't send money to yourself");
                console.Pause();
                return false;
            }


            decimal transferAmount = console.PromptForDecimal("Enter amount to send");

            // condition check - to not allow zero or negative amount.
            if (transferAmount <= 0)
            {
                console.PrintError("Dollar amount must be greater than zero.");
                console.Pause();
                return false;
            }


            // condition check - cannot send more money than currently in account.
            decimal currentBalance = account.balance;
            if (transferAmount > currentBalance)
            {
                console.PrintError("Insufficient funds");
                console.Pause();
                return false;
            }



            // update to new user balance.
            Account toAccount = tenmoApiService.GetAccount(toUserId);
            toAccount.balance += transferAmount;
            Account updatedToAccount = tenmoApiService.UpdateBalance(toAccount);

            // Update from user balance.
            account.balance -= transferAmount;
            Account updatedFromAccount = tenmoApiService.UpdateBalance(account);

            // create a transfer object and send it to tenmoApiService.AddTransfer() method.
            Transfer newTransfer = new Transfer();
            newTransfer.AccountFrom = updatedFromAccount.account_id;
            newTransfer.AccountTo = updatedToAccount.account_id;
            newTransfer.Amount = transferAmount;
            newTransfer.TransferTypeId = 2;

            // sending transfer has an initial status of APPROVED.
            newTransfer.TransferStatusId = 2;

            tenmoApiService.AddTransfer(newTransfer);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Transfer Successful!");
            Console.ResetColor();
            console.Pause();
            return true;

        }


        // implementation for option 5: "Request TE bucks"
        private bool Option5()
        {
            // Request TE bucks
            List<User> users = tenmoApiService.GetUsers();
            console.PrintUsers(users);
            int fromUserId = console.PromptForInteger("Id of the user you are requesting money from (0 to cancel) ", 0);
            Account account = tenmoApiService.GetBalance();

            if (fromUserId == 0)
            {
                return true;
            }

            bool isUser = false;
            foreach (User user in users)
            {
                if (user.UserId == fromUserId)
                {
                    isUser = true;
                }
            }

            if (isUser == false)
            {
                console.PrintError("Invalid user Id");
                console.Pause();
                return false;
            }
            if (fromUserId == account.user_id)
            {
                console.PrintError("You can't request money from yourself");
                console.Pause();
                return false;
            }


            decimal requestAmount = console.PromptForDecimal("Enter amount");

            // condition check - to not allow zero or negative amount.
            if (requestAmount <= 0)
            {
                console.PrintError("Dollar amount must be greater than zero.");
                console.Pause();
                return false;
            }

            //get the account information of the requested user
            Account fromAccount = tenmoApiService.GetAccount(fromUserId);

            // create a transfer object and send it to tenmoApiService.AddTransfer() method.
            Transfer newTransfer = new Transfer();
            newTransfer.AccountFrom = fromAccount.account_id;
            newTransfer.AccountTo = account.account_id;
            newTransfer.Amount = requestAmount;
            // request transfer type = 1
            newTransfer.TransferTypeId = 1;

            // request transfer has an initial status of PENDING.
            newTransfer.TransferStatusId = 1;

            tenmoApiService.AddTransfer(newTransfer);
            
            // Colors.Net package - install a package
            RichString darkYellow = StringStaticMethods.DarkYellow("Request Pending...");
            ColoredConsole.WriteLine(darkYellow);
            console.Pause();
            return true;
        }

    }
}
