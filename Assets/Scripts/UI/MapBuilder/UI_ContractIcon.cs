using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilderUI
{
    partial class UI_ContractIcon
    {
        public ContractData Contract;

        public void SetContract(ContractData contract)
        {
            Contract = contract;
            icon = "ui://Res/" + Contract.Icon;
            text = Contract.Name;
        }
    }
}
