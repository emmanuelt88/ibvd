using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.Logic.Entities
{
    [Serializable]
    public abstract class ItemReunion
    {
        public abstract int GetId();
        public abstract TipoItemReunion GetTipo();

        internal void SetId(int id)
        {
            SetCurrentId(id);
        }

        protected abstract void SetCurrentId(int id);

        public abstract string GetText();

        public abstract string GetDetails();

    }
}
