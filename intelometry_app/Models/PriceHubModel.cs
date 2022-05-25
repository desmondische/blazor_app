using System.ComponentModel.DataAnnotations;

namespace intelometry_app.Models
{
    public class PriceHubModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
