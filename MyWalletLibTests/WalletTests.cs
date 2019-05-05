using MyWalletLib.Models;
using NSubstitute;
using NUnit.Framework;

namespace MyWalletLibTests
{
    [TestFixture]
    public class WalletTests
    {
        private IBankingAccount _bankingAccount;
        private IFee _fee;
        private ILogger _log;
        private IWalletRepo _walletRepo;

        [SetUp]
        public void Setup()
        {
            _fee = Substitute.For<IFee>();
            _log = Substitute.For<ILogger>();
            _walletRepo = Substitute.For<IWalletRepo>();
            _bankingAccount = Substitute.For<IBankingAccount>();
        }

        [Test]
        public void storedValue_from_banking_to_wallet()
        {
            var wallet = new Wallet(_walletRepo, _bankingAccount, _fee, _log);

            wallet.StoreValue("919", 1000m, "joey");

            _bankingAccount.Received(1).Withdraw("919", 1000m);
            _walletRepo.Received(1).UpdateDelta("joey", 1000m);
        }

        [Test]
        public void withdrawal_from_wallet_to_banking_account_successfully()
        {
            var wallet = new Wallet(_walletRepo, _bankingAccount, _fee, _log);
            _fee.Get("919").ReturnsForAnyArgs(5m);

            wallet.Withdraw("joey", 1000m, "919");

            _walletRepo.Received(1).UpdateDelta("joey", -1000m);
            _bankingAccount.Received().Saving("919", 995);
        }
    }
}