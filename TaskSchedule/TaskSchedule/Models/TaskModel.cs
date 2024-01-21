using SQLite;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TaskSchedule.Models
{
    [Table("Tasks")]
    public  class TaskModel
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public string TaskName {  get; set; }

        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime DueDate { get; set; }

    }
}
