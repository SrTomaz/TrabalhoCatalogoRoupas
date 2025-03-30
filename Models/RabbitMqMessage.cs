namespace CatalogoRoupas.Models
{
    public class RabbitMQMessage
    {
        public string Tipo { get; set; } = string.Empty;
        public Roupa Dados { get; set; } = new Roupa();
    }
}
