using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models.DatabaseModels
{
    public class Category
    {
        // ATTRIBUTI
        public int Id { get; set; }
        [Required(ErrorMessage = "Il titolo della categoria è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il titolo della categoria non può superare i 100 caratteri.")]
        public string Title { get; set; } = string.Empty;

        // relazione N:N 
        public List<Photo> Photos { get; set; }

        // COSTRUTTORE
        public Category()
        {

        }
    }
}
