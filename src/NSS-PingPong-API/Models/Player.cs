using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NSS_PingPong_API.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public int FirebaseId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool LeftHanded { get; set; }
        public string Cohort { get; set; }

        public Stats Stats { get; set; }
    }
}
