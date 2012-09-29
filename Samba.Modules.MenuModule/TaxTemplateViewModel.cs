﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using FluentValidation;
using Samba.Domain.Models.Accounts;
using Samba.Domain.Models.Menus;
using Samba.Localization.Properties;
using Samba.Presentation.Common.ModelBase;

namespace Samba.Modules.MenuModule
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class TaxTemplateViewModel : EntityViewModelBase<TaxTemplate>
    {
        public string DisplayName
        {
            get
            {
                return string.Format("{0} - {1}", Name, (TaxIncluded ? Resources.Included : Resources.Excluded));
            }
        }

        private IEnumerable<AccountTransactionType> _accountTransactionTypes;
        public IEnumerable<AccountTransactionType> AccountTransactionTypes
        {
            get { return _accountTransactionTypes ?? (_accountTransactionTypes = Workspace.All<AccountTransactionType>()); }
        }

        public AccountTransactionType AccountTransactionType { get { return Model.AccountTransactionType; } set { Model.AccountTransactionType = value; } }

        public decimal Rate { get { return Model.Rate; } set { Model.Rate = value; } }

        public bool TaxIncluded { get { return Model.TaxIncluded; } set { Model.TaxIncluded = value; } }

        public override Type GetViewType()
        {
            return typeof(TaxTemplateView);
        }

        public override string GetModelTypeString()
        {
            return Resources.TaxTemplate;
        }
    }

    internal class TaxTemplateValidator : EntityValidator<TaxTemplate>
    {
        public TaxTemplateValidator()
        {
            RuleFor(x => x.AccountTransactionType).NotNull();
        }
    }
}
