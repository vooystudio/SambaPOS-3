using System;
using System.Collections.Generic;
using System.Linq;
using Samba.Domain.Models.Accounts;
using Samba.Presentation.Common;
using Samba.Services;

namespace Samba.Modules.AccountModule
{
    public class AccountSelectViewModel : ObservableObject
    {
        private readonly IAccountService _accountService;

        public AccountSelectViewModel(IAccountService accountService, AccountType AccountType)
        {
            _accountService = accountService;
            AccountType = AccountType;
        }

        public AccountSelectViewModel(IAccountService accountService, AccountType AccountType, int accountId, string accountName)
            : this(accountService, AccountType)
        {
            _accountName = accountName;
            _selectedAccountId = accountId;
        }

        public AccountSelectViewModel(IAccountService accountService, AccountType AccountType, string accountName, Action<string, int> updateAction)
            : this(accountService, AccountType)
        {
            _accountName = accountName;
            UpdateAction = updateAction;
        }

        protected Action<string, int> UpdateAction { get; set; }

        private string _accountName;
        public string AccountName
        {
            get { return _accountName ?? (_accountName = _accountService.GetAccountNameById(SelectedAccountId)); }
            set
            {
                _accountName = value;
                SelectedAccountId = _accountService.GetAccountIdByName(value);
                if (SelectedAccountId == 0)
                    RaisePropertyChanged(() => AccountNames);
                _accountName = null;
                RaisePropertyChanged(() => AccountName);
            }
        }

        public IEnumerable<string> AccountNames
        {
            get
            {
                if (AccountType == null) return null;
                return _accountService.GetCompletingAccountNames(AccountType.Id, AccountName);
            }
        }

        private AccountType _AccountType;
        private int _selectedAccountId;

        public AccountType AccountType
        {
            get { return _AccountType; }
            set
            {
                _AccountType = value;
                RaisePropertyChanged(() => AccountType);
                RaisePropertyChanged(() => TemplateName);
            }
        }

        public int SelectedAccountId
        {
            get { return _selectedAccountId; }
            set
            {
                _selectedAccountId = value;
                if (UpdateAction != null)
                    UpdateAction.Invoke(AccountName, value);
            }
        }

        public string TemplateName { get { return AccountType == null ? "" : string.Format("{0}:", AccountType.Name); } }
    }
}