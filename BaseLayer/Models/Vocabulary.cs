using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    [Table("Vocabularies")]
    public class Vocabulary
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
       // [NotMapped]
        public Int16 Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("CreationDate")]
        public DateTime CreationDate { get; set; }
        [Column("Theme")]
        public string Theme { get; set; }
        public Int16 LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }
        // related tables
        public List<VocabularyWord> vocabularyWords { get; set; }
        public List<PersonVocabulary> personVocabularies { get; set; }
        public Vocabulary()
        {
            vocabularyWords = new List<VocabularyWord>();
            personVocabularies = new List<PersonVocabulary>();
        }
        
    }
}
