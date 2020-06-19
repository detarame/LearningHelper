using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    [Table("People")]
    public class Person
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("Id")]
        public Int16 Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }
        public Int16 MainLanguageId { get; set; }
        [ForeignKey("MainLanguageId")]
        public Language MainLanguage { get; set; }
        // related table
        public List<PersonVocabulary> personVocabularies { get; set; }
        public Person()
        {
            personVocabularies = new List<PersonVocabulary>();
        }
    }
}
