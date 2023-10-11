using Microsoft.AspNetCore.Mvc.Rendering;
using net_il_mio_fotoalbum.Models.DatabaseModels;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoFormModel
    {
        public Photo Photo { get; set; }
        public IFormFile? ImageFormFile { get; set; }

        // gestione select multipla nelle viste
        public List<SelectListItem>? Categories { get; set; } 
        public List<string> SelectedCategoriesId { get; set; }
    }
}
