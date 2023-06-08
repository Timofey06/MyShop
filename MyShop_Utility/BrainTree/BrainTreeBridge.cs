using Braintree;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_Utility.BrainTree
{
    public class BrainTreeBridge : IBrainTreeBridge
    {
        public SettingsBrainTree settingsBrainTree { get; set; }
        private IBraintreeGateway gateway; 
        public BrainTreeBridge(IOptions<SettingsBrainTree> options)
        {
            this.settingsBrainTree = options.Value;
        }

        public IBraintreeGateway CreateGateWay()
        {
            return new BraintreeGateway(settingsBrainTree.Environment, settingsBrainTree.MerchantId,
                settingsBrainTree.PublicKey, settingsBrainTree.PrivateKey);
        }

        public IBraintreeGateway GetGateWay()
        {
            if (gateway==null)
            {
                gateway = CreateGateWay();
            }
            return gateway;
        }
    }
}
