using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    [Table("WordValues")]
    public class Word
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int16 Id { get; set; }
        public Int16 WordId { get; set; }
        [ForeignKey("WordId")]
        public virtual WordId WordIdReference { get; set; }
        public Int16 LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }
        public string Value { get; set; }
        // related table
        public List<VocabularyWord> VocabularyWords { get; set; }
        public Word()
        {
            VocabularyWords = new List<VocabularyWord>();
        }
    }
}
