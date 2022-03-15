using System;
using System.Collections.Generic;
using System.Text;
using Realms;
using MongoDB.Bson;

namespace App1.Models
{
    public class Surgery : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId? Id { get; set; }

        public string PatientId { get; set; }

        public string ProcedureName { get; set; }

        [Required]
        public IList<string> Staff { get; }

        public string Surgeon { get; set; }

        [Ignored]
        public string StaffList
        {
            get { return string.Join(",", Staff); }
        }
    }

}
