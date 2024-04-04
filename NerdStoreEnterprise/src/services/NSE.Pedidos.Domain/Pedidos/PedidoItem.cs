using NSE.Core.DomainObjects;

namespace NSE.Pedidos.Domain.Pedidos
{
    public class PedidoItem : Entity
    {
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public string? ProdutoImagem { get; private set; }

        //EF Relation
        public Pedido Pedido { get; set; }

        public PedidoItem()
        {
            
        }

        public PedidoItem(Guid pedidoId,
                          Guid produtoId,
                          string produtoNome,
                          int quantidade,
                          decimal valorUnitario,
                          string produtoImagem = null)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ProdutoImagem = produtoImagem;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }
}
