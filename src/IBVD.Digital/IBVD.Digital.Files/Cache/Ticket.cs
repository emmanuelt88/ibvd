using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.IBVD.Cache
{
    public class Ticket
    {
        private string id;

        public Ticket(string id)
        {
            this.id = id;
        }

        public string Value
        {
            get { return id.ToString(); }
        }

        public static Ticket Create(string id)
        {
            return new Ticket(id);
        }

        public static Ticket Create()
        {
            Guid guid = Guid.NewGuid();
            return new Ticket(guid.ToString());
        }
    }
}
