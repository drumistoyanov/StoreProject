using Store.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Store.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            this.Sales = new List<Sale>();
        }
        public string Password { get; set; }
        [ForeignKey("FK_User_Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public List<Sale> Sales { get; set; }
    }
}
