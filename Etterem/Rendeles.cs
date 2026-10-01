namespace EtteremAPI.Models
{
    public class Rendeles
    {
        public int Id { get; set; }

        public string Dish { get; set; }

        public string Description { get; set; }

        public DateTime OrderTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public int VendegId { get; set; }
    }
}