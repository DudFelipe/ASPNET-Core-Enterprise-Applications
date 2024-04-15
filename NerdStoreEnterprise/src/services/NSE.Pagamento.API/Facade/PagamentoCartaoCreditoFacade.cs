using Microsoft.Extensions.Options;
using NSE.Pagamento.API.Models;
using NSE.Pagamentos.NerdsPag;

namespace NSE.Pagamento.API.Facade
{
    public class PagamentoCartaoCreditoFacade : IPagamentoFacade
    {
        private readonly PagamentoConfig _pagamentoConfig;

        public PagamentoCartaoCreditoFacade(IOptions<PagamentoConfig> pagamentoConfig)
        {
            _pagamentoConfig = pagamentoConfig.Value;    
        }

        public async Task<Transacao> AutorizarPagamento(Models.Pagamento pagamento)
        {
            var nerdsPagSvc = new NerdsPagService(_pagamentoConfig.DefaultApiKey, _pagamentoConfig.DefaultEncryptionKey);

            var cardHashGen = new CardHash(nerdsPagSvc)
            {
                CardNumber = pagamento.CartaoCredito.NumeroCartao,
                CardHolderName = pagamento.CartaoCredito.NomeCartao,
                CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
                CardCvv = pagamento.CartaoCredito.CVV
            };

            var cardHash = cardHashGen.Generate();

            var transacao = new Transaction(nerdsPagSvc)
            {
                CardHash = cardHash,
                CardNumber = pagamento.CartaoCredito.NumeroCartao,
                CardHolderName = pagamento.CartaoCredito.NomeCartao,
                CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
                CardCvv = pagamento.CartaoCredito.CVV,
                PaymentMethod = PaymentMethod.CreditCard,
                Amount = pagamento.Valor
            };

            return ParaTransacao(await transacao.AuthorizeCardTransaction());
        }

        public static Transacao ParaTransacao(Transaction transcation)
        {
            return new Transacao
            {
                Id = Guid.NewGuid(),
                Status = (StatusTransacao)transcation.Status,
                ValorTotal = transcation.Amount,
                BandeiraCartao = transcation.CardBrand,
                CodigoAutorizacao = transcation.AuthorizationCode,
                CustoTransacao = transcation.Cost,
                DataTransacao = transcation.TransactionDate,
                NSU = transcation.Nsu,
                TID = transcation.Tid
            };
        }

        public static Transaction ParaTransaction(Transacao transacao, NerdsPagService nerdsPagService)
        {
            return new Transaction(nerdsPagService)
            {
                Status = (TransactionStatus)transacao.Status,
                Amount = transacao.ValorTotal,
                CardBrand = transacao.BandeiraCartao,
                AuthorizationCode = transacao.CodigoAutorizacao,
                Cost = transacao.CustoTransacao,
                Nsu = transacao.NSU,
                Tid = transacao.TID
            };
        }

        public async Task<Transacao> CapturarPagamento(Transacao transacao)
        {
            var nerdsPagSvc = new NerdsPagService(_pagamentoConfig.DefaultApiKey, _pagamentoConfig.DefaultEncryptionKey);

            var transcation = ParaTransaction(transacao, nerdsPagSvc);

            return ParaTransacao(await transcation.CaptureCardTransaction());
        }

        public async Task<Transacao> CancelarAutorizacao(Transacao transacao)
        {
            var nerdsPagSvc = new NerdsPagService(_pagamentoConfig.DefaultApiKey, _pagamentoConfig.DefaultEncryptionKey);

            var transcation = ParaTransaction(transacao, nerdsPagSvc);

            return ParaTransacao(await transcation.CancelAuthorization());
        }
    }
}
