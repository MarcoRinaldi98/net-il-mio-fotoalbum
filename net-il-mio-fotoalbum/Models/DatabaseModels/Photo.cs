using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models.DatabaseModels
{
    public class Photo
    {
        // ATTRIBUTI
        public int Id { get; set; }
        [Required(ErrorMessage = "Il titolo della foto è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il Titolo non può essere più lungo di 100 caratteri.")]
        public string Title { get; set; }
        [StringLength(500, ErrorMessage = "La descrizione della Foto non può superare i 500 caratteri.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "L'immagine è obbligatoria")]
        public string ImageUrl { get; set; }
        public byte[]? ImageFile { get; set; }
        public string ImageSrc => ImageFile is null ? ImageUrl is null ? "" : ImageUrl : $"data:image/png;base64,{Convert.ToBase64String(ImageFile)}";
        public bool IsVisible { get; set; }

        // relazione N:N 
        public List<Category>? Categories { get; set; }

        // COSTRUTTORE
        public Photo()
        {
        }
    }
}
