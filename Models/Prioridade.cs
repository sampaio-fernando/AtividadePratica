using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiServico.Models
{
    [Table("prioridade")]
    public class Prioridade
    {
        [Column("id_pri")]
        public int Id { get; set; }
        [Column("nome_pri")]
        public string? Nome { get; set; }

        /*configuração de Relacionamento um-para-muitos (n:1)
         * entre chamado (n) e Prioridade (1)
         * configuração inversa
        */
        [JsonIgnore]
        public ICollection<Chamado> chamados { get; set; } = new List<Chamado>();
    }
}
