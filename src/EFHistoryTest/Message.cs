using EFHistory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFHistoryTest
{
    public class Message : HistoryEntity
    {
        public string Msg { get; set; }   
    }
}
