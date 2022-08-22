namespace Dataedo_RazorPages.Models
{
    public class ImportedObject : _ImportedObjectBaseClass
    {
        public string Schema { get; set; }

        public string ParentName { get; set; }
        public string ParentType { get; set; }

        public string DataType { get; set; }
        public bool IsNullable { get; set; }

        public uint NumberOfChildren { get; set; }
    }
}
