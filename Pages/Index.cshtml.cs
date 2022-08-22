using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dataedo_RazorPages.Models;
using Dataedo_RazorPages.Data;

namespace Dataedo_RazorPages.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string FileName { get; set; }
        public bool NoFileName = false;

        public IList<ImportedObject> ImportedObjects = new List<ImportedObject>();

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            NoFileName = FileName == null;
            if (NoFileName)
            {
                return Page();
            }

            if (!System.IO.File.Exists(FileName))
            {
                if (!System.IO.File.Exists("Data/" + FileName))
                {
                    return NotFound();
                }
                else
                {
                    FileName = "Data/" + FileName;
                }
            }

            DataReader dataReader = new DataReader();
            dataReader.ImportAndPrintData(FileName, false);
            ImportedObjects = dataReader.ImportedObjects.ToList();

            return Page();
        }
    }
}