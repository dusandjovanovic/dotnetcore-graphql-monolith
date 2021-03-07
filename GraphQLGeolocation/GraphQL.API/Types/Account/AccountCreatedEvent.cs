using GraphQL.Core.Data;

namespace GraphQL.API.Types.Account
{
    public class AccountCreatedEvent : AccountObject
    {
        public AccountCreatedEvent(IAccountRepository accountRepository) 
            : base(accountRepository) { }
    }
}