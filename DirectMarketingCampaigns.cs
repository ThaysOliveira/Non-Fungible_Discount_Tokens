using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.ComponentModel;
using System.Numerics;

namespace Neo.SmartContract
{
    public class DirectMarketingCampaigns : Framework.SmartContract
    {
        public static byte[] SCOwner = "AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y".ToScriptHash(); // Owner of the Smart Contract, able to Register Campaings
        private static readonly byte[] PREFIX_REGISTERED_CAMPAIGNS = "REGISTERED_CAMPAIGNS".AsByteArray();
        private static readonly byte[] PREFIX_REGISTERED_CAMPAIGNS_BUDGET = "REGISTERED_CAMPAIGNS_BUDGET".AsByteArray();
        private static readonly byte[] PREFIX_REGISTERED_CAMPAIGNS_OWNER = "REGISTERED_CAMPAIGNS_OWNER".AsByteArray();
        
        
        public static Object Main(string operation, params object[] args)
        {
            if (Runtime.Trigger == TriggerType.Application)
            {
                if (operation == "registerCampaign") return RegisterCampaign(args); // Register a campaign with: a maximun budget and campaignOwner
                if (operation == "getRegisteredCampaigns") return GetRegisteredCampaigns(args); // Get number of registered campaigns
                if (operation == "getCampaignBudget") return GetCampaignBudget(args); // Get campaign total budget
                if (operation == "getCampaignOwner") return GetCampaignOwner(args); // Get campaign owner
                if (operation == "registerDiscountVoucher") return RegisterDiscountVoucher(args); // Register discount voucher: campaignID, number of vouchers, maximum discount value, discount product, expire date (block)
                if (operation == "getProductNumberOfVouchers") return GetProductNumberOfVouchers(args); // get total number of vouchers: campaignID, voucherID
                if (operation == "transferVoucher") return TransferVoucher(args); // transferVoucher: campaignID, voucherID, addressOfNewOwner, discountID
                if (operation == "getVoucherValue") return GetVoucherValue(args); // get voucher value: campaignID, voucherID, clientID
                if (operation == "registerClients") return RegisterClients(args); // Register the profile of a client that is willing to participate in the DM campaigns: Specific data
                if (operation == "getClientVouchers") return GetClientVouchers(args); // List vouchers owned by a client: clientID
                if (operation == "setClientLimit") return setClientLimit(args); // Set maximum number of voucher to be received for a given client
                if (operation == "changeOwner") return ChangeSCOwner(args); // Change Smart Contract Owner
                if (operation == "changeCampaignOwner") return ChangeCampaignOwner(args); // Change Campaign Owner: campaignID
                if (operation == "redeemVoucher") return RedeemVoucher(args); // Clients uses the voucher: voucherHash
            }
            return true;
        }
        
        public static BigInteger RegisterCampaign(object[] args)
        {
            if (!Runtime.CheckWitness(SCOwner)) 
            {
                Runtime.Notify("You are not allowed for this operation...:");
                return 0;
            }
            
            if (args.Length != 2) return 0;
            byte[] cOwner = (byte[])args[0];
            BigInteger maxBudget = (BigInteger)args[1];
            
            if (maxBudget <= 0) return 0;
            
            BigInteger registeredCampaigns = Storage.Get(PREFIX_REGISTERED_CAMPAIGNS).AsBigInteger();
            registeredCampaigns = registeredCampaigns + 1;
            Storage.Put(PREFIX_REGISTERED_CAMPAIGNS,registeredCampaigns);
            
            byte[] idAsByteArray = registeredCampaigns.AsByteArray();
            Storage.Put(idAsByteArray.Concat(PREFIX_REGISTERED_CAMPAIGNS_OWNER),cOwner);
            Storage.Put(idAsByteArray.Concat(PREFIX_REGISTERED_CAMPAIGNS_BUDGET),maxBudget);
            
            return registeredCampaigns;
        }

         public static BigInteger GetRegisteredCampaigns(object[] args)
         {
             BigInteger registeredCampaigns = Storage.Get(PREFIX_REGISTERED_CAMPAIGNS).AsBigInteger();
             Runtime.Notify("registeredCampaigns is:");
             Runtime.Notify(registeredCampaigns);
             return registeredCampaigns;
         }
         
         public static BigInteger GetCampaignBudget(object[] args)
         {
             if (args.Length != 1) return 0;
             Runtime.Notify("Params ok:");
             BigInteger campaignID = (BigInteger)args[0];
             Runtime.Notify("Getting campaignID:");
             Runtime.Notify(campaignID);
             
             byte[] idAsByteArray = campaignID.AsByteArray();
             BigInteger budget = Storage.Get(idAsByteArray.Concat(PREFIX_REGISTERED_CAMPAIGNS_BUDGET)).AsBigInteger();
             Runtime.Notify("Budget for campaign:");
             Runtime.Notify(campaignID);
             Runtime.Notify("is: ");
             Runtime.Notify(budget);
             
             return budget;
         }
         
         public static byte[] GetCampaignOwner(object[] args)
         {
             if (args.Length != 1) return null;
             Runtime.Notify("Params ok:");
             BigInteger campaignID = (BigInteger)args[0];
             Runtime.Notify("Getting campaignID:");
             Runtime.Notify(campaignID);
             
             byte[] idAsByteArray = campaignID.AsByteArray();
             byte[] owner = Storage.Get(idAsByteArray.Concat(PREFIX_REGISTERED_CAMPAIGNS_OWNER));
             Runtime.Notify("owner for campaign:");
             Runtime.Notify(campaignID);
             Runtime.Notify("is: ");
             Runtime.Notify(owner);
             
             return owner;
         }
         
         public static bool RegisterDiscountVoucher(object[] args)
         {
             if (args.Length != 1) return false;
             Runtime.Notify("RegisterDiscountVoucher Params ok:");
             BigInteger campaignID = (BigInteger)args[0];
             Runtime.Notify("Getting campaignID:");
             Runtime.Notify(campaignID);
             byte[] idAsByteArray = campaignID.AsByteArray();
             byte[] owner = Storage.Get(idAsByteArray.Concat(PREFIX_REGISTERED_CAMPAIGNS_OWNER));
             Runtime.Notify("Going to check witness of: ");
             //Runtime.Notify(owner);
             
             if (!Runtime.CheckWitness(owner)) 
             {
                Runtime.Notify("You are not allowed for this operation...:");
                return false;
             }
             Runtime.Notify("You are the owner of this campaign. Allowed to modify and register vouchers.");
             return true;
         }
         public static bool GetVoucherValue(object[] args)
         {
             return true;
         }
         
         public static bool GetProductNumberOfVouchers(object[] args)
         {
             return true;
         }
         
         public static bool TransferVoucher(object[] args)
         {
             return true;
         }
         
         public static bool RegisterClients(object[] args)
         {
             return true;
         }
         
         public static bool GetClientVouchers(object[] args)
         {
             return true;
         }
         
         public static bool setClientLimit(object[] args)
         {
             return true;
         }
         
         public static bool ChangeSCOwner(object[] args)
         {
             return true;
         }
         
         public static bool ChangeCampaignOwner(object[] args)
         {
             return true;
         }
         
         public static bool RedeemVoucher(object[] args)
         {
             return true;
         }

    
    }
}
