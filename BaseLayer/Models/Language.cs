using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Language
    {
        [Column("Id")]
        public Int16 Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        // related tables
        public List<Word> WordValues { get; set; }
        public List<Person> People { get; set; }
        public List<Vocabulary> Vocabularies { get; set; }
        public Language()
        {
            WordValues = new List<Word>();
            People = new List<Person>();
            Vocabularies = new List<Vocabulary>();
        }
    }
}
