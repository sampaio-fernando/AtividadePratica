using ApiServico.Models.Dtos;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiServico.Models
{
    [Table("chamado")]
    public class Chamado
    {
        [Column("id_cha")]
        public int Id { get; set; }

        [Column("titulo_cha")]
        public required string Titulo { get; set; }

        [Column("descricao_cha")]
        public required string Descricao { get; set; }

        [Column("data_abertura_cha")]
        public DateTime DataAbertura { get; set; } = DateTime.Now;

        [Column("data_fechamento_cha")]
        public DateTime? DataFechamento { get; set; }

        [Column("situacao_cha")]
        public string Status { get; set; } = "Aberto";

        /*configuração de Relacionamento um-para-muitos (n:1)
         * entre chamado (n) e Prioridade (1)
        */
        public virtual Prioridade? Prioridade { get; set; }
        [JsonIgnore]
        [Column("fk_id_pri")]
        public int PrioridadeId { get; set; }
    }
}
