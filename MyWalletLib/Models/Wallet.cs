namespace MyWalletLib.Models
{
    public class Wallet
    {
        private readonly IWalletRepo _walletRepo;
        private readonly IBankingAccount _bankingAccount;
        private readonly IFee _fee;
        private readonly ILogger _log;

        public Wallet(IWalletRepo walletRepo, IBankingAccount bankingAccount, IFee fee, ILogger log)
        {
            _walletRepo = walletRepo;
            _bankingAccount = bankingAccount;
            _fee = fee;
            _log = log;
        }

        public void Withdraw(string account, decimal amount, string bankingAccount)
        {
            _log.Info($"account {account} amount {amount} banking account {bankingAccount}");
            _walletRepo.UpdateDelta(account, amount * -1);

            var fee = _fee.Get(bankingAccount);

            _bankingAccount.Saving(bankingAccount, amount - fee);
        }

        public void StoreValue(string bankingAccount, decimal amount, string account)
        {
            _bankingAccount.Withdraw(bankingAccount, amount);
            _walletRepo.UpdateDelta(account, amount);
        }
    }

    public interface ILogger
    {
        void Info(string msg);
    }

    public interface IFee
    {
        decimal Get(string bankingAccount);
    }

    public interface IBankingAccount
    {
        void Saving(string bankingAccount, decimal amount);
        void Withdraw(string bankingAccount, decimal amount);
    }

    public interface IWalletRepo
    {
        void UpdateDelta(string account, decimal amount);
    }
}