using Store.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Store.Entities
{
   public class Role: BaseEntity
    {
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
